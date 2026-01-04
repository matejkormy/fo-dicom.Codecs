
namespace Dicom.Codecs.Jpeg
{
    using System.Runtime.InteropServices;

    internal static class NativeMethods
    {
        // DLLIMPORT libijg8 library for win_x64 or win_arm64
        // Encode Native functions

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_std_error_8")]
        public static extern unsafe jpeg_error_mgr* jpeg_std_error_8_win(ref jpeg_error_mgr err);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_create_compress_8")]
        public static extern unsafe void jpeg_create_compress_8_win(ref j_compress_ptr cinfo);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_set_defaults_8")]
        public static extern unsafe void jpeg_set_defaults_8_win(ref j_compress_ptr cinfo);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_set_quality_8")]
        public static extern unsafe void jpeg_set_quality_8_win(ref j_compress_ptr cinfo, int quality, int force_baseline);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_simple_progression_8")]
        public static extern unsafe void jpeg_simple_progression_8_win(ref j_compress_ptr cinfo);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_simple_lossless_8")]
        public static extern unsafe void jpeg_simple_lossless_8_win(ref j_compress_ptr cinfo, int predictor, int point_transform);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_set_colorspace_8")]
        public static extern unsafe void jpeg_set_colorspace_8_win(ref j_compress_ptr cinfo, J_COLOR_SPACE in_color_space);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_start_compress_8")]
        public static extern unsafe void jpeg_start_compress_8_win(ref j_compress_ptr cinfo, int write_all_tables);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_write_scanlines_8")]
        public static extern unsafe void jpeg_write_scanlines_8_win(ref j_compress_ptr cinfo, byte** scanlines, uint num_lines);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_finish_compress_8")]
        public static extern unsafe void jpeg_finish_compress_8_win(ref j_compress_ptr cinfo);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_destroy_compress_8")]
        public static extern unsafe void jpeg_destroy_compress_8_win(ref j_compress_ptr cinfo);

        // Decode Native functions
        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_create_decompress_8")]
        public static extern unsafe void jpeg_create_decompress_8_win(ref j_decompress_ptr dinfo);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_read_header_8")]
        public static extern unsafe int jpeg_read_header_8_win(ref j_decompress_ptr dinfo, int require_image);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_calc_output_dimensions_8")]
        public static extern unsafe void jpeg_calc_output_dimensions_8_win(ref j_decompress_ptr dinfo);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_start_decompress_8")]
        public static extern unsafe int jpeg_start_decompress_8_win(ref j_decompress_ptr dinfo);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_read_scanlines_8")]
        public static extern unsafe uint jpeg_read_scanlines_8_win(ref j_decompress_ptr dinfo, byte** scanlines, uint max_lines);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_destroy_decompress_8")]
        public static extern unsafe void jpeg_destroy_decompress_8_win(ref j_decompress_ptr dinfo);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_resync_to_restart_8")]
        public static extern unsafe int jpeg_resync_to_restart_8_win(ref j_decompress_ptr dinfo, int desired);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "format_message_8")]
        public static extern unsafe void format_message_win(j_common_ptr* cinfo, char[] buffer);


        // DLLIMPORT libijg12 library for win_x64 or win_arm64

        // Encode Native functions
        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, EntryPoint = "jpeg_std_error_12")]
        public static extern unsafe jpeg_error_mgr* jpeg_std_error_12_win(ref jpeg_error_mgr err);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, EntryPoint = "jpeg_create_compress_12")]
        public static extern unsafe void jpeg_create_compress_12_win(ref j_compress_ptr cinfo);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, EntryPoint = "jpeg_set_defaults_12")]
        public static extern unsafe void jpeg_set_defaults_12_win(ref j_compress_ptr cinfo);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, EntryPoint = "jpeg_set_quality_12")]
        public static extern unsafe void jpeg_set_quality_12_win(ref j_compress_ptr cinfo, int quality, int force_baseline);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, EntryPoint = "jpeg_simple_progression_12")]
        public static extern unsafe void jpeg_simple_progression_12_win(ref j_compress_ptr cinfo);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, EntryPoint = "jpeg_simple_lossless_12")]
        public static extern unsafe void jpeg_simple_lossless_12_win(ref j_compress_ptr cinfo, int predictor, int point_transform);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, EntryPoint = "jpeg_set_colorspace_12")]
        public static extern unsafe void jpeg_set_colorspace_12_win(ref j_compress_ptr cinfo, J_COLOR_SPACE in_color_space);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, EntryPoint = "jpeg_start_compress_12")]
        public static extern unsafe void jpeg_start_compress_12_win(ref j_compress_ptr cinfo, int write_all_tables);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, EntryPoint = "jpeg_write_scanlines_12")]
        public static extern unsafe uint jpeg_write_scanlines_12_win(ref j_compress_ptr cinfo, short** scanlines, uint num_lines);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, EntryPoint = "jpeg_finish_compress_12")]
        public static extern unsafe void jpeg_finish_compress_12_win(ref j_compress_ptr cinfo);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, EntryPoint = "jpeg_destroy_compress_12")]
        public static extern unsafe void jpeg_destroy_compress_12_win(ref j_compress_ptr cinfo);

        // Decode Native functions
        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_create_decompress_12")]
        public static extern unsafe void jpeg_create_decompress_12_win(ref j_decompress_ptr dinfo);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_read_header_12")]
        public static extern unsafe int jpeg_read_header_12_win(ref j_decompress_ptr dinfo, int require_image);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_calc_output_dimensions_12")]
        public static extern unsafe void jpeg_calc_output_dimensions_12_win(ref j_decompress_ptr dinfo);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_start_decompress_12")]
        public static extern unsafe int jpeg_start_decompress_12_win(ref j_decompress_ptr dinfo);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_read_scanlines_12")]
        public static extern unsafe uint jpeg_read_scanlines_12_win(ref j_decompress_ptr dinfo, short** scanlines, uint max_lines);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_destroy_decompress_12")]
        public static extern unsafe void jpeg_destroy_decompress_12_win(ref j_decompress_ptr dinfo);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_resync_to_restart_12")]
        public static extern unsafe int jpeg_resync_to_restart_12_win(ref j_decompress_ptr dinfo, int desired);

        // DLLIMPORT libijg16 library for win_x64 win_arm64

        // Encode Native functions
        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_std_error_16")]
        public static extern unsafe jpeg_error_mgr* jpeg_std_error_16_win(ref jpeg_error_mgr err);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_create_compress_16")]
        public static extern unsafe void jpeg_create_compress_16_win(ref j_compress_ptr cinfo);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_set_defaults_16")]
        public static extern unsafe void jpeg_set_defaults_16_win(ref j_compress_ptr cinfo);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_set_quality_16")]
        public static extern unsafe void jpeg_set_quality_16_win(ref j_compress_ptr cinfo, int quality, int force_baseline);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_simple_progression_16")]
        public static extern unsafe void jpeg_simple_progression_16_win(ref j_compress_ptr cinfo);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_simple_lossless_16")]
        public static extern unsafe void jpeg_simple_lossless_16_win(ref j_compress_ptr cinfo, int predictor, int point_transform);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_set_colorspace_16")]
        public static extern unsafe void jpeg_set_colorspace_16_win(ref j_compress_ptr cinfo, J_COLOR_SPACE in_color_space);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_start_compress_16")]
        public static extern unsafe void jpeg_start_compress_16_win(ref j_compress_ptr cinfo, int write_all_tables);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_write_scanlines_16")]
        public static extern unsafe void jpeg_write_scanlines_16_win(ref j_compress_ptr cinfo, ushort** scanlines, uint num_lines);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_finish_compress_16")]
        public static extern unsafe void jpeg_finish_compress_16_win(ref j_compress_ptr cinfo);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_destroy_compress_16")]
        public static extern unsafe void jpeg_destroy_compress_16_win(ref j_compress_ptr cinfo);

        // Decode native functions
        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_create_decompress_16")]
        public static extern unsafe void jpeg_create_decompress_16_win(ref j_decompress_ptr dinfo);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_read_header_16")]
        public static extern unsafe int jpeg_read_header_16_win(ref j_decompress_ptr dinfo, int require_image);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_calc_output_dimensions_16")]
        public static extern unsafe void jpeg_calc_output_dimensions_16_win(ref j_decompress_ptr dinfo);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_start_decompress_16")]
        public static extern unsafe int jpeg_start_decompress_16_win(ref j_decompress_ptr dinfo);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_read_scanlines_16")]
        public static extern unsafe uint jpeg_read_scanlines_16_win(ref j_decompress_ptr dinfo, ushort** scanlines, uint max_lines);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_destroy_decompress_16")]
        public static extern unsafe void jpeg_destroy_decompress_16_win(ref j_decompress_ptr dinfo);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_resync_to_restart_16")]
        public static extern unsafe int jpeg_resync_to_restart_16_win(ref j_decompress_ptr dinfo, int desired);

        // DLLIMPORT libijg8 library
        // Encode Native functions

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_std_error_8")]
        public static extern unsafe jpeg_error_mgr* jpeg_std_error_8(ref jpeg_error_mgr err);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_create_compress_8")]
        public static extern unsafe void jpeg_create_compress_8(ref j_compress_ptr cinfo);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_set_defaults_8")]
        public static extern unsafe void jpeg_set_defaults_8(ref j_compress_ptr cinfo);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_set_quality_8")]
        public static extern unsafe void jpeg_set_quality_8(ref j_compress_ptr cinfo, int quality, int force_baseline);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_simple_progression_8")]
        public static extern unsafe void jpeg_simple_progression_8(ref j_compress_ptr cinfo);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_simple_lossless_8")]
        public static extern unsafe void jpeg_simple_lossless_8(ref j_compress_ptr cinfo, int predictor, int point_transform);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_set_colorspace_8")]
        public static extern unsafe void jpeg_set_colorspace_8(ref j_compress_ptr cinfo, J_COLOR_SPACE in_color_space);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_start_compress_8")]
        public static extern unsafe void jpeg_start_compress_8(ref j_compress_ptr cinfo, int write_all_tables);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_write_scanlines_8")]
        public static extern unsafe void jpeg_write_scanlines_8(ref j_compress_ptr cinfo, byte** scanlines, uint num_lines);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_finish_compress_8")]
        public static extern unsafe void jpeg_finish_compress_8(ref j_compress_ptr cinfo);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_destroy_compress_8")]
        public static extern unsafe void jpeg_destroy_compress_8(ref j_compress_ptr cinfo);

        // Decode Native functions
        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_create_decompress_8")]
        public static extern unsafe void jpeg_create_decompress_8(ref j_decompress_ptr dinfo);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_read_header_8")]
        public static extern unsafe int jpeg_read_header_8(ref j_decompress_ptr dinfo, int require_image);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_calc_output_dimensions_8")]
        public static extern unsafe void jpeg_calc_output_dimensions_8(ref j_decompress_ptr dinfo);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_start_decompress_8")]
        public static extern unsafe int jpeg_start_decompress_8(ref j_decompress_ptr dinfo);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_read_scanlines_8")]
        public static extern unsafe uint jpeg_read_scanlines_8(ref j_decompress_ptr dinfo, byte** scanlines, uint max_lines);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_destroy_decompress_8")]
        public static extern unsafe void jpeg_destroy_decompress_8(ref j_decompress_ptr dinfo);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_resync_to_restart_8")]
        public static extern unsafe int jpeg_resync_to_restart_8(ref j_decompress_ptr dinfo, int desired);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "format_message_8")]
        public static extern unsafe void format_message(j_common_ptr* cinfo, char[] buffer);


        // DLLIMPORT libijg12 library

        // Encode Native functions
        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, EntryPoint = "jpeg_std_error_12")]
        public static extern unsafe jpeg_error_mgr* jpeg_std_error_12(ref jpeg_error_mgr err);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, EntryPoint = "jpeg_create_compress_12")]
        public static extern unsafe void jpeg_create_compress_12(ref j_compress_ptr cinfo);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, EntryPoint = "jpeg_set_defaults_12")]
        public static extern unsafe void jpeg_set_defaults_12(ref j_compress_ptr cinfo);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, EntryPoint = "jpeg_set_quality_12")]
        public static extern unsafe void jpeg_set_quality_12(ref j_compress_ptr cinfo, int quality, int force_baseline);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, EntryPoint = "jpeg_simple_progression_12")]
        public static extern unsafe void jpeg_simple_progression_12(ref j_compress_ptr cinfo);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, EntryPoint = "jpeg_simple_lossless_12")]
        public static extern unsafe void jpeg_simple_lossless_12(ref j_compress_ptr cinfo, int predictor, int point_transform);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, EntryPoint = "jpeg_set_colorspace_12")]
        public static extern unsafe void jpeg_set_colorspace_12(ref j_compress_ptr cinfo, J_COLOR_SPACE in_color_space);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, EntryPoint = "jpeg_start_compress_12")]
        public static extern unsafe void jpeg_start_compress_12(ref j_compress_ptr cinfo, int write_all_tables);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, EntryPoint = "jpeg_write_scanlines_12")]
        public static extern unsafe uint jpeg_write_scanlines_12(ref j_compress_ptr cinfo, short** scanlines, uint num_lines);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, EntryPoint = "jpeg_finish_compress_12")]
        public static extern unsafe void jpeg_finish_compress_12(ref j_compress_ptr cinfo);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, EntryPoint = "jpeg_destroy_compress_12")]
        public static extern unsafe void jpeg_destroy_compress_12(ref j_compress_ptr cinfo);

        // Decode Native functions
        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_create_decompress_12")]
        public static extern unsafe void jpeg_create_decompress_12(ref j_decompress_ptr dinfo);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_read_header_12")]
        public static extern unsafe int jpeg_read_header_12(ref j_decompress_ptr dinfo, int require_image);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_calc_output_dimensions_12")]
        public static extern unsafe void jpeg_calc_output_dimensions_12(ref j_decompress_ptr dinfo);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_start_decompress_12")]
        public static extern unsafe int jpeg_start_decompress_12(ref j_decompress_ptr dinfo);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_read_scanlines_12")]
        public static extern unsafe uint jpeg_read_scanlines_12(ref j_decompress_ptr dinfo, short** scanlines, uint max_lines);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_destroy_decompress_12")]
        public static extern unsafe void jpeg_destroy_decompress_12(ref j_decompress_ptr dinfo);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_resync_to_restart_12")]
        public static extern unsafe int jpeg_resync_to_restart_12(ref j_decompress_ptr dinfo, int desired);


        // DLLIMPORT libijg16 library

        // Encode Native functions
        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_std_error_16")]
        public static extern unsafe jpeg_error_mgr* jpeg_std_error_16(ref jpeg_error_mgr err);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_create_compress_16")]
        public static extern unsafe void jpeg_create_compress_16(ref j_compress_ptr cinfo);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_set_defaults_16")]
        public static extern unsafe void jpeg_set_defaults_16(ref j_compress_ptr cinfo);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_set_quality_16")]
        public static extern unsafe void jpeg_set_quality_16(ref j_compress_ptr cinfo, int quality, int force_baseline);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_simple_progression_16")]
        public static extern unsafe void jpeg_simple_progression_16(ref j_compress_ptr cinfo);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_simple_lossless_16")]
        public static extern unsafe void jpeg_simple_lossless_16(ref j_compress_ptr cinfo, int predictor, int point_transform);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_set_colorspace_16")]
        public static extern unsafe void jpeg_set_colorspace_16(ref j_compress_ptr cinfo, J_COLOR_SPACE in_color_space);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_start_compress_16")]
        public static extern unsafe void jpeg_start_compress_16(ref j_compress_ptr cinfo, int write_all_tables);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_write_scanlines_16")]
        public static extern unsafe void jpeg_write_scanlines_16(ref j_compress_ptr cinfo, ushort** scanlines, uint num_lines);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_finish_compress_16")]
        public static extern unsafe void jpeg_finish_compress_16(ref j_compress_ptr cinfo);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_destroy_compress_16")]
        public static extern unsafe void jpeg_destroy_compress_16(ref j_compress_ptr cinfo);

        // Decode native functions
        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_create_decompress_16")]
        public static extern unsafe void jpeg_create_decompress_16(ref j_decompress_ptr dinfo);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_read_header_16")]
        public static extern unsafe int jpeg_read_header_16(ref j_decompress_ptr dinfo, int require_image);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_calc_output_dimensions_16")]
        public static extern unsafe void jpeg_calc_output_dimensions_16(ref j_decompress_ptr dinfo);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_start_decompress_16")]
        public static extern unsafe int jpeg_start_decompress_16(ref j_decompress_ptr dinfo);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_read_scanlines_16")]
        public static extern unsafe uint jpeg_read_scanlines_16(ref j_decompress_ptr dinfo, ushort** scanlines, uint max_lines);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_destroy_decompress_16")]
        public static extern unsafe void jpeg_destroy_decompress_16(ref j_decompress_ptr dinfo);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "jpeg_resync_to_restart_16")]
        public static extern unsafe int jpeg_resync_to_restart_16(ref j_decompress_ptr dinfo, int desired);
    }
}
