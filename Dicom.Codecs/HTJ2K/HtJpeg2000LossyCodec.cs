
namespace Dicom.Codecs.HTJ2K
{
    using Dicom.Codecs.Common;

    internal class HtJpeg2000LossyCodec : HtJpeg2000CodecBase
    {
        public HtJpeg2000LossyCodec(TransferSyntax outputTransferSyntax) : base()
        {
            OutputTransferSyntax = outputTransferSyntax;
        }

        public override TransferSyntax OutputTransferSyntax { get; }

        public override TransferSyntax TransferSyntax => TransferSyntax.HTJ2K;
    }
}
