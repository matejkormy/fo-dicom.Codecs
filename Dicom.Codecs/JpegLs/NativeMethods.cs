
namespace Dicom.Codecs.JpegLs
{
    using System.Runtime.InteropServices;

    internal static class NativeMethods
    {
        //Encode JPEGLS for win_x64 or win_arm64
        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, EntryPoint = "JpegLSEncode")]
        public static extern unsafe CharlsApiResultType JpegLSEncode_win(void* destination, uint destinationLength, uint* bytesWritten, void* source, uint sourceLength, ref JlsParameters obj, char[] errorMessage);

        //Decode JPEGLS for winx64
        [DllImport("Dicom.Native.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "JpegLSDecode")]
        public static extern unsafe CharlsApiResultType JpegLSDecode_win(void* destination, int destinationLength, void* source, uint sourceLength, ref JlsParameters obj, char[] errorMessage);

        //For Encode JPEGLS
        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl, EntryPoint = "JpegLSEncode")]
        public static extern unsafe CharlsApiResultType JpegLSEncode(void* destination, uint destinationLength, uint* bytesWritten, void* source, uint sourceLength, ref JlsParameters obj, char[] errorMessage);

        //For Decode JPEGLS
        [DllImport("Dicom.Native", CallingConvention = CallingConvention.Cdecl, EntryPoint = "JpegLSDecode")]
        public static extern unsafe CharlsApiResultType JpegLSDecode(void* destination, int destinationLength, void* source, uint sourceLength, ref JlsParameters obj, char[] errorMessage);
    }
}
