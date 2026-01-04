
namespace Dicom.Codecs.Jpeg2K
{
    using Dicom.Codecs.Common;

    internal class Jpeg2KLossyCodec : Jpeg2KCodecBase
    {
        public Jpeg2KLossyCodec(TransferSyntax outputTransferSyntax)
        {
            OutputTransferSyntax = outputTransferSyntax;
        }

        public override TransferSyntax TransferSyntax => TransferSyntax.JPEG2000Lossy;

        public override TransferSyntax OutputTransferSyntax { get; }
    }
}
