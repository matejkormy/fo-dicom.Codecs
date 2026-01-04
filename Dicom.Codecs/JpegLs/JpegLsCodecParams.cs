
namespace Dicom.Codecs.JpegLs
{
    using Dicom.Codecs.Common;

    internal class JpegLsCodecParams : CodecParams
    {
        public JpegLsCodecParams()
        {
            AllowedError = 3;
            InterleaveMode = DicomJpegLsInterleaveMode.Line;
            ColorTransform = DicomJpegLsColorTransform.HP1;
        }

        public int AllowedError { get; set; }

        public DicomJpegLsInterleaveMode InterleaveMode { get; set; }

        public DicomJpegLsColorTransform ColorTransform { get; set; }
    }
}
