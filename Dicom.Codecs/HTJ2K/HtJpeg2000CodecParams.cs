
namespace Dicom.Codecs.HTJ2K
{
    using Dicom.Codecs.Common;

    public class HtJpeg2000CodecParams : CodecParams
    {
        public HtJpeg2000CodecParams(OPJ_PROG_ORDER progressionOrder)
        {
            ProgressionOrder = progressionOrder;
        }

        public OPJ_PROG_ORDER ProgressionOrder { get; set; }
    }
}
