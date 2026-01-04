
namespace Dicom.Codecs.HTJ2K
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct Raw_outdata
    {
        public unsafe byte* buffer;
        public unsafe uint size_outbuffer;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct Htj2k_outdata
    {
        public unsafe byte* buffer;
        public unsafe uint size_outbuffer;
    }

    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct Frameinfo
    {
        /// <summary>
        /// Width of the image, range [1, 65535].
        /// </summary>
        public ushort width;

        /// <summary>
        /// Height of the image, range [1, 65535].
        /// </summary>
        public ushort height;

        /// <summary>
        /// Number of bits per sample, range [2, 16]
        /// </summary>
        public byte bitsPerSample;

        /// <summary>
        /// Number of components contained in the frame, range [1, 255]
        /// </summary>
        public byte componentCount;

        /// <summary>
        /// true if signed, false if unsigned
        /// </summary>
        [MarshalAs(UnmanagedType.I1)] public bool isSigned;

        /// <summary>
        /// true if color transform is used, false if not
        /// </summary>
        [MarshalAs(UnmanagedType.I1)] public bool isUsingColorTransform;

        /// <summary>
        /// true if lossless, false is lossy
        /// </summary>
        [MarshalAs(UnmanagedType.I1)] public bool isReversible;
    }
}
