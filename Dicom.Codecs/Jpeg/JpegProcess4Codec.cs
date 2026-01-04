

namespace Dicom.Codecs.Jpeg
{
    using Dicom.Codecs.Common;

    internal class JpegProcess4Codec : CodecBase
    {
        public JpegProcess4Codec(TransferSyntax outputTransferSyntax)
        {
            OutputTransferSyntax = outputTransferSyntax;
        }

        public override TransferSyntax TransferSyntax => TransferSyntax.JPEGProcess2_4;

        public override TransferSyntax OutputTransferSyntax { get; }

        public override void Decode(byte[] input, byte[] output, FrameMetadata metadata, out FrameMetadata outputMetadata, CodecParams codecParams = null)
        {
            var jparams = codecParams as JpegCodecParams ?? new JpegCodecParams();
            var codec = new JpegCodec(TransferSyntax, OutputTransferSyntax, JpegMode.Sequential, 0, 0, metadata.BitsStored);
            codec.Decode(input, output, metadata, out outputMetadata, jparams);
        }

        public override byte[] Encode(byte[] input, FrameMetadata metadata, out FrameMetadata outputMetadata, CodecParams codecParams = null)
        {
            var jparams = codecParams as JpegCodecParams ?? new JpegCodecParams();
            var codec = new JpegCodec(TransferSyntax, OutputTransferSyntax, JpegMode.Sequential, 0, 0, metadata.BitsStored);
            return codec.Encode(input, metadata, out outputMetadata, jparams);
        }
    }
}
