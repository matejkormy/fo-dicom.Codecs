
namespace Dicom.Codecs.Common
{
    using System;

    public struct FrameMetadata : ICloneable
    {
        /// <summary>
        /// Width of the image, range [1, 65535].
        /// </summary>
        public ushort Width;

        /// <summary>
        /// Height of the image, range [1, 65535].
        /// </summary>
        public ushort Height;

        /// <summary>
        /// Number of bits per sample, range [2, 16]
        /// </summary>
        public byte BitsAllocated;

        /// <summary>
        /// Number of bits stored per sample, range [1, BitsAllocated]
        /// </summary>
        public byte BitsStored;

        /// <summary>
        /// Number of bytes per sample
        /// </summary>
        public int BytesAllocated => (BitsAllocated - 1) / 8 + 1;

        /// <summary>
        /// Highest bit position, range [0, BitsStored - 1]
        /// </summary>
        public ushort HighBit;

        /// <summary>
        /// Number of components contained in the frame, range [1, 255]
        /// </summary>
        public byte SamplesPerPixel;

        /// <summary>
        /// true if signed, false if unsigned
        /// </summary>
        public bool IsSigned;

        /// <summary>
        /// Photometric Interpretation of the image
        /// </summary>
        public PhotometricInterpretation PhotometricInterpretation;

        /// <summary>
        /// Planar Configuration of the image
        /// </summary>
        public PlanarConfiguration PlanarConfiguration;

        /// <summary>
        /// Uncompressed size of the frame in bytes
        /// </summary>
        public long UncompressedSize;

        public object Clone()
        {
            return new FrameMetadata
            {
                Width = Width,
                Height = Height,
                BitsAllocated = BitsAllocated,
                BitsStored = BitsStored,
                HighBit = HighBit,
                SamplesPerPixel = SamplesPerPixel,
                IsSigned = IsSigned,
                PhotometricInterpretation = PhotometricInterpretation,
                PlanarConfiguration = PlanarConfiguration,
                UncompressedSize = UncompressedSize
            };
        }
    }
}
