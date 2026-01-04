
namespace Dicom.Codecs.HTJ2K
{
    using System;

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
