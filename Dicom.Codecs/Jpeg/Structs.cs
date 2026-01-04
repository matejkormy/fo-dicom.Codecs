
namespace Dicom.Codecs.Jpeg
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public unsafe struct SourceManagerStruct
    {
        public jpeg_source_mgr pub;
        public int skip_bytes;
        public IntPtr next_buffer;
        public uint next_buffer_size;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public unsafe struct j_common_ptr
    {
        public jpeg_error_mgr* err;
        public jpeg_progress_mgr* progress;
        public jpeg_memory_mgr* mem;
        public void* client_data;
        public int is_decompressor;
        public int global_state;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public unsafe struct j_compress_ptr
    {
        public jpeg_error_mgr* err;
        public jpeg_progress_mgr* progress;
        public jpeg_memory_mgr* mem;
        public void* client_data;
        public int is_decompressor;
        public int global_state;
        public jpeg_destination_mgr* dest;
        public uint image_width;
        public uint image_height;
        public int input_components;
        public J_COLOR_SPACE in_color_space;
        public double input_gamma;
        public int lossless;
        public int data_precision;
        public int num_components;
        public J_COLOR_SPACE jpeg_color_space;
        public jpeg_component_info* comp_info;
        public JQUANT_TBL* quant_tbl_ptrs_1;
        public JQUANT_TBL* quant_tbl_ptrs_2;
        public JQUANT_TBL* quant_tbl_ptrs_3;
        public JQUANT_TBL* quant_tbl_ptrs_4;
        public JHUFF_TBL* dc_huff_tbl_ptrs_1;
        public JHUFF_TBL* dc_huff_tbl_ptrs_2;
        public JHUFF_TBL* dc_huff_tbl_ptrs_3;
        public JHUFF_TBL* dc_huff_tbl_ptrs_4;
        public JHUFF_TBL* ac_huff_tbl_ptrs_1;
        public JHUFF_TBL* ac_huff_tbl_ptrs_2;
        public JHUFF_TBL* ac_huff_tbl_ptrs_3;
        public JHUFF_TBL* ac_huff_tbl_ptrs_4;
        public fixed byte arith_dc_L[16];
        public fixed byte arith_dc_U[16];
        public fixed byte arith_ac_K[16];
        public int num_scans;
        public jpeg_scan_info* scan_info;
        public int raw_data_in;          /* TRUE=caller supplies downsampled data */
        public int arith_code;           /* TRUE=arithmetic coding, FALSE=Huffman */
        public int optimize_coding;      /* TRUE=optimize entropy encoding parms */
        public int CCIR601_sampling;     /* TRUE=first samples are cosited */
        public int smoothing_factor;
        public J_DCT_METHOD dct_method;
        public uint restart_interval;
        public int restart_in_rows;
        public int write_JFIF_header;    /* should a JFIF marker be written? */
        public byte JFIF_major_version;     /* What to write for the JFIF version number */
        public byte JFIF_minor_version;
        public byte density_unit;           /* JFIF code for pixel size units */
        public ushort X_density;             /* Horizontal pixel density */
        public ushort Y_density;             /* Vertical pixel density */
        public int write_Adobe_marker;
        public uint next_scanline;
        public int data_unit;                /* size of data unit in samples */
        public J_CODEC_PROCESS process;      /* encoding process of JPEG image */
        public int max_h_samp_factor;        /* largest h_samp_factor */
        public int max_v_samp_factor;
        public uint total_iMCU_rows;
        public int comps_in_scan;
        public jpeg_component_info* cur_comp_info_1;
        public jpeg_component_info* cur_comp_info_2;
        public jpeg_component_info* cur_comp_info_3;
        public jpeg_component_info* cur_comp_info_4;
        public uint MCUs_per_row;      /* # of MCUs across the image */
        public uint MCU_rows_in_scan;
        public int data_units_in_MCU;
        public fixed int MCU_membership[10];
        public int Ss;
        public int Se;
        public int Ah;
        public int Al;
        public jpeg_comp_master* master;
        public jpeg_c_main_controller* main;
        public jpeg_c_prep_controller* prep;
        public jpeg_c_codec* codec;
        public jpeg_marker_writer* marker;
        public jpeg_color_converter* cconvert;
        public jpeg_downsampler* downsample;
        public jpeg_scan_info* script_space;
        public int script_space_size;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public unsafe struct j_decompress_ptr
    {
        public jpeg_error_mgr* err;
        public jpeg_progress_mgr* progress;
        public jpeg_memory_mgr* mem;
        public void* client_data;
        public int is_decompressor;
        public int global_state;
        public jpeg_source_mgr* src;
        public uint image_width;
        public uint image_height;
        public int num_components;
        public J_COLOR_SPACE jpeg_color_space;
        public J_COLOR_SPACE out_color_space;
        public uint scale_num;
        public uint scale_denom;
        public double output_gamma;
        public int buffered_image;
        public int raw_data_out;
        public J_DCT_METHOD dct_method;
        public int do_fancy_upsampling;
        public int do_block_smoothing;
        public int quantize_colors;
        public J_DITHER_MODE dither_mode;
        public int two_pass_quantize;
        public int desired_number_of_colors;
        public int enable_1pass_quant;
        public int enable_external_quant;
        public int enable_2pass_quant;
        public uint output_width;
        public uint output_height;
        public int out_color_components;
        public int output_components;
        public int rec_outbuf_height;
        public int actual_number_of_colors;
        public IntPtr colormap;
        public uint output_scanline;
        public int input_scan_number;
        public uint input_iMCU_row;
        public int output_scan_number;
        public uint output_iMCU_row;
        // int (* coef_bits)[64];
        public IntPtr coef_bits;
        public JQUANT_TBL* quant_tbl_ptrs_1;
        public JQUANT_TBL* quant_tbl_ptrs_2;
        public JQUANT_TBL* quant_tbl_ptrs_3;
        public JQUANT_TBL* quant_tbl_ptrs_4;
        public JHUFF_TBL* dc_huff_tbl_ptrs_1;
        public JHUFF_TBL* dc_huff_tbl_ptrs_2;
        public JHUFF_TBL* dc_huff_tbl_ptrs_3;
        public JHUFF_TBL* dc_huff_tbl_ptrs_4;
        public JHUFF_TBL* ac_huff_tbl_ptrs_1;
        public JHUFF_TBL* ac_huff_tbl_ptrs_2;
        public JHUFF_TBL* ac_huff_tbl_ptrs_3;
        public JHUFF_TBL* ac_huff_tbl_ptrs_4;
        public int data_precision;
        public jpeg_component_info* comp_info;
        public int arith_code;
        public fixed byte arith_dc_L[16];
        public fixed byte arith_dc_U[16];
        public fixed byte arith_dc_K[16];
        public uint restart_interval;
        public int saw_JFIF_marker;
        public byte JFIF_major_version;
        public byte JFIF_minor_version;
        public byte density_unit;
        public ushort X_density;
        public ushort Y_density;
        public int saw_Adobe_marker;
        public byte Adobe_transform;
        public int CCIR601_sampling;
        public jpeg_marker_struct* marker_list;
        public int data_unit;
        public J_CODEC_PROCESS process;
        public int max_h_samp_factor;
        public int max_v_samp_factor;
        public int min_codec_data_unit;
        public uint total_iMCU_rows;
        public IntPtr sample_range_limit;
        public int comps_in_scan;
        public jpeg_component_info* cur_comp_info_1;
        public jpeg_component_info* cur_comp_info_2;
        public jpeg_component_info* cur_comp_info_3;
        public jpeg_component_info* cur_comp_info_4;
        public uint MCUs_per_row;
        public uint MCU_rows_in_scan;
        public int data_units_in_MCU;
        public fixed int MCU_membership[10];
        public int Ss;
        public int Se;
        public int Ah;
        public int Al;
        public int unread_marker;
        public IntPtr master;
        public IntPtr main;
        public IntPtr codec;
        public IntPtr post;
        public IntPtr inputctl;
        public IntPtr marker;
        public IntPtr upsample;
        public IntPtr cconvert;
        public IntPtr cquantize;
        public uint workaround_options;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public unsafe struct jpeg_memory_mgr
    {
        public IntPtr alloc_small;
        public IntPtr alloc_large;
        public IntPtr alloc_sarray;
        public IntPtr alloc_barray;
        public IntPtr alloc_darray;
        public IntPtr request_virt_sarray;
        public IntPtr request_virt_barray;
        public IntPtr realize_virt_arrays;
        public IntPtr access_virt_sarray;
        public IntPtr access_virt_barray;
        public IntPtr free_pool;
        public IntPtr self_destruct;
        public int max_memory_to_use;
        public int max_alloc_chunk;
    }

    [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
    public unsafe delegate void ouput_Message(j_common_ptr* cinfo);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public unsafe struct jpeg_error_mgr
    {
        public IntPtr error_exit;
        public IntPtr emit_message;
        public IntPtr output_message;
        public IntPtr format_message;
        public IntPtr reset_error_mgr;
        public int msg_code;
        public msg_parm msg_Parm;
        public int trace_level;
        public int num_warnings;
        public char* jpeg_message_table;
        public int last_jpeg_message;
        public char* addon_message_table;
        public int first_addon_message;
        public int last_addon_message;
    }

    [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
    public unsafe struct msg_parm
    {
        [FieldOffset(0)]
        public fixed int i[8];
        [FieldOffset(0)]
        public fixed sbyte s[80];
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public unsafe struct JBLOCKARRAY
    {
        public fixed short JBLOCK[64];
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public unsafe struct jpeg_progress_mgr
    {
        public IntPtr progress_monitor;
        public int pass_counter;            /* work units completed in this pass */
        public int pass_limit;              /* total number of work units in this pass */
        public int completed_passes;         /* passes completed so far */
        public int total_passes;             /* total number of passes expected */
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public unsafe struct jpeg_destination_mgr
    {
        public IntPtr next_output_byte;
        public uint free_in_buffer;
        public IntPtr init_Destination;
        public IntPtr empty_Output_Buffer;
        public IntPtr term_Destination;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public unsafe struct jpeg_source_mgr
    {
        public IntPtr next_input_byte;
        public uint bytes_in_buffer;
        public IntPtr init_source;
        public IntPtr fill_input_buffer;
        public IntPtr skip_input_data;
        public IntPtr resync_to_restart;
        public IntPtr term_source;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public unsafe struct jpeg_component_info
    {
        public int component_id;             /* identifier for this component (0..255) */
        public int component_index;          /* its index in SOF or cinfo->comp_info[] */
        public int h_samp_factor;            /* horizontal sampling factor (1..4) */
        public int v_samp_factor;            /* vertical sampling factor (1..4) */
        public int quant_tbl_no;
        public int dc_tbl_no;                /* DC entropy table selector (0..3) */
        public int ac_tbl_no;
        public uint width_in_data_units;
        public uint height_in_data_units;
        public int codec_data_unit;
        public uint downsampled_width;
        public uint downsampled_height;
        public int component_needed;
        public int MCU_width;                /* number of data units per MCU, horizontally */
        public int MCU_height;               /* number of data units per MCU, vertically */
        public int MCU_data_units;           /* MCU_width * MCU_height */
        public int MCU_sample_width;         /* MCU width in samples, MCU_width*codec_data_unit */
        public int last_col_width;           /* # of non-dummy data_units across in last MCU */
        public int last_row_height;
        public void* dct_table;
        public JQUANT_TBL* quant_table;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public unsafe struct jpeg_downsampler
    {
        public IntPtr start_pass;
        public IntPtr downsample;
        public int need_context_rows;
    }


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public unsafe struct jpeg_color_converter
    {
        public IntPtr start_pass;
        public IntPtr color_convert;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public unsafe struct jpeg_marker_struct
    {
        public jpeg_marker_struct* next;
        public byte marker;
        public uint original_length;
        public int data_length;
        public IntPtr data;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public unsafe struct jpeg_marker_writer
    {
        public IntPtr write_file_header;
        public IntPtr write_frame_header;
        public IntPtr write_scan_header;
        public IntPtr write_file_trailer;
        public IntPtr write_tables_only;
        public IntPtr write_marker_header;
        public IntPtr write_marker_byte;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public unsafe struct jpeg_c_codec
    {
        public IntPtr entropy_start_pass;
        public IntPtr entropy_finish_pass;
        public IntPtr need_optimization_pass;
        public IntPtr start_pass;
        public IntPtr compress_data;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public unsafe struct jpeg_c_prep_controller
    {
        public IntPtr start_pass;
        public IntPtr pre_process_data;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public unsafe struct jpeg_c_main_controller
    {
        public IntPtr start_pass;
        public IntPtr process_data;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public unsafe struct jpeg_comp_master
    {
        public IntPtr prepare_for_pass;
        public IntPtr pass_startup;
        public IntPtr finish_pass;
        public int call_pass_startup;  /* True if pass_startup must be called */
        public int is_last_pass;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public unsafe struct jpeg_scan_info
    {
        public int comps_in_scan;
        public fixed int component_index[4];
        public int Ss;
        public int Se;
        public int Ah;
        public int Al;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public unsafe struct JHUFF_TBL
    {
        public fixed byte bits[17];
        public fixed byte huffval[256];
        public int sent_table;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public unsafe struct JQUANT_TBL
    {
        public fixed ushort quantval[64];
        public int sent_table;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public unsafe struct jvirt_sarray_control
    {
        public IntPtr mem_buffer;
        public uint rows_in_array;
        public uint samplesperrow;
        public uint maxaccess;
        public uint rows_in_mem;
        public uint rowsperchunk;
        public uint cur_start_row;
        public uint first_undef_row;
        public int pre_zero;
        public int dirty;
        public int b_s_open;
        public jvirt_sarray_control* next;
        public backing_store_struct b_s_info;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public unsafe struct backing_store_struct
    {
        public IntPtr read_backing_store;
        public IntPtr write_backing_store;
        public IntPtr close_backing_store;
        public fixed sbyte temp_name[64];
        public FILE* temp_file;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public unsafe struct FILE
    {
        public IntPtr _placeholder;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public unsafe struct jvirt_barray_control
    {
        public IntPtr mem_buffer;
        public uint rows_in_array;
        public uint samplesperrow;
        public uint maxaccess;
        public uint rows_in_mem;
        public uint rowsperchunk;
        public uint cur_start_row;
        public uint first_undef_row;
        public int pre_zero;
        public int dirty;
        public int b_s_open;
        public jvirt_sarray_control* next;
        public backing_store_struct b_s_info;
    }
}
