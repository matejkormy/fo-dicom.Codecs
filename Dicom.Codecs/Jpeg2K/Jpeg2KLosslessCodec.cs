
namespace Dicom.Codecs.Jpeg2K
{
    using Dicom.Codecs.Common;

    internal class Jpeg2KLosslessCodec : Jpeg2KCodecBase
    {
        public Jpeg2KLosslessCodec(TransferSyntax outputTransferSyntax)
        {
            OutputTransferSyntax = outputTransferSyntax;
        }

        public override TransferSyntax TransferSyntax => TransferSyntax.JPEG2000Lossless;

        public override TransferSyntax OutputTransferSyntax { get; }
    }
}
