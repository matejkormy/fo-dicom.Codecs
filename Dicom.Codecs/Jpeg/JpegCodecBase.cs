
namespace Dicom.Codecs.Jpeg
{
    using System.IO;
    using System.Runtime.InteropServices;
    using Dicom.Codecs.Common;

    internal abstract class JpegCodecBase : CodecBase
    {
        protected MemoryStream MemoryBuffer;
        protected PinnedByteArray DataArray;
        protected JpegMode Mode;
        protected int Predictor;
        protected int PointTransform;
        protected int Bits;

        public JpegCodecBase(JpegMode mode, int predictor, int point_transform, int bits)
        {
            Mode = mode;
            Predictor = predictor;
            PointTransform = point_transform;
            Bits = bits;
            DefaultParams = new JpegCodecParams();
        }

        protected abstract int ScanHeaderForPrecision(byte[] frame, FrameMetadata metadata, bool isjPEG);

        [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
        protected unsafe delegate void init_destination(j_compress_ptr* cinfo);

        [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
        protected unsafe delegate int empty_output_buffer(j_compress_ptr* cinfo);

        [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
        protected unsafe delegate void term_destination(j_compress_ptr* cinfo);

        [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
        protected unsafe delegate void Init_source(j_decompress_ptr* dinfo);

        [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
        protected unsafe delegate int Fill_input_buffer(j_decompress_ptr* dinfo);

        [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
        protected unsafe delegate void Skip_input_data(j_decompress_ptr* dinfo, int num_bytes);

        [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
        protected unsafe delegate int Resync_to_restart(ref j_decompress_ptr dinfo, int desired);

        protected JpegCodecParams DefaultParams { get; set; }

        protected static J_COLOR_SPACE GetJpegColorSpace(PhotometricInterpretation photometricInterpretation) => photometricInterpretation switch
        {
            PhotometricInterpretation.Rgb => J_COLOR_SPACE.JCS_RGB,
            PhotometricInterpretation.Monochrome1 => J_COLOR_SPACE.JCS_GRAYSCALE,
            PhotometricInterpretation.Monochrome2 => J_COLOR_SPACE.JCS_GRAYSCALE,
            PhotometricInterpretation.PaletteColor => J_COLOR_SPACE.JCS_UNKNOWN,
            PhotometricInterpretation.YbrFull => J_COLOR_SPACE.JCS_YCbCr,
            PhotometricInterpretation.YbrFull422 => J_COLOR_SPACE.JCS_YCbCr,
            PhotometricInterpretation.YbrPartial422 => J_COLOR_SPACE.JCS_YCbCr,
            _ => J_COLOR_SPACE.JCS_UNKNOWN,
        };
    }
}
