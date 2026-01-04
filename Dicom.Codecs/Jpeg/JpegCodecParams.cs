
namespace Dicom.Codecs.Jpeg
{
    using Dicom.Codecs.Common;

    public class JpegCodecParams : CodecParams
    {
        public JpegCodecParams()
        {
            Quality = 90;
            SmoothingFactor = 0;
            ConvertColorSpaceToRGB = false;
            SampleFactor = DicomJpegSampleFactor.SF444;
            Predictor = 1;
            PointTransform = 0;
        }

        public int Quality { get; set; }
        public int SmoothingFactor { get; set; }
        public bool ConvertColorSpaceToRGB { get; set; }
        public DicomJpegSampleFactor SampleFactor { get; set; }
        public int Predictor { get; set; }
        public int PointTransform { get; set; }
    }
}
