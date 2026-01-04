
namespace Dicom.Codecs.Jpeg2K
{
    using System;

    [Flags]
    public enum OPJ_CODEC_FORMAT
    {
        CODEC_UNKNOWN = -1, /**< place-holder */
        CODEC_J2K = 0,      /**< JPEG-2000 codestream : read/write */
        CODEC_JPT = 1,      /**< JPT-stream (JPEG 2000, JPIP) : read only */
        CODEC_JP2 = 2       /**< JPEG-2000 file format : read/write */
    }

    [Flags]
    public enum OPJ_COLOR_SPACE
    {
        CLRSPC_UNKNOWN = -1,    /**< not supported by the library */
        CLRSPC_UNSPECIFIED = 0, /**< not specified in the codestream */
        CLRSPC_SRGB = 1,        /**< sRGB */
        CLRSPC_GRAY = 2,        /**< grayscale */
        CLRSPC_SYCC = 3
    }

    [Flags]
    public enum OPJ_RSIZ_CAPABILITIES
    {
        STD_RSIZ = 0,       /** Standard JPEG2000 profile*/
        CINEMA2K = 3,       /** Profile name for a 2K image*/
        CINEMA4K = 4		/** Profile name for a 4K image*/
    }

    [Flags]
    public enum OPJ_CINEMA_MODE
    {
        OFF = 0,                    /** Not Digital Cinema*/
        CINEMA2K_24 = 1,    /** 2K Digital Cinema at 24 fps*/
        CINEMA2K_48 = 2,    /** 2K Digital Cinema at 48 fps*/
        CINEMA4K_24 = 3		/** 4K Digital Cinema at 24 fps*/
    }

    [Flags]
    public enum OPJ_PROG_ORDER
    {
        PROG_UNKNOWN = -1,  /**< place-holder */
        LRCP = 0,       /**< layer-resolution-component-precinct order */
        RLCP = 1,       /**< resolution-layer-component-precinct order */
        RPCL = 2,       /**< resolution-precinct-component-layer order */
        PCRL = 3,       /**< precinct-component-resolution-layer order */
        CPRL = 4		/**< component-precinct-resolution-layer order */
    }
}
