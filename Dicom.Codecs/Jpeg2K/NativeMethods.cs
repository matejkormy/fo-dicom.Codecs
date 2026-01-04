
namespace Dicom.Codecs.Jpeg2K
{
    using System.Runtime.InteropServices;

    internal static class NativeMethods
    {
        //Encode OpenJPEG library for win_x64 or win_arm64

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Opj_create_compress")]
        public static extern unsafe void* Opj_create_compress_win(OPJ_CODEC_FORMAT format);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Opj_image_create")]
        public static extern unsafe opj_image_t* Opj_image_create_win(uint numcmpts, ref opj_image_cmptparm_t cmptparms, OPJ_COLOR_SPACE clrspc);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Opj_setup_encoder")]
        public static extern unsafe void Opj_setup_encoder_win(void* codec, ref opj_cparameters_t parameters, opj_image_t* image);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Opj_create_stream")]
        public static extern unsafe void* Opj_create_stream_win(byte* buffer, uint length, bool isDecompressor);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Opj_encode")]
        public static extern unsafe int Opj_encode_win(void* codec, void* stream, opj_image_t* image);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Opj_stream_close")]
        public static extern unsafe void Opj_stream_close_win(void* stream);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Opj_image_destroy")]
        public static extern unsafe void Opj_image_destroy_win(opj_image_t* image);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Opj_destroy_compress")]
        public static extern unsafe void Opj_destroy_compress_win(void* codec);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Opj_stream_tell")]
        public static extern unsafe long Opj_stream_tell_win(void* stream);

        //Decode OpenJPEG library for win_x64 or win_arm64

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Opj_create_decompress")]
        public static extern unsafe void* Opj_create_decompress_win(OPJ_CODEC_FORMAT format);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Opj_setup_decoder")]
        public static extern unsafe void Opj_setup_decoder_win(void* codec, opj_dparameters_t* parameters);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Opj_set_default_decode_parameters")]
        public static extern unsafe void Opj_set_default_decode_parameters_win(opj_dparameters_t* parameters);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Opj_decode")]
        public static extern unsafe opj_image_t* Opj_decode_win(void* codec, void* stream);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Opj_destroy_decompress")]
        public static extern unsafe void Opj_destroy_decompress_win(void* codec);

        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetCodecFormat")]
        public static extern unsafe OPJ_CODEC_FORMAT GetCodecFormat_win(byte* buffer);

        //Encode OpenJPEG library

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Opj_create_compress")]
        public static extern unsafe void* Opj_create_compress(OPJ_CODEC_FORMAT format);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Opj_image_create")]
        public static extern unsafe opj_image_t* Opj_image_create(int numcmpts, ref opj_image_cmptparm_t cmptparms, OPJ_COLOR_SPACE clrspc);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Opj_setup_encoder")]
        public static extern unsafe void Opj_setup_encoder(void* codec, ref opj_cparameters_t parameters, opj_image_t* image);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Opj_create_stream")]
        public static extern unsafe void* Opj_create_stream(byte* buffer, uint length, bool isDecompressor);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Opj_encode")]
        public static extern unsafe int Opj_encode(void* codec, void* stream, opj_image_t* image);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Opj_stream_close")]
        public static extern unsafe void Opj_stream_close(void* stream);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Opj_image_destroy")]
        public static extern unsafe void Opj_image_destroy(opj_image_t* image);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Opj_destroy_compress")]
        public static extern unsafe void Opj_destroy_compress(void* codec);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Opj_stream_tell")]
        public static extern unsafe long Opj_stream_tell(void* stream);

        //Decode OpenJPEG library

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Opj_create_decompress")]
        public static extern unsafe void* Opj_create_decompress(OPJ_CODEC_FORMAT format);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Opj_setup_decoder")]
        public static extern unsafe void Opj_setup_decoder(void* codec, opj_dparameters_t* parameters);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Opj_set_default_decode_parameters")]
        public static extern unsafe void Opj_set_default_decode_parameters(opj_dparameters_t* parameters);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Opj_decode")]
        public static extern unsafe opj_image_t* Opj_decode(void* codec, void* stream);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Opj_destroy_decompress")]
        public static extern unsafe void Opj_destroy_decompress(void* codec);

        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetCodecFormat")]
        public static extern unsafe OPJ_CODEC_FORMAT GetCodecFormat(byte* buffer);
    }
}
