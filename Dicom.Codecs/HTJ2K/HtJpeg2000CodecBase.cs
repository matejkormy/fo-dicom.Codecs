
namespace Dicom.Codecs.HTJ2K
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using Dicom.Codecs.Common;

    internal abstract class HtJpeg2000CodecBase : CodecBase
    {
        protected HtJpeg2000CodecBase()
        {
            DefaultParams = new HtJpeg2000CodecParams(OPJ_PROG_ORDER.PROG_UNKNOWN);
        }

        protected HtJpeg2000CodecParams DefaultParams { get; set; }

        public override unsafe byte[] Encode(byte[] input, FrameMetadata metadata, out FrameMetadata outputMetadata, CodecParams codecParams = null)
        {
            var j2kParams = codecParams as HtJpeg2000CodecParams ?? DefaultParams;
            outputMetadata = (FrameMetadata)metadata.Clone();

            byte[] frameData = input;

            try
            {
                frameData = ConvertToRgbIfNeeded(input, metadata);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot convert HTJ2K buffer data from PhotometricInterpretation = {metadata.PhotometricInterpretation} to RGB");
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }

            PinnedByteArray frameArray = new PinnedByteArray(frameData);
            Frameinfo frameinfo = GetFrameInfo(metadata);
            Htj2k_outdata j2k_outinfo = new Htj2k_outdata();

            try
            {
                if (Platform.IsWindows)
                {
                    NativeMethods.InvokeHTJ2KEncode_win(
                        ref j2k_outinfo,
                        (byte*)frameArray.Pointer,
                        (uint)frameArray.Count,
                        ref frameinfo,
                        j2kParams.ProgressionOrder);
                }
                else
                {
                    NativeMethods.InvokeHTJ2KEncode(
                        ref j2k_outinfo,
                        (byte*)frameArray.Pointer,
                        (uint)frameArray.Count,
                        ref frameinfo,
                        j2kParams.ProgressionOrder);
                }

                uint jpegHT2KDataSize = j2k_outinfo.size_outbuffer;
                uint evenSize = (jpegHT2KDataSize + 1) & ~1u;
                byte[] jpegHT2KData = new byte[evenSize];

                Marshal.Copy((IntPtr)j2k_outinfo.buffer, jpegHT2KData, 0, (int)jpegHT2KDataSize);

                return jpegHT2KData;
            }
            finally
            {
                frameArray?.Dispose();
            }
        }

        public override unsafe void Decode(byte[] input, byte[] output, FrameMetadata metadata, out FrameMetadata outputMetadata, CodecParams _ = null)
        {
            ArgumentNullException.ThrowIfNull(output);
            outputMetadata = (FrameMetadata)metadata.Clone();
            byte[] htjpeg2kData = ConvertToRgbIfNeeded(input, metadata);

            PinnedByteArray htjpeg2kArray = new PinnedByteArray(htjpeg2kData);

            try
            {
                Raw_outdata raw_Outdata = new Raw_outdata();

                if (Platform.IsWindows)
                {
                    NativeMethods.InvokeHTJ2KDecode_win(
                        ref raw_Outdata,
                        (byte*)htjpeg2kArray.Pointer,
                        (uint)htjpeg2kArray.Count);
                }
                else
                {
                    NativeMethods.InvokeHTJ2KDecode(
                        ref raw_Outdata,
                        (byte*)htjpeg2kArray.Pointer,
                        (uint)htjpeg2kArray.Count);
                }

                if (output.Length < raw_Outdata.size_outbuffer)
                {
                    throw new CodecException($"Output buffer is too small. Required size: {raw_Outdata.size_outbuffer}, provided size: {output.Length}");
                }

                Marshal.Copy((IntPtr)raw_Outdata.buffer, output, 0, (int)raw_Outdata.size_outbuffer);

            }
            catch (Exception e)
            {
                throw new CodecException(e.Message, e);
            }
            finally
            {
                htjpeg2kArray?.Dispose();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Frameinfo GetFrameInfo(FrameMetadata metadata) => new()
        {
            width = metadata.Width,
            height = metadata.Height,
            bitsPerSample = metadata.BitsAllocated,
            componentCount = metadata.SamplesPerPixel,
            isSigned = metadata.IsSigned,
            isUsingColorTransform = metadata.SamplesPerPixel > 1,
            isReversible = OutputTransferSyntax == TransferSyntax.HTJ2KLossless ||
                           OutputTransferSyntax == TransferSyntax.HTJ2KLosslessRPCL
        };
    }
}
