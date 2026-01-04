
namespace Dicom.Codecs.JpegLs
{
    using System;
    using System.Buffers;
    using Dicom.Codecs.Common;

    internal abstract class JpegLsCodecBase : CodecBase
    {
        protected JpegLsCodecBase()
        {
            DefaultParams = new JpegLsCodecParams();
        }

        protected JpegLsCodecParams DefaultParams { get; }

        public override byte[] Encode(byte[] input, FrameMetadata metadata, out FrameMetadata outputMetadata, CodecParams codecParams = null)
        {
            // IMPORT JpegLsEncode
            unsafe
            {

                if (metadata.PhotometricInterpretation == PhotometricInterpretation.YbrPartial422 ||
                    metadata.PhotometricInterpretation == PhotometricInterpretation.YbrPartial420)
                {
                    throw new CodecException($"Photometric Interpretation {metadata.PhotometricInterpretation} not supported by JPEG-LS encoder");
                }

                outputMetadata = (FrameMetadata)metadata.Clone();
                JpegLsCodecParams jparams = codecParams as JpegLsCodecParams ?? DefaultParams;

                JlsParameters jls = new JlsParameters
                {
                    width = metadata.Width,
                    height = metadata.Height,
                    bitsPerSample = metadata.BitsStored,
                    stride = metadata.BytesAllocated * metadata.Width * metadata.SamplesPerPixel,
                    components = metadata.SamplesPerPixel,
                    interleaveMode = metadata.SamplesPerPixel == 1
                        ? CharlsInterleaveModeType.None
                        : metadata.PlanarConfiguration == PlanarConfiguration.Interleaved
                            ? CharlsInterleaveModeType.Sample
                            : CharlsInterleaveModeType.Line,
                    colorTransformation = CharlsColorTransformationType.None
                };

                if (TransferSyntax == TransferSyntax.JPEGLSNearLossless)
                {
                    jls.allowedLossyError = jparams.AllowedError;
                }

                var pool = ArrayPool<byte>.Shared;
                byte[] newJpegData = null;
                byte[] frameData = input;

                //Converting photometric interpretation YbrFull or YbrFull422 to RGB
                if (metadata.PlanarConfiguration == PlanarConfiguration.Planar && metadata.SamplesPerPixel > 1)
                {
                    if (metadata.SamplesPerPixel != 3 || metadata.BitsStored > 8)
                        throw new CodecException("Planar reconfiguration only implemented for SamplesPerPixel=3 && BitsStored <= 8");

                    frameData = PixelDataConverter.PlanarToInterleaved24(frameData);
                }
                else if (metadata.PhotometricInterpretation == PhotometricInterpretation.YbrFull)
                {
                    frameData = PixelDataConverter.YbrFullToRgb(frameData);
                }
                else if (metadata.PhotometricInterpretation == PhotometricInterpretation.YbrFull422)
                {
                    frameData = PixelDataConverter.YbrFull422ToRgb(frameData, metadata.Width);
                }

                PinnedByteArray frameArray = new PinnedByteArray(frameData);

                byte[] jpegData = pool.Rent(frameData.Length);

                try
                {
                    fixed (byte* jpegDataPointer = jpegData)
                    {
                        uint jpegDataSize = 0;
                        char[] errorMessage = new char[256];

                        CharlsApiResultType err = CharlsApiResultType.Unknown;

                        if (Platform.IsWindows)
                            err = NativeMethods.JpegLSEncode_win(
                                jpegDataPointer,
                                (uint)jpegData.Length,
                                &jpegDataSize,
                                (void*)frameArray.Pointer,
                                (uint)frameArray.Count,
                                ref jls, errorMessage);
                        else
                            err = NativeMethods.JpegLSEncode(
                                jpegDataPointer,
                                (uint)jpegData.Length,
                                &jpegDataSize,
                                (void*)frameArray.Pointer,
                                (uint)frameArray.Count,
                                ref jls,
                                errorMessage);

                        newJpegData = new byte[(int)jpegDataSize];
                        // If something is wrong later it may be caused by not padding buffer to even number
                        Array.Copy(jpegData, newJpegData, newJpegData.Length);
                    }

                    return newJpegData;
                }
                catch (Exception e)
                {
                    throw new CodecException(e.Message, e);
                }
                finally
                {
                    frameArray?.Dispose();

                    if (jpegData != null)
                    {
                        pool.Return(jpegData);
                        jpegData = null;
                    }
                }
            }
        }

        public override void Decode(byte[] input, byte[] output, FrameMetadata metadata, out FrameMetadata outputMetadata, CodecParams _ = null)
        {
            outputMetadata = (FrameMetadata)metadata.Clone();

            byte[] jpegData = input;

            try
            {
                jpegData = ConvertToRgbIfNeeded(jpegData, metadata);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cannot convert JPEG-LS buffer data from PhotometricInterpretation = {0} to RGB => {1} => {2}", metadata
                .PhotometricInterpretation, ex.Message, ex.StackTrace);
            }

            byte[] frameData = output;
            PinnedByteArray jpegArray = new PinnedByteArray(jpegData);
            PinnedByteArray frameArray = new PinnedByteArray(frameData);

            try
            {
                JlsParameters jls = new JlsParameters();

                char[] errorMessage = new char[256];

                CharlsApiResultType err = CharlsApiResultType.Unknown;

                unsafe
                {
                    if (Platform.IsWindows)
                        err = NativeMethods.JpegLSDecode_win((void*)frameArray.Pointer, frameData.Length, (void*)jpegArray.Pointer, Convert.ToUInt32(jpegData.Length), ref jls, errorMessage);
                    else
                        err = NativeMethods.JpegLSDecode((void*)frameArray.Pointer, frameData.Length, (void*)jpegArray.Pointer, Convert.ToUInt32(jpegData.Length), ref jls, errorMessage);
                }
            }
            catch (Exception ex)
            {
                throw new CodecException(ex.Message, ex);
            }
            finally
            {
                frameArray?.Dispose();
                jpegArray?.Dispose();
            }
        }
    }
}
