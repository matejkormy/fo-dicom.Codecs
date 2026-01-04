
namespace Dicom.Codecs.JpegLs
{
    using Dicom.Codecs.Common;

    internal class JpegLsNearLosslessCodec : JpegLsCodecBase
    {
        public JpegLsNearLosslessCodec(TransferSyntax outputSyntax)
        {
            OutputTransferSyntax = outputSyntax;
        }

        public override TransferSyntax TransferSyntax => TransferSyntax.JPEGLSNearLossless;

        public override TransferSyntax OutputTransferSyntax { get; }
    }
}
