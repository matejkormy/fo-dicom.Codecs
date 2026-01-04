
namespace Dicom.Codecs.Jpeg
{
    using System;

    [Flags]
    public enum J_BUF_MODE
    {
        JBUF_PASS_THRU,     /* Plain stripwise operation */
        JBUF_SAVE_SOURCE,   /* Run source subobject only, save output */
        JBUF_CRANK_DEST,    /* Run dest subobject only, using saved data */
        JBUF_SAVE_AND_PASS
    }

    [Flags]
    public enum J_CODEC_PROCESS
    {
        JPROC_SEQUENTIAL,       /* baseline/extended sequential DCT */
        JPROC_PROGRESSIVE,      /* progressive DCT */
        JPROC_LOSSLESS
    }

    [Flags]
    public enum J_DCT_METHOD
    {
        JDCT_ISLOW,             /* slow but accurate integer algorithm */
        JDCT_IFAST,             /* faster, less accurate integer method */
        JDCT_FLOAT
    }

    [Flags]
    public enum J_COLOR_SPACE
    {
        JCS_UNKNOWN,            /* error/unspecified */
        JCS_GRAYSCALE,          /* monochrome */
        JCS_RGB,                /* red/green/blue */
        JCS_YCbCr,              /* Y/Cb/Cr (also known as YUV) */
        JCS_CMYK,               /* C/M/Y/K */
        JCS_YCCK
    }

    [Flags]
    public enum J_DITHER_MODE
    {
        JDITHER_NONE,           /* no dithering */
        JDITHER_ORDERED,        /* simple ordered dither */
        JDITHER_FS
    }

    public enum DicomJpegSampleFactor
    {
        SF444,
        SF422,
        Unknown
    }

    public enum JpegMode : int
    {
        Baseline,
        Sequential,
        SpectralSelection,
        Progressive,
        Lossless
    };
}
