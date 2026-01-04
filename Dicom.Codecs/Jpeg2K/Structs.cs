
namespace Dicom.Codecs.Jpeg2K
{
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public unsafe struct opj_image_comp_t
    {
        /** XRsiz: horizontal separation of a sample of ith component with respect to the reference grid */
        public uint dx;
        /** YRsiz: vertical separation of a sample of ith component with respect to the reference grid */
        public uint dy;
        /** data width */
        public uint w;
        /** data height */
        public uint h;
        /** x component offset compared to the whole image */
        public uint x0;
        /** y component offset compared to the whole image */
        public uint y0;
        /** precision */
        public uint prec;
        /** image depth in bits */
        public uint bpp;
        /** signed (1) / unsigned (0) */
        public uint sgnd;
        /** number of decoded resolution */
        public uint resno_decoded;
        /** number of division by 2 of the out image compared to the original size of image */
        public uint factor;
        /** image component data */
        public int* data;
        public ushort alpha;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public unsafe struct opj_image_t
    {
        /** XOsiz: horizontal offset from the origin of the reference grid to the left side of the image area */
        public uint x0;
        /** YOsiz: vertical offset from the origin of the reference grid to the top side of the image area */
        public uint y0;
        /** Xsiz: width of the reference grid */
        public uint x1;
        /** Ysiz: height of the reference grid */
        public uint y1;
        /** number of components in the image */
        public uint numcomps;
        /** color space: sRGB, Greyscale or YUV */
        public OPJ_COLOR_SPACE color_space;
        /** image components */
        public opj_image_comp_t* comps;
        /** 'restricted' ICC profile */
        public byte* icc_profile_buf;
        /** size of ICC profile */
        public uint icc_profile_len;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public unsafe struct opj_cparameters_t
    {
        public int tile_size_on;
        public int cp_tx0;
        /** YTOsiz */
        public int cp_ty0;
        /** XTsiz */
        public int cp_tdx;
        /** YTsiz */
        public int cp_tdy;
        /** allocation by rate/distortion */
        public int cp_disto_alloc;
        /** allocation by fixed layer */
        public int cp_fixed_alloc;
        /** add fixed_quality */
        public int cp_fixed_quality;
        /** fixed layer */
        public unsafe int* cp_matrice;
        /** comment for coding */
        public unsafe char* cp_comment;
        /** csty : coding style */
        public int csty;
        public OPJ_PROG_ORDER prog_order;
        public opj_poc_t POC1;
        public opj_poc_t POC2;
        public opj_poc_t POC3;
        public opj_poc_t POC4;
        public opj_poc_t POC5;
        public opj_poc_t POC6;
        public opj_poc_t POC7;
        public opj_poc_t POC8;
        public opj_poc_t POC9;
        public opj_poc_t POC10;
        public opj_poc_t POC11;
        public opj_poc_t POC12;
        public opj_poc_t POC13;
        public opj_poc_t POC14;
        public opj_poc_t POC15;
        public opj_poc_t POC16;
        public opj_poc_t POC17;
        public opj_poc_t POC18;
        public opj_poc_t POC19;
        public opj_poc_t POC20;
        public opj_poc_t POC21;
        public opj_poc_t POC22;
        public opj_poc_t POC23;
        public opj_poc_t POC24;
        public opj_poc_t POC25;
        public opj_poc_t POC26;
        public opj_poc_t POC27;
        public opj_poc_t POC28;
        public opj_poc_t POC29;
        public opj_poc_t POC30;
        public opj_poc_t POC31;
        public opj_poc_t POC32;
        public uint numpocs;
        /** number of layers */
        public int tcp_numlayers;
        /** rates of layers */
        public unsafe fixed float tcp_rates[100];
        /** different psnr for successive layers */
        public unsafe fixed float tcp_distoratio[100];
        /** number of resolutions */
        public int numresolution;
        /** initial code block width, default to 64 */
        public int cblockw_init;
        /** initial code block height, default to 64 */
        public int cblockh_init;
        /** mode switch (cblk_style) */
        public int mode;
        /** 1 : use the irreversible DWT 9-7, 0 : use lossless compression (default) */
        public int irreversible;
        /** region of interest: affected component in [0..3], -1 means no ROI */
        public int roi_compno;
        /** region of interest: upshift value */
        public int roi_shift;
        /* number of precinct size specifications */
        public int res_spec;
        public unsafe fixed int prcw_init[33];
        /** initial precinct height */
        public unsafe fixed int prch_init[33];
        /** input file name */
        public unsafe fixed sbyte infile[4096];
        /** output file name */
        public unsafe fixed sbyte outfile[4096];
        /** DEPRECATED. Index generation is now handeld with the opj_encode_with_info() function. Set to NULL */
        public int index_on;
        /** DEPRECATED. Index generation is now handeld with the opj_encode_with_info() function. Set to NULL */
        public unsafe fixed sbyte index[4096];
        /** subimage encoding: origin image offset in x direction */
        public int image_offset_x0;
        /** subimage encoding: origin image offset in y direction */
        public int image_offset_y0;
        /** subsampling value for dx */
        public int subsampling_dx;
        /** subsampling value for dy */
        public int subsampling_dy;
        /** input file format 0: PGX, 1: PxM, 2: BMP 3:TIF*/
        public int decod_format;
        /** output file format 0: J2K, 1: JP2, 2: JPT */
        public int cod_format;
        public int jpwl_epc_on;
        /** error protection method for MH (0,1,16,32,37-128) */
        public int jpwl_hprot_MH;
        /** tile number of header protection specification (>=0) */
        public unsafe fixed int jpwl_hprot_TPH_tileno[16];
        /** error protection methods for TPHs (0,1,16,32,37-128) */
        public unsafe fixed int jpwl_hprot_TPH[16];
        /** tile number of packet protection specification (>=0) */
        public unsafe fixed int jpwl_pprot_tileno[16];
        /** packet number of packet protection specification (>=0) */
        public unsafe fixed int jpwl_pprot_packno[16];
        /** error protection methods for packets (0,1,16,32,37-128) */
        public unsafe fixed int jpwl_pprot[16];
        /** enables writing of ESD, (0=no/1/2 bytes) */
        public int jpwl_sens_size;
        /** sensitivity addressing size (0=auto/2/4 bytes) */
        public int jpwl_sens_addr;
        /** sensitivity range (0-3) */
        public int jpwl_sens_range;
        /** sensitivity method for MH (-1=no,0-7) */
        public int jpwl_sens_MH;
        /** tile number of sensitivity specification (>=0) */
        public unsafe fixed int jpwl_sens_TPH_tileno[16];
        /** sensitivity methods for TPHs (-1=no,0-7) */
        public unsafe fixed int jpwl_sens_TPH[16];
        public OPJ_CINEMA_MODE cp_cinema;
        /** Maximum rate for each component. If == 0, component size limitation is not considered */
        public int max_comp_size;
        /** Profile name*/
        public OPJ_RSIZ_CAPABILITIES cp_rsiz;
        /** Tile part generation*/
        public sbyte tp_on;
        /** Flag for Tile part generation*/
        public sbyte tp_flag;
        /** MCT (multiple component transform) */
        public sbyte tcp_mct;
        /** Enable JPIP indexing*/
        public int jpip_on;
        public void* mct_data;
        /**
        * Maximum size (in bytes) for the whole codestream.
        * If == 0, codestream size limitation is not considered
        * If it does not comply with tcp_rates, max_cs_size prevails
        * and a warning is issued.
        * */
        public int max_cs_size;
        public ushort rsiz;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct opj_poc_t
    {
        public uint resno0, compno0;
        /** Layer num end,Resolution num end, Component num end, given by POC */
        public uint layno1, resno1, compno1;
        /** Layer num start,Precinct num start, Precinct num end */
        public uint layno0, precno0, precno1;
        /** Progression order enum*/
        public OPJ_PROG_ORDER prg1, prg;
        /** Progression order string*/
        public unsafe fixed sbyte progorder[5];
        /** Tile number */
        public uint tile;
        /** Start and end values for Tile width and height*/
        public uint tx0, tx1, ty0, ty1;
        /** Start value, initialised in pi_initialise_encode*/
        public uint layS, resS, compS, prcS;
        /** End value, initialised in pi_initialise_encode */
        public uint layE, resE, compE, prcE;
        /** Start and end values of Tile width and height, initialised in pi_initialise_encode*/
        public uint txS, txE, tyS, tyE, dx, dy;
        /** Temporary values for Tile parts, initialised in pi_create_encode */
        public uint lay_t, res_t, comp_t, prc_t, tx0_t, ty0_t;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public unsafe struct opj_image_cmptparm_t
    {
        public uint dx;
        /** YRsiz: vertical separation of a sample of ith component with respect to the reference grid */
        public uint dy;
        /** data width */
        public uint w;
        /** data height */
        public uint h;
        /** x component offset compared to the whole image */
        public uint x0;
        /** y component offset compared to the whole image */
        public uint y0;
        /** precision */
        public uint prec;
        /** image depth in bits */
        public uint bpp;
        /** signed (1) / unsigned (0) */
        public uint sgnd;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public unsafe struct opj_dparameters_t
    {
        public uint cp_reduce;
        public uint cp_layer;
        public unsafe fixed sbyte infile[4096];
        public unsafe fixed sbyte outfile[4096];
        public int decod_format;
        public int cod_format;
        public uint DA_x0;
        public uint DA_x1;
        public uint DA_y0;
        public uint DA_y1;
        public int m_verbose;
        public uint tile_index;
        public uint nb_tile_to_decode;
        public int jpwl_correct;
        public int jpwl_exp_comps;
        public int jpwl_max_tiles;
        public uint flags;
    }
}
