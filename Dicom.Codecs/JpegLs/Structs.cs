
namespace Dicom.Codecs.JpegLs
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct JpegLSPresetCodingParameters
    {
        public int MaximumSampleValue;
        public int Threshold1;
        public int Threshold2;
        public int Threshold3;
        public int ResetValue;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct JfifParameters
    {
        public int version;
        public int units;
        public int Xdensity;
        public int Ydensity;
        public int Xthumbnail;
        public int Ythumbnail;
        public void* thumbnail;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct JlsParameters
    {
        public int width;
        public int height;
        public int bitsPerSample;
        public int stride;
        public int components;
        public int allowedLossyError;
        public CharlsInterleaveModeType interleaveMode;
        public CharlsColorTransformationType colorTransformation;
        public JpegLSPresetCodingParameters custom;
        public JfifParameters jfif;
        public sbyte outputBgr;
    }
}
