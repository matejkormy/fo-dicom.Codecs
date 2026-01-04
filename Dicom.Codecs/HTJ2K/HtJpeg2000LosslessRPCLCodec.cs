
namespace Dicom.Codecs.HTJ2K
{
    using Dicom.Codecs.Common;

    internal class HtJpeg2000LosslessRPCLCodec : HtJpeg2000CodecBase
    {
        public HtJpeg2000LosslessRPCLCodec(TransferSyntax outputTransferSyntax) : base()
        {
            OutputTransferSyntax = outputTransferSyntax;
            DefaultParams = new HtJpeg2000CodecParams(OPJ_PROG_ORDER.RPCL);
        }

        public override TransferSyntax OutputTransferSyntax { get; }

        public override TransferSyntax TransferSyntax => TransferSyntax.HTJ2KLosslessRPCL;
    }
}
