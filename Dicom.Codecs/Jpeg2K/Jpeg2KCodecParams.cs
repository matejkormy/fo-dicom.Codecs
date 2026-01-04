

namespace Dicom.Codecs.Jpeg2K
{
    using Dicom.Codecs.Common;

    public class Jpeg2KCodecParams : CodecParams
    {
        private double[] _rates;

        public Jpeg2KCodecParams()
        {
            Irreversible = true;
            Rate = 20;
            IsVerbose = false;
            AllowMCT = true;
            UpdatePhotometricInterpretation = true;
            EncodeSignedPixelValuesAsUnsigned = false;

            _rates = new double[9];
            _rates[0] = 1280;
            _rates[1] = 640;
            _rates[2] = 320;
            _rates[3] = 160;
            _rates[4] = 80;
            _rates[5] = 40;
            _rates[6] = 20;
            _rates[7] = 10;
            _rates[8] = 5;

            RateLevels = _rates;
        }

        public bool Irreversible { get; set; }
        public double Rate { get; set; }
        public OPJ_PROG_ORDER ProgressionOrder { get; set; } = OPJ_PROG_ORDER.LRCP;
        public double[] RateLevels { get; set; }
        public bool IsVerbose { get; set; }
        public bool AllowMCT { get; set; }
        public bool UpdatePhotometricInterpretation { get; set; }
        public bool EncodeSignedPixelValuesAsUnsigned { get; set; }
    }
}
