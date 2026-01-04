namespace Dicom.Codecs.Jpeg
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    internal class NativeWrapper
    {
        private readonly static IntPtr DicomNativeLibrary = IntPtr.Zero;
        private readonly static Dictionary<int, Lazy<NativeWrapper>> InstancesByBitDepth = new();

        // Encode delegates
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public unsafe delegate jpeg_error_mgr* JpegStdErrorDelegate(ref jpeg_error_mgr err);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public unsafe delegate void JpegCreateCompressDelegate(ref j_compress_ptr cinfo);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public unsafe delegate void JpegSetDefaultsDelegate(ref j_compress_ptr cinfo);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public unsafe delegate void JpegSetQualityDelegate(ref j_compress_ptr cinfo, int quality, int force_baseline);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public unsafe delegate void JpegSimpleProgressionDelegate(ref j_compress_ptr cinfo);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public unsafe delegate void JpegSimpleLosslessDelegate(ref j_compress_ptr cinfo, int predictor, int point_transform);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public unsafe delegate void JpegSetColorspaceDelegate(ref j_compress_ptr cinfo, J_COLOR_SPACE in_color_space);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public unsafe delegate void JpegStartCompressDelegate(ref j_compress_ptr cinfo, int write_all_tables);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public unsafe delegate void JpegWriteScanlinesDelegate(ref j_compress_ptr cinfo, byte** scanlines, uint num_lines);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public unsafe delegate void JpegFinishCompressDelegate(ref j_compress_ptr cinfo);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public unsafe delegate void JpegDestroyCompressDelegate(ref j_compress_ptr cinfo);

        // Decode delgates
        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public unsafe delegate void JpegCreateDecompressDelegate(ref j_decompress_ptr dinfo);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public unsafe delegate int JpegReadHeaderDelegate(ref j_decompress_ptr dinfo, int require_image);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public unsafe delegate void JpegCalcOutputDimensionsDelegate(ref j_decompress_ptr dinfo);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public unsafe delegate int JpegStartDecompressDelegate(ref j_decompress_ptr dinfo);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public unsafe delegate uint JpegReadScanlinesDelegate(ref j_decompress_ptr dinfo, byte** scanlines, uint max_lines);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public unsafe delegate void JpegDestroyDecompressDelegate(ref j_decompress_ptr dinfo);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public unsafe delegate int JpegResyncToRestartDelegate(ref j_decompress_ptr dinfo, int desired);

        [UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public unsafe delegate void FormatMessageDelegate(j_common_ptr* cinfo, char[] buffer);

        // Encode
        private readonly JpegStdErrorDelegate _jpegStdError;
        private readonly JpegCreateCompressDelegate _jpegCreateCompress;
        private readonly JpegSetDefaultsDelegate _jpegSetDefaults;
        private readonly JpegSetQualityDelegate _jpegSetQuality;
        private readonly JpegSimpleProgressionDelegate _jpegSimpleProgression;
        private readonly JpegSimpleLosslessDelegate _jpegSimpleLossless;
        private readonly JpegSetColorspaceDelegate _jpegSetColorspace;
        private readonly JpegStartCompressDelegate _jpegStartCompress;
        private readonly JpegWriteScanlinesDelegate _jpegWriteScanlines;
        private readonly JpegFinishCompressDelegate _jpegFinishCompress;
        private readonly JpegDestroyCompressDelegate _jpegDestroyCompress;

        // Decode
        private readonly JpegCreateDecompressDelegate _jpegCreateDecompress;
        private readonly JpegReadHeaderDelegate _jpegReadHeader;
        private readonly JpegCalcOutputDimensionsDelegate _jpegCalcOutputDimensions;
        private readonly JpegStartDecompressDelegate _jpegStartDecompress;
        private readonly JpegReadScanlinesDelegate _jpegReadScanlines;
        private readonly JpegDestroyDecompressDelegate _jpegDestroyDecompress;
        private readonly JpegResyncToRestartDelegate _jpegResyncToRestart;
        private readonly FormatMessageDelegate _formatMessage;

        static NativeWrapper()
        {
            DicomNativeLibrary = NativeLibrary.Load("Dicom.Native");
        }

        private NativeWrapper(int bitDepth)
        {
            string suffix = bitDepth.ToString();

            // Encode delegates
            _jpegStdError = Marshal.GetDelegateForFunctionPointer<JpegStdErrorDelegate>(
                NativeLibrary.GetExport(DicomNativeLibrary, $"jpeg_std_error_{suffix}"));

            _jpegCreateCompress = Marshal.GetDelegateForFunctionPointer<JpegCreateCompressDelegate>(
                NativeLibrary.GetExport(DicomNativeLibrary, $"jpeg_create_compress_{suffix}"));

            _jpegSetDefaults = Marshal.GetDelegateForFunctionPointer<JpegSetDefaultsDelegate>(
                NativeLibrary.GetExport(DicomNativeLibrary, $"jpeg_set_defaults_{suffix}"));

            _jpegSetQuality = Marshal.GetDelegateForFunctionPointer<JpegSetQualityDelegate>(
                NativeLibrary.GetExport(DicomNativeLibrary, $"jpeg_set_quality_{suffix}"));

            _jpegSimpleProgression = Marshal.GetDelegateForFunctionPointer<JpegSimpleProgressionDelegate>(
                NativeLibrary.GetExport(DicomNativeLibrary, $"jpeg_simple_progression_{suffix}"));

            _jpegSimpleLossless = Marshal.GetDelegateForFunctionPointer<JpegSimpleLosslessDelegate>(
                NativeLibrary.GetExport(DicomNativeLibrary, $"jpeg_simple_lossless_{suffix}"));

            _jpegSetColorspace = Marshal.GetDelegateForFunctionPointer<JpegSetColorspaceDelegate>(
                NativeLibrary.GetExport(DicomNativeLibrary, $"jpeg_set_colorspace_{suffix}"));

            _jpegStartCompress = Marshal.GetDelegateForFunctionPointer<JpegStartCompressDelegate>(
                NativeLibrary.GetExport(DicomNativeLibrary, $"jpeg_start_compress_{suffix}"));

            _jpegWriteScanlines = Marshal.GetDelegateForFunctionPointer<JpegWriteScanlinesDelegate>(
                NativeLibrary.GetExport(DicomNativeLibrary, $"jpeg_write_scanlines_{suffix}"));

            _jpegFinishCompress = Marshal.GetDelegateForFunctionPointer<JpegFinishCompressDelegate>(
                NativeLibrary.GetExport(DicomNativeLibrary, $"jpeg_finish_compress_{suffix}"));

            _jpegDestroyCompress = Marshal.GetDelegateForFunctionPointer<JpegDestroyCompressDelegate>(
                NativeLibrary.GetExport(DicomNativeLibrary, $"jpeg_destroy_compress_{suffix}"));

            // Decode delegates
            _jpegCreateDecompress = Marshal.GetDelegateForFunctionPointer<JpegCreateDecompressDelegate>(
                NativeLibrary.GetExport(DicomNativeLibrary, $"jpeg_create_decompress_{suffix}"));

            _jpegReadHeader = Marshal.GetDelegateForFunctionPointer<JpegReadHeaderDelegate>(
                NativeLibrary.GetExport(DicomNativeLibrary, $"jpeg_read_header_{suffix}"));

            _jpegCalcOutputDimensions = Marshal.GetDelegateForFunctionPointer<JpegCalcOutputDimensionsDelegate>(
                NativeLibrary.GetExport(DicomNativeLibrary, $"jpeg_calc_output_dimensions_{suffix}"));

            _jpegStartDecompress = Marshal.GetDelegateForFunctionPointer<JpegStartDecompressDelegate>(
                NativeLibrary.GetExport(DicomNativeLibrary, $"jpeg_start_decompress_{suffix}"));

            _jpegReadScanlines = Marshal.GetDelegateForFunctionPointer<JpegReadScanlinesDelegate>(
                NativeLibrary.GetExport(DicomNativeLibrary, $"jpeg_read_scanlines_{suffix}"));

            _jpegDestroyDecompress = Marshal.GetDelegateForFunctionPointer<JpegDestroyDecompressDelegate>(
                NativeLibrary.GetExport(DicomNativeLibrary, $"jpeg_destroy_decompress_{suffix}"));

            _jpegResyncToRestart = Marshal.GetDelegateForFunctionPointer<JpegResyncToRestartDelegate>(
                NativeLibrary.GetExport(DicomNativeLibrary, $"jpeg_resync_to_restart_{suffix}"));

            _formatMessage = Marshal.GetDelegateForFunctionPointer<FormatMessageDelegate>(
                NativeLibrary.GetExport(DicomNativeLibrary, "format_message_8")); // 8 is on purpose here
        }

        public static NativeWrapper GetInstance(int bitDepth)
        {
            int normalizedBitDepth = NormalizeBitDepth(bitDepth);

            if (!InstancesByBitDepth.TryGetValue(normalizedBitDepth, out Lazy<NativeWrapper> instance))
            {
                instance = new Lazy<NativeWrapper>(() => new NativeWrapper(normalizedBitDepth));
                InstancesByBitDepth[normalizedBitDepth] = instance;
            }

            return instance.Value;
        }

        // Encode methods
        public unsafe jpeg_error_mgr* JpegStdError(ref jpeg_error_mgr err)
        {
            return _jpegStdError(ref err);
        }

        public unsafe void JpegCreateCompress(ref j_compress_ptr cinfo)
        {
            _jpegCreateCompress(ref cinfo);
        }

        public unsafe void JpegSetDefaults(ref j_compress_ptr cinfo)
        {
            _jpegSetDefaults(ref cinfo);
        }

        public unsafe void JpegSetQuality(ref j_compress_ptr cinfo, int quality, int force_baseline)
        {
            _jpegSetQuality(ref cinfo, quality, force_baseline);
        }

        public unsafe void JpegSimpleProgression(ref j_compress_ptr cinfo)
        {
            _jpegSimpleProgression(ref cinfo);
        }

        public unsafe void JpegSimpleLossless(ref j_compress_ptr cinfo, int predictor, int point_transform)
        {
            _jpegSimpleLossless(ref cinfo, predictor, point_transform);
        }

        public unsafe void JpegSetColorspace(ref j_compress_ptr cinfo, J_COLOR_SPACE in_color_space)
        {
            _jpegSetColorspace(ref cinfo, in_color_space);
        }

        public unsafe void JpegStartCompress(ref j_compress_ptr cinfo, int write_all_tables)
        {
            _jpegStartCompress(ref cinfo, write_all_tables);
        }

        public unsafe void JpegWriteScanlines(ref j_compress_ptr cinfo, byte** scanlines, uint num_lines)
        {
            _jpegWriteScanlines(ref cinfo, scanlines, num_lines);
        }

        public unsafe void JpegFinishCompress(ref j_compress_ptr cinfo)
        {
            _jpegFinishCompress(ref cinfo);
        }

        public unsafe void JpegDestroyCompress(ref j_compress_ptr cinfo)
        {
            _jpegDestroyCompress(ref cinfo);
        }

        // Decode methods
        public unsafe void JpegCreateDecompress(ref j_decompress_ptr dinfo)
        {
            _jpegCreateDecompress(ref dinfo);
        }

        public unsafe int JpegReadHeader(ref j_decompress_ptr dinfo, int require_image)
        {
            return _jpegReadHeader(ref dinfo, require_image);
        }

        public unsafe void JpegCalcOutputDimensions(ref j_decompress_ptr dinfo)
        {
            _jpegCalcOutputDimensions(ref dinfo);
        }

        public unsafe int JpegStartDecompress(ref j_decompress_ptr dinfo)
        {
            return _jpegStartDecompress(ref dinfo);
        }

        public unsafe uint JpegReadScanlines(ref j_decompress_ptr dinfo, byte** scanlines, uint max_lines)
        {
            return _jpegReadScanlines(ref dinfo, scanlines, max_lines);
        }

        public unsafe void JpegDestroyDecompress(ref j_decompress_ptr dinfo)
        {
            _jpegDestroyDecompress(ref dinfo);
        }

        public unsafe int JpegResyncToRestart(ref j_decompress_ptr dinfo, int desired)
        {
            return _jpegResyncToRestart(ref dinfo, desired);
        }

        public unsafe void FormatMessage(j_common_ptr* cinfo, char[] buffer)
        {
            _formatMessage(cinfo, buffer);
        }

        private static int NormalizeBitDepth(int bitDepth)
        {
            if (bitDepth <= 12 && bitDepth > 8)
            {
                return 12;
            }
            else if (bitDepth <= 16 && bitDepth > 12)
            {
                return 16;
            }

            return 8;
        }
    }
}
