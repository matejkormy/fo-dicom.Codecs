
namespace Dicom.Codecs.HTJ2K
{
    using System.Runtime.InteropServices;

    internal static class NativeMethods
    {
        // Encode HTJ2K for win_x64
        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "InvokeHTJ2KEncode")]
        public static extern unsafe void InvokeHTJ2KEncode_win(ref Htj2k_outdata j2c_outinfo, byte* source, uint sourceLength, ref Frameinfo frameinfo, OPJ_PROG_ORDER progressionOrder = OPJ_PROG_ORDER.PROG_UNKNOWN);

        // Decode HTJ2K for win_x64
        [DllImport("Dicom.Native.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "InvokeHTJ2KDecode")]
        public static extern unsafe void InvokeHTJ2KDecode_win(ref Raw_outdata raw_outinfo, byte* source, uint sourceLength);

        // Encode HTJ2k
        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "InvokeHTJ2KEncode")]
        public static extern unsafe void InvokeHTJ2KEncode(ref Htj2k_outdata j2c_outinfo, byte* source, uint sourceLength, ref Frameinfo frameinfo, OPJ_PROG_ORDER progressionOrder = OPJ_PROG_ORDER.PROG_UNKNOWN);

        // Decode HTJ2k
        [DllImport("Dicom.Native", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "InvokeHTJ2KDecode")]
        public static extern unsafe void InvokeHTJ2KDecode(ref Raw_outdata raw_outinfo, byte* source, uint sourceLength);
    }
}
