
namespace Dicom.Codecs.Common
{
    public enum PlanarConfiguration
    {
        /// <summary>
        /// The sample values for the first pixel are followed by the sample values for the second
        /// pixel, etc. For RGB images, this means the order of the pixel values sent shall be R1,
        /// G1, B1, R2, G2, B2, ..., etc.
        /// </summary>
        Interleaved = 0,

        /// <summary>
        /// Each color plane shall be sent contiguously. For RGB images, this means the order of
        /// the pixel values sent is R1, R2, R3, ..., G1, G2, G3, ..., B1, B2, B3, etc.
        /// </summary>
        Planar = 1
    }

    public enum PhotometricInterpretation
    {
        /// <summary>
        /// Pixel data represent a single monochrome image plane. The minimum sample value is intended
        /// to be displayed as white after any VOI gray scale transformations have been performed. See
        /// PS 3.4. This value may be used only when Samples per Pixel (0028,0002) has a value of 1.
        /// </summary>
         Monochrome1,

        /// <summary>
        /// Pixel data represent a single monochrome image plane. The minimum sample value is intended
        /// to be displayed as black after any VOI gray scale transformations have been performed. See
        /// PS 3.4. This value may be used only when Samples per Pixel (0028,0002) has a value of 1.
        /// </summary>
         Monochrome2,

        /// <summary>
        /// Pixel data describe a color image with a single sample per pixel (single image plane). The
        /// pixel value is used as an index into each of the Red, Blue, and Green Palette Color Lookup
        /// Tables (0028,1101-1103 &amp; 1201-1203). This value may be used only when Samples per Pixel (0028,0002)
        /// has a value of 1. When the Photometric Interpretation is Palette Color; Red, Blue, and Green
        /// Palette Color Lookup Tables shall be present.
        /// </summary>
         PaletteColor,

        /// <summary>
        /// Pixel data represent a color image described by red, green, and blue image planes. The minimum
        /// sample value for each color plane represents minimum intensity of the color. This value may be
        /// used only when Samples per Pixel (0028,0002) has a value of 3.
        /// </summary>
         Rgb,

        /// <summary>
        /// Pixel data represent a color image described by one luminance (Y) and two chrominance planes
        /// (Cb and Cr). This photometric interpretation may be used only when Samples per Pixel (0028,0002)
        /// has a value of 3. Black is represented by Y equal to zero. The absence of color is represented
        /// by both Cb and Cr values equal to half full scale.
        ///
        /// In the case where Bits Allocated (0028,0100) has a value of 8 then the following equations convert
        /// between RGB and YCBCR Photometric Interpretation:
        /// Y  = + .2990R + .5870G + .1140B
        /// Cb = - .1687R - .3313G + .5000B + 128
        /// Cr = + .5000R - .4187G - .0813B + 128
        /// </summary>
         YbrFull,

        /// <summary>
        /// The same as YBR_FULL except that the Cb and Cr values are sampled horizontally at half the Y rate
        /// and as a result there are half as many Cb and Cr values as Y values.
        ///
        /// This Photometric Interpretation is only allowed with Planar Configuration (0028,0006) equal to 0.
        /// Two Y values shall be stored followed by one Cb and one Cr value. The Cb and Cr values shall be
        /// sampled at the location of the first of the two Y values. For each Row of Pixels, the first Cb and
        /// Cr samples shall be at the location of the first Y sample. The next Cb and Cr samples shall be
        /// at the location of the third Y sample etc.
        /// </summary>
         YbrFull422,

        /// <summary>
        /// The same as YBR_FULL_422 except that:
        /// <list type="number">
        /// <item>black corresponds to Y = 16</item>
        /// <item>Y is restricted to 220 levels (i.e. the maximum value is 235)</item>
        /// <item>Cb and Cr each has a minimum value of 16</item>
        /// <item>Cb and Cr are restricted to 225 levels (i.e. the maximum value is 240)</item>
        /// <item>lack of color is represented by Cb and Cr equal to 128</item>
        /// </list>
        ///
        /// In the case where Bits Allocated (0028,0100) has value of 8 then the following equations convert
        /// between RGB and YBR_PARTIAL_422 Photometric Interpretation:
        /// Y  = + .2568R + .5041G + .0979B + 16
        /// Cb = - .1482R - .2910G + .4392B + 128
        /// Cr = + .4392R - .3678G - .0714B + 128
        /// </summary>
         YbrPartial422,

        /// <summary>
        /// The same as YBR_PARTIAL_422 except that the Cb and Cr values are sampled horizontally and vertically
        /// at half the Y rate and as a result there are four times less Cb and Cr values than Y values, versus
        /// twice less for YBR_PARTIAL_422.
        ///
        /// This Photometric Interpretation is only allowed with Planar Configuration (0028,0006) equal to 0.
        /// The Cb and Cr values shall be sampled at the location of the first of the two Y values. For the first
        /// Row of Pixels (etc.), the first Cb and Cr samples shall be at the location of the first Y sample.  The
        /// next Cb and Cr samples shall be at the location of the third Y sample etc. The next Rows of Pixels
        /// containing Cb and Cr samples (at the same locations than for the first Row) will be the third etc.
        /// </summary>
         YbrPartial420,

        /// <summary>
        /// Pixel data represent a color image described by one luminance (Y) and two chrominance planes
        /// (Cb and Cr). This photometric interpretation may be used only when Samples per Pixel (0028,0002) has
        /// a value of 3. Black is represented by Y equal to zero. The absence of color is represented by both
        /// Cb and Cr values equal to zero.
        ///
        /// Regardless of the value of Bits Allocated (0028,0100), the following equations convert between RGB
        /// and YCbCr Photometric Interpretation:
        /// Y  = + .29900R + .58700G + .11400B
        /// Cb = - .16875R - .33126G + .50000B
        /// Cr = + .50000R - .41869G - .08131B
        /// </summary>
         YbrIct,

        /// <summary>
        /// Pixel data represent a color image described by one luminance (Y) and two chrominance planes
        /// (Cb and Cr). This photometric interpretation may be used only when Samples per Pixel (0028,0002)
        /// has a value of 3. Black is represented by Y equal to zero. The absence of color is represented
        /// by both Cb and Cr values equal to zero.
        ///
        /// Regardless of the value of Bits Allocated (0028,0100), the following equations convert between
        /// RGB and YBR_RCT Photometric Interpretation:
        /// Y  = floor((R + 2G +B) / 4)
        /// Cb = B - G
        /// Cr = R - G
        ///
        /// The following equations convert between YBR_RCT and RGB Photometric Interpretation:
        /// R = Cr + G
        /// G = Y – floor((Cb + Cr) / 4)
        /// B = Cb + G
        /// </summary>
         YbrRct
    }

    public enum TransferSyntax
    {
        /// <summary>Virtual transfer syntax for reading datasets improperly encoded in Big Endian format with implicit VR.</summary>
        ImplicitVRBigEndian,
        /// <summary>GE Private Implicit VR Big Endian</summary>
        /// <remarks>Same as Implicit VR Little Endian except for big endian pixel data.</remarks>
        GEPrivateImplicitVRBigEndian,
        /// <summary>Implicit VR Little Endian</summary>
        ImplicitVRLittleEndian,
        /// <summary>Explicit VR Little Endian</summary>
        ExplicitVRLittleEndian,
        /// <summary>Explicit VR Big Endian</summary>
        ExplicitVRBigEndian,
        /// <summary>Deflated Explicit VR Little Endian</summary>
        DeflatedExplicitVRLittleEndian,
        /// <summary>JPEG Baseline (Process 1)</summary>
        JPEGProcess1,
        /// <summary>JPEG Extended (Process 2 &amp; 4)</summary>
        JPEGProcess2_4,
        /// <summary>JPEG Extended (Process 3 &amp; 5) (Retired)</summary>
        JPEGProcess3_5Retired,
        /// <summary>JPEG Spectral Selection, Non-Hierarchical (Process 6 &amp; 8) (Retired)</summary>
        JPEGProcess6_8Retired,
        /// <summary>JPEG Spectral Selection, Non-Hierarchical (Process 7 &amp; 9) (Retired)</summary>
        JPEGProcess7_9Retired,
        /// <summary>JPEG Spectral Selection, Non-Hierarchical (Process 10 &amp; 12) (Retired)</summary>
        JPEGProcess10_12Retired,
        /// <summary>JPEG Full Progression, Non-Hierarchical (Process 11 &amp; 13) (Retired)</summary>
        JPEGProcess11_13Retired,
        /// <summary>JPEG Lossless, Non-Hierarchical (Process 14)</summary>
        JPEGProcess14,
        /// <summary>JPEG Lossless, Non-Hierarchical (Process 15) (Retired)</summary>
        JPEGProcess15Retired,
        /// <summary>JPEG Extended, Hierarchical (Process 16 &amp; 18) (Retired)</summary>
        JPEGProcess16_18Retired,
        /// <summary>JPEG Extended, Hierarchical (Process 17 &amp; 19) (Retired)</summary>
        JPEGProcess17_19Retired,
        /// <summary>JPEG Spectral Selection, Hierarchical (Process 20 &amp; 22) (Retired)</summary>
        JPEGProcess20_22Retired,
        /// <summary>JPEG Spectral Selection, Hierarchical (Process 21 &amp; 23) (Retired)</summary>
        JPEGProcess21_23Retired,
        /// <summary>JPEG Full Progression, Hierarchical (Process 24 &amp; 26) (Retired)</summary>
        JPEGProcess24_26Retired,
        /// <summary>JPEG Full Progression, Hierarchical (Process 25 &amp; 27) (Retired)</summary>
        JPEGProcess25_27Retired,
        /// <summary>JPEG Lossless, Hierarchical (Process 28) (Retired)</summary>
        JPEGProcess28Retired,
        /// <summary>JPEG Lossless, Hierarchical (Process 29) (Retired)</summary>
        JPEGProcess29Retired,
        /// <summary>JPEG Lossless, Non-Hierarchical, First-Order Prediction (Process 14 [Selection Value 1])</summary>
        JPEGProcess14SV1,
        /// <summary>JPEG-LS Lossless Image Compression</summary>
        JPEGLSLossless,
        /// <summary>JPEG-LS Lossy (Near-Lossless) Image Compression</summary>
        JPEGLSNearLossless,
        /// <summary>JPEG 2000 Lossless Image Compression</summary>
        JPEG2000Lossless,
        /// <summary>JPEG 2000 Lossy Image Compression</summary>
        JPEG2000Lossy,
        ///<summary>JPEG 2000 Part 2 Multi-component Image Compression (Lossless Only)</summary>
        JPEG2000Part2MultiComponentLosslessOnly,
        ///<summary>JPEG 2000 Part 2 Multi-component Image Compression</summary>
        JPEG2000Part2MultiComponent,
        ///<summary>JPIP Referenced</summary>
        JPIPReferenced,
        ///<summary>JPIP Referenced Deflate</summary>
        JPIPReferencedDeflate,
        /// <summary>MPEG2 Main Profile @ Main Level</summary>
        MPEG2,
        /// <summary>Fragmentable MPEG2 Main Profile @ Main Level</summary>
        FragmentableMPEG2,
        ///<summary>MPEG2 Main Profile / High Level</summary>
        MPEG2MainProfileHighLevel,
        ///<summary>Fragmentable MPEG2 Main Profile / High Level</summary>
        FragmentableMPEG2MainProfileHighLevel,
        ///<summary>MPEG-4 AVC/H.264 High Profile / Level 4.1</summary>
        MPEG4AVCH264HighProfileLevel41,
        ///<summary>Fragmentable MPEG-4 AVC/H.264 High Profile / Level 4.1</summary>
        FragmentableMPEG4AVCH264HighProfileLevel41,
        ///<summary>MPEG-4 AVC/H.264 BD-compatible High Profile / Level 4.1</summary>
        MPEG4AVCH264BDCompatibleHighProfileLevel41,
        ///<summary>Fragmentable MPEG-4 AVC/H.264 BD-compatible High Profile / Level 4.1</summary>
        FragmentableMPEG4AVCH264BDCompatibleHighProfileLevel41,
        ///<summary>MPEG-4 AVC/H.264 High Profile / Level 4.2 For 2D Video</summary>
        MPEG4AVCH264HighProfileLevel42For2DVideo,
        ///<summary>Fragmentable MPEG-4 AVC/H.264 High Profile / Level 4.2 For 2D Video</summary>
        FragmentableMPEG4AVCH264HighProfileLevel42For2DVideo,
        ///<summary>MPEG-4 AVC/H.264 High Profile / Level 4.2 For 3D Video</summary>
        MPEG4AVCH264HighProfileLevel42For3DVideo,
        ///<summary>Fragmentable MPEG-4 AVC/H.264 High Profile / Level 4.2 For 3D Video</summary>
        FragmentableMPEG4AVCH264HighProfileLevel42For3DVideo,
        ///<summary>MPEG-4 AVC/H.264 Stereo High Profile / Level 4.2</summary>
        MPEG4AVCH264StereoHighProfileLevel42,
        ///<summary>Fragmentable MPEG-4 AVC/H.264 Stereo High Profile / Level 4.2</summary>
        FragmentableMPEG4AVCH264StereoHighProfileLevel42,
        ///<summary>HEVC/H.265 Main Profile / Level 5.1</summary>
        HEVCH265MainProfileLevel51,
        ///<summary>HEVC/H.265 Main 10 Profile / Level 5.1</summary>
        HEVCH265Main10ProfileLevel51,
        /// <summary>High-Throughput JPEG 2000 Image Compression (Lossless Only)</summary>
        HTJ2KLossless,
        /// <summary>High-Throughput JPEG 2000 with RPCL Options Image Compression (Lossless Only)</summary>
        HTJ2KLosslessRPCL,
        ///<summary>High-Throughput JPEG 2000 Image Compression</summary>
        HTJ2K,
        ///<summary>JPIP HTJ2K Referenced</summary>
        JPIPHTJ2KReferenced,
        ///<summary>JPIP HTJ2K Referenced Deflate</summary>
        JPIPHTJ2KReferencedDeflate,
        /// <summary>RLE Lossless</summary>
        RLELossless,
        ///<summary>RFC 2557 MIME encapsulation</summary>
        RFC2557MIMEEncapsulation,
        ///<summary>XML Encoding</summary>
        XMLEncoding,
        ///<summary>Papyrus 3 Implicit VR Little Endian (Retired)</summary>
        Papyrus3ImplicitVRLittleEndianRetired,
    }
}
