
namespace Dicom.Codecs.Rle
{
    using System;
    using Dicom.Codecs.Common;

    internal class RleCodec : CodecBase
    {
        public RleCodec(TransferSyntax outputSyntax)
        {
            OutputTransferSyntax = outputSyntax;
        }

        public override TransferSyntax TransferSyntax => TransferSyntax.RLELossless;

        public override TransferSyntax OutputTransferSyntax { get; }

        public override byte[] Encode(byte[] input, FrameMetadata metadata, out FrameMetadata outputMetadata, CodecParams _ = null)
        {
            outputMetadata = (FrameMetadata)metadata.Clone();

            var pixelCount = metadata.Width * metadata.Height;
            var numberOfSegments = metadata.BytesAllocated * metadata.SamplesPerPixel;

            var frameArray = input;

            using (var encoder = new RleEncoder())
            {

                for (var s = 0; s < numberOfSegments; s++)
                {
                    encoder.NextSegment();

                    var sample = s / metadata.BytesAllocated;
                    var sabyte = s % metadata.BytesAllocated;

                    int pos;
                    int offset;

                    if (outputMetadata.PlanarConfiguration == PlanarConfiguration.Interleaved)
                    {
                        pos = sample * metadata.BytesAllocated;
                        offset = numberOfSegments;
                    }
                    else
                    {
                        pos = sample * metadata.BytesAllocated * pixelCount;
                        offset = metadata.BytesAllocated;
                    }

                    pos += metadata.BytesAllocated - sabyte - 1;

                    for (var p = 0; p < pixelCount; p++)
                    {
                        if (pos >= frameArray.Length)
                        {
                            throw new InvalidOperationException("Read position is past end of frame buffer");
                        }
                        encoder.Encode(frameArray[pos]);
                        pos += offset;
                    }
                    encoder.Flush();
                }

                encoder.MakeEvenLength();

                var data = encoder.GetBuffer();
                return data;
            }
        }

        public override void Decode(byte[] input, byte[] output, FrameMetadata metadata, out FrameMetadata outputMetadata, CodecParams codecParams = null)
        {
            try
            {
                outputMetadata = (FrameMetadata)metadata.Clone();
                var rleData = input;

                // Create new frame data of even length
                var frameSize = outputMetadata.UncompressedSize;

                if ((frameSize & 1) == 1)
                {
                    ++frameSize;
                }

                if (output.Length < frameSize)
                {
                    throw new ArgumentException("Output buffer is smaller than expected frame size");
                }

                var frameData = output;

                var pixelCount = metadata.Width * metadata.Height;
                var numberOfSegments = metadata.BytesAllocated * metadata.SamplesPerPixel;

                var decoder = new RleDecoder(rleData);

                if (decoder.NumberOfSegments != numberOfSegments)
                {
                    throw new InvalidOperationException("Unexpected number of RLE segments!");
                }

                for (var s = 0; s < numberOfSegments; s++)
                {
                    var sample = s / outputMetadata.BytesAllocated;
                    var sabyte = s % outputMetadata.BytesAllocated;

                    int pos, offset;

                    if (outputMetadata.PlanarConfiguration == PlanarConfiguration.Interleaved)
                    {
                        pos = sample * outputMetadata.BytesAllocated;
                        offset = outputMetadata.SamplesPerPixel * outputMetadata.BytesAllocated;
                    }
                    else
                    {
                        pos = sample * outputMetadata.BytesAllocated * pixelCount;
                        offset = outputMetadata.BytesAllocated;
                    }

                    pos += outputMetadata.BytesAllocated - sabyte - 1;
                    decoder.DecodeSegment(s, frameData, pos, offset);
                }
            }
            catch (Exception ex)
            {
                throw new CodecException(ex.Message, ex);
            }
        }
    }
}
