
namespace Dicom.Codecs.JpegLs
{
    using Dicom.Codecs.Common;

    internal class JpegLsLosslessCodec : JpegLsCodecBase
    {
        public JpegLsLosslessCodec(TransferSyntax outputSyntax)
        {
            OutputTransferSyntax = outputSyntax;
        }

        public override TransferSyntax TransferSyntax => TransferSyntax.JPEGLSLossless;

        public override TransferSyntax OutputTransferSyntax { get; }
    }
}
