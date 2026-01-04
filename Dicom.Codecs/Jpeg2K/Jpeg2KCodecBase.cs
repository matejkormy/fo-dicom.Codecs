
namespace Dicom.Codecs.Jpeg2K
{
    using System;
    using System.Buffers;
    using System.Linq;
    using Dicom.Codecs.Common;

    internal abstract class Jpeg2KCodecBase : CodecBase
    {
        protected Jpeg2KCodecBase()
        {
            DefaultParams = new Jpeg2KCodecParams();
        }

        protected Jpeg2KCodecParams DefaultParams { get; set; }

        public override unsafe byte[] Encode(byte[] input, FrameMetadata metadata, out FrameMetadata outputMetadata, CodecParams codecParams = null)
        {
            if (metadata.PhotometricInterpretation == PhotometricInterpretation.YbrPartial422 ||
                metadata.PhotometricInterpretation == PhotometricInterpretation.YbrPartial420)
            {
                throw new CodecException($"Photometric Interpretation {metadata.PhotometricInterpretation} not supported by JPEG 2000 encoder");
            }

            Jpeg2KCodecParams jparams = codecParams as Jpeg2KCodecParams ?? DefaultParams;
            outputMetadata = (FrameMetadata)metadata.Clone();

            int pixelCount = metadata.Height * metadata.Width;

            var pool = ArrayPool<byte>.Shared;
            byte[] cbuf = null;

            byte[] frameData = input;

            //Converting photometric interpretation YbrFull or YbrFull422 to RGB
            if (metadata.PlanarConfiguration == PlanarConfiguration.Planar && metadata.SamplesPerPixel > 1)
            {
                if (metadata.SamplesPerPixel != 3 ||
                    metadata.BitsStored > 8)
                {
                    throw new CodecException("Planar reconfiguration only implemented for SamplesPerPixel=3 && BitsStored <= 8");
                }

                frameData = PixelDataConverter.PlanarToInterleaved24(frameData);
                metadata.PlanarConfiguration = PlanarConfiguration.Interleaved;

                if (metadata.PhotometricInterpretation == PhotometricInterpretation.YbrFull)
                    frameData = PixelDataConverter.YbrFullToRgb(frameData);
            }
            else if (metadata.PhotometricInterpretation == PhotometricInterpretation.YbrFull)
            {
                frameData = PixelDataConverter.YbrFullToRgb(frameData);
            }
            else if (metadata.PhotometricInterpretation == PhotometricInterpretation.YbrFull422)
            {
                frameData = PixelDataConverter.YbrFull422ToRgb(frameData, metadata.Width);
            }

            PinnedByteArray frameArray = new PinnedByteArray(frameData);

            try
            {
                opj_image_cmptparm_t[] cmptparm = new opj_image_cmptparm_t[3];

                opj_cparameters_t eparams = new opj_cparameters_t();
                void* codec = null;  /* handle to a compressor */
                opj_image_t* image = null;
                void* c_stream = null;

                if (Platform.IsWindows)
                {
                    codec = NativeMethods.Opj_create_compress_win(OPJ_CODEC_FORMAT.CODEC_J2K);
                }
                else
                {
                    codec = NativeMethods.Opj_create_compress(OPJ_CODEC_FORMAT.CODEC_J2K);
                }

                eparams.cp_cinema = OPJ_CINEMA_MODE.OFF;
                eparams.max_comp_size = 0;
                eparams.numresolution = 6;
                eparams.cp_rsiz = OPJ_RSIZ_CAPABILITIES.STD_RSIZ;
                eparams.cblockw_init = 64;
                eparams.cblockh_init = 64;
                eparams.prog_order = jparams.ProgressionOrder;
                eparams.roi_compno = -1;
                eparams.subsampling_dx = 1;
                eparams.subsampling_dy = 1;
                eparams.tp_on = 0;
                eparams.decod_format = -1;
                eparams.cod_format = -1;
                eparams.tcp_rates[0] = 0;
                eparams.tcp_numlayers = 0;
                eparams.cp_disto_alloc = 0;
                eparams.cp_fixed_alloc = 0;
                eparams.cp_fixed_quality = 0;
                eparams.jpip_on = 0;
                //eparams.cp_disto_alloc = 1;

                if (OutputTransferSyntax == TransferSyntax.JPEG2000Lossy && jparams.Irreversible)
                    eparams.irreversible = 1;

                int r = 0;
                for (; r < jparams.RateLevels.Length; r++)
                {
                    if (jparams.RateLevels[r] > jparams.Rate)
                    {
                        eparams.tcp_numlayers++;
                        eparams.tcp_rates[r] = (float)jparams.RateLevels[r];
                    }
                    else
                        break;
                }

                eparams.tcp_numlayers++;
                eparams.tcp_rates[r] = (float)(jparams.Rate * metadata.BitsStored / metadata.BitsAllocated);

                if (OutputTransferSyntax == TransferSyntax.JPEG2000Lossless && jparams.Rate > 0)
                    eparams.tcp_rates[eparams.tcp_numlayers++] = 0;

                if (metadata.PhotometricInterpretation == PhotometricInterpretation.Rgb && jparams.AllowMCT)
                    eparams.tcp_mct = 1;

                for (int i = 0; i < metadata.SamplesPerPixel; i++)
                {
                    cmptparm[i].bpp = metadata.BitsAllocated;
                    cmptparm[i].prec = metadata.BitsStored;
                    if (!jparams.EncodeSignedPixelValuesAsUnsigned)
                        cmptparm[i].sgnd = (uint)(metadata.IsSigned ? 1 : 0);

                    cmptparm[i].dx = (uint)eparams.subsampling_dx;
                    cmptparm[i].dy = (uint)eparams.subsampling_dy;
                    cmptparm[i].h = metadata.Height;
                    cmptparm[i].w = metadata.Width;
                }

                try
                {
                    OPJ_COLOR_SPACE color_space = GetOpenJpegColorSpace(metadata.PhotometricInterpretation);

                    if (Platform.IsWindows)
                        image = NativeMethods.Opj_image_create_win(metadata.SamplesPerPixel, ref cmptparm[0], color_space);
                    else
                        image = NativeMethods.Opj_image_create(metadata.SamplesPerPixel, ref cmptparm[0], color_space);

                    image->x0 = (uint)eparams.image_offset_x0;
                    image->y0 = (uint)eparams.image_offset_y0;
                    image->x1 = (uint)(image->x0 + ((metadata.Width - 1) * eparams.subsampling_dx) + 1);
                    image->y1 = (uint)(image->y0 + ((metadata.Height - 1) * eparams.subsampling_dy) + 1);

                    for (int c = 0; c < image->numcomps; c++)
                    {
                        opj_image_comp_t* comp = &image->comps[c];

                        int pos = metadata.PlanarConfiguration == PlanarConfiguration.Planar ? (c * pixelCount) : c;
                        int offset = metadata.PlanarConfiguration == PlanarConfiguration.Planar ? 1 : (int)image->numcomps;

                        if (metadata.BytesAllocated == 1)
                        {
                            if (Convert.ToBoolean(comp->sgnd))
                            {
                                if (metadata.BitsStored < 8)
                                {
                                    byte sign = (byte)(1 << metadata.HighBit);
                                    byte mask = (byte)(0xff >> (metadata.BitsAllocated - metadata.BitsStored));
                                    for (int p = 0; p < pixelCount; p++)
                                    {
                                        byte pixel = frameArray.Data[pos];
                                        if (Convert.ToBoolean(pixel & sign))
                                            comp->data[p] = -(((-pixel) & mask) + 1);
                                        else
                                            comp->data[p] = pixel;
                                        pos += offset;
                                    }
                                }
                                else
                                {
                                    char* frameData8 = (char*)(void*)frameArray.Pointer;
                                    for (int p = 0; p < pixelCount; p++)
                                    {
                                        comp->data[p] = frameData8[pos];
                                        pos += offset;
                                    }
                                }
                            }
                            else
                            {
                                for (int p = 0; p < pixelCount; p++)
                                {
                                    comp->data[p] = frameArray.Data[pos];
                                    pos += offset;
                                }
                            }
                        }
                        else if (metadata.BytesAllocated == 2)
                        {
                            if (Convert.ToBoolean(comp->sgnd))
                            {
                                if (metadata.BitsStored < 16)
                                {
                                    ushort* frameData16 = (ushort*)(void*)frameArray.Pointer;
                                    ushort sign = (ushort)(1 << metadata.HighBit);
                                    ushort mask = (ushort)(0xffff >> (metadata.BitsAllocated - metadata.BitsStored));
                                    for (int p = 0; p < pixelCount; p++)
                                    {
                                        ushort pixel = frameData16[pos];
                                        if (Convert.ToBoolean(pixel & sign))
                                            comp->data[p] = -(((-pixel) & mask) + 1);
                                        else
                                            comp->data[p] = pixel;
                                        pos += offset;
                                    }
                                }
                                else
                                {
                                    short* frameData16 = (short*)frameArray.Pointer.ToPointer();
                                    for (int p = 0; p < pixelCount; p++)
                                    {
                                        comp->data[p] = frameData16[pos];
                                        pos += offset;
                                    }
                                }
                            }
                            else
                            {
                                if (metadata.BitsStored < 16)
                                {
                                    ushort* frameData16 = (ushort*)frameArray.Pointer.ToPointer();
                                    ushort mask = (ushort)(0xffff >> (metadata.BitsAllocated - metadata.BitsStored));

                                    for (int p = 0; p < pixelCount; p++)
                                    {
                                        ushort pixel = frameData16[pos];
                                        comp->data[p] = pixel & mask;
                                        pos += offset;
                                    }
                                }
                                else
                                {
                                    ushort* frameData16 = (ushort*)frameArray.Pointer.ToPointer();
                                    for (int p = 0; p < pixelCount; p++)
                                    {
                                        comp->data[p] = frameData16[pos];
                                        pos += offset;
                                    }
                                }
                            }
                        }
                        else
                            throw new CodecException("JPEG 2000 codec only supports Bits Allocated == 8 or 16");
                    }

                    uint img_size = 0;
                    for (int i = 0; i < image->numcomps; i++)
                    {
                        img_size += image->comps[i].w * image->comps[i].h * image->comps[i].prec;
                    }

                    var outlen = (uint)(0.1625 * img_size + 2000); /* 0.1625 = 1.3/8 and 2000 bytes as a minimum for headers */
                    var buf = new PinnedByteArray(new byte[outlen]);

                    if (Platform.IsWindows)
                    {
                        NativeMethods.Opj_setup_encoder_win(codec, ref eparams, image);
                        c_stream = NativeMethods.Opj_create_stream_win((byte*)buf.Pointer, (uint)buf.ByteSize, false);
                    }
                    else
                    {
                        NativeMethods.Opj_setup_encoder(codec, ref eparams, image);
                        c_stream = NativeMethods.Opj_create_stream((byte*)buf.Pointer, (uint)buf.ByteSize, false);
                    }

                    var isEncodeSuccess = false;
                    if (Platform.Current.Equals(Platform.Type.win_x64) || Platform.Current.Equals(Platform.Type.win_arm64))
                        isEncodeSuccess = Convert.ToBoolean(NativeMethods.Opj_encode_win(codec, c_stream, image));
                    else
                        isEncodeSuccess = Convert.ToBoolean(NativeMethods.Opj_encode(codec, c_stream, image));

                    if (isEncodeSuccess)
                    {
                        int clen = 0;

                        if (Platform.IsWindows)
                            clen = (int)NativeMethods.Opj_stream_tell_win(c_stream);
                        else
                            clen = (int)NativeMethods.Opj_stream_tell(c_stream);

                        //cbuf = pool.Rent(clen);
                        //Marshal.Copy(buf.Pointer, cbuf, 0, clen);
                        cbuf = buf.Data.Take(clen).ToArray(); // in case stuff doesn't work making buffer even size may help

                    }
                    else
                        throw new CodecException("Unable to JPEG 2000 encode image");
                }
                catch (Exception e)
                {
                    throw new CodecException(e.Message, e);
                }
                finally
                {
                    if (c_stream != null)
                    {
                        if (Platform.IsWindows)
                            NativeMethods.Opj_stream_close_win(c_stream);
                        else
                            NativeMethods.Opj_stream_close(c_stream);
                    }

                    if (image != null)
                    {
                        if (Platform.IsWindows)
                            NativeMethods.Opj_image_destroy_win(image);
                        else
                            NativeMethods.Opj_image_destroy(image);
                    }

                    if (codec != null)
                    {
                        if (Platform.IsWindows)
                            NativeMethods.Opj_destroy_compress_win(codec);
                        else
                            NativeMethods.Opj_destroy_compress(codec);
                    }
                }
            }
            catch (Exception e)
            {
                throw new CodecException(e.Message, e);
            }
            finally
            {
                if (frameArray != null)
                {
                    frameArray.Dispose();
                    frameArray = null;
                }
            }

            if (metadata.PhotometricInterpretation == PhotometricInterpretation.Rgb ||
                metadata.PhotometricInterpretation == PhotometricInterpretation.YbrFull ||
                metadata.PhotometricInterpretation == PhotometricInterpretation.YbrFull422)
            {
                outputMetadata.PlanarConfiguration = PlanarConfiguration.Interleaved;

                if (jparams.AllowMCT && jparams.UpdatePhotometricInterpretation)
                {
                    if (OutputTransferSyntax == TransferSyntax.JPEG2000Lossy && jparams.Irreversible)
                        outputMetadata.PhotometricInterpretation = PhotometricInterpretation.YbrIct;
                    else
                        outputMetadata.PhotometricInterpretation = PhotometricInterpretation.YbrRct;
                }
            }

            return cbuf;
        }

        public override void Decode(byte[] input, byte[] output, FrameMetadata metadata, out FrameMetadata outputMetadata, CodecParams codecParams = null)
        {
            Jpeg2KCodecParams jparams = codecParams as Jpeg2KCodecParams ?? DefaultParams;
            outputMetadata = (FrameMetadata)metadata.Clone();

            // TODO: New pixelData used originally so there may be a problem with metadata,
            // but new pixel data were copied from old so it should not make difference
            if (outputMetadata.PhotometricInterpretation == PhotometricInterpretation.YbrIct ||
                outputMetadata.PhotometricInterpretation == PhotometricInterpretation.YbrRct)
            {
                outputMetadata.PhotometricInterpretation = PhotometricInterpretation.Rgb;
            }

            if (outputMetadata.PhotometricInterpretation == PhotometricInterpretation.YbrFull422 ||
                outputMetadata.PhotometricInterpretation == PhotometricInterpretation.YbrPartial422 ||
                outputMetadata.PhotometricInterpretation == PhotometricInterpretation.YbrFull)
            {
                outputMetadata.PhotometricInterpretation = PhotometricInterpretation.Rgb;
            }

            //if (newPixelData.PhotometricInterpretation == PhotometricInterpretation.YbrFull)
            //    newPixelData.PlanarConfiguration = PlanarConfiguration.Planar;

                byte[] j2kData = input;

                try
                {

                    j2kData = ConvertToRgbIfNeeded(j2kData, metadata);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Cannot convert J2k buffer data from PhotometricInterpretation = {0} to RGB => {1} => {2}",
                    metadata.PhotometricInterpretation, ex.Message, ex.StackTrace);
                }

                PinnedByteArray j2kArray = new PinnedByteArray(j2kData);
                PinnedByteArray destArray = new PinnedByteArray(output);

                try
                {
                    unsafe
                    {
                        opj_dparameters_t dparams = new opj_dparameters_t();
                        opj_image_t* image = null;
                        void* codec = null;
                        void* d_stream = null;

                        if (Platform.IsWindows)
                            NativeMethods.Opj_set_default_decode_parameters_win(&dparams);
                        else
                            NativeMethods.Opj_set_default_decode_parameters(&dparams);

                        dparams.cp_layer = 0;
                        dparams.cp_reduce = 0;

                        byte* buf = (byte*)(void*)j2kArray.Pointer;

                        OPJ_CODEC_FORMAT format;

                        try
                        {
                            format = OPJ_CODEC_FORMAT.CODEC_UNKNOWN;
                            if (Platform.IsWindows)
                            {
                                format = NativeMethods.GetCodecFormat_win(buf);
                                dparams.decod_format = (int)format;

                                codec = NativeMethods.Opj_create_decompress_win(format);
                                NativeMethods.Opj_setup_decoder_win(codec, &dparams);

                                d_stream = NativeMethods.Opj_create_stream_win(buf, (uint)j2kArray.ByteSize, true);
                                image = NativeMethods.Opj_decode_win(codec, d_stream);
                            }
                            else
                            {
                                format = NativeMethods.GetCodecFormat(buf);
                                dparams.decod_format = (int)format;

                                codec = NativeMethods.Opj_create_decompress(format);
                                NativeMethods.Opj_setup_decoder(codec, &dparams);

                                d_stream = NativeMethods.Opj_create_stream(buf, (uint)j2kArray.ByteSize, true);
                                image = NativeMethods.Opj_decode(codec, d_stream);
                            }

                            int pixelCount = 0;

                            if (image == null)
                            {
                                throw new CodecException("Error in JPEG 2000 decode stream => output image data is null");
                            }
                            else
                            {
                                pixelCount = (int)(image->x1 * image->y1);
                            }

                            for (int c = 0; c < image->numcomps; c++)
                            {
                                opj_image_comp_t* comp = &image->comps[c];

                                if (comp->data == null)
                                {
                                    throw new CodecException("Error in JPEG 2000 decode stream => output image component data is null");
                                }
                                else
                                {
                                    if (comp->h != image->y1)
                                        throw new CodecException("Error in JPEG 2000 decode stream");

                                    if (comp->w != image->x1)
                                        throw new CodecException("Error in JPEG 2000 decode stream");
                                }

                                int pos = metadata.PlanarConfiguration == PlanarConfiguration.Planar ? (c * pixelCount) : c;
                                int offset = (int)(metadata.PlanarConfiguration == PlanarConfiguration.Planar ? 1 : image->numcomps);

                                var prec = comp->prec < metadata.BitsStored ? metadata.BitsStored : comp->prec;

                                if (metadata.BytesAllocated == 1)
                                {
                                    if (prec <= 8)
                                    {
                                        if (Convert.ToBoolean(comp->sgnd))
                                        {
                                            byte sign = (byte)(1 << (byte)(comp->prec - 1));
                                            byte mask = (byte)(0xFF ^ sign);
                                            for (int p = 0; p < pixelCount; p++)
                                            {
                                                try
                                                {
                                                    int i = comp->data[p];
                                                    if (i < 0)
                                                        //destArray->Data[pos] = (unsigned char)(-i | sign);
                                                        destArray.Data[pos] = (byte)((i & mask) | sign);
                                                    else
                                                        //destArray->Data[pos] = (unsigned char)(i);
                                                        destArray.Data[pos] = (byte)(i & mask);
                                                    pos += offset;
                                                }
                                                catch (Exception e)
                                                {
                                                    throw new CodecException(e.Message, e);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            for (int p = 0; p < pixelCount; p++)
                                            {
                                                try
                                                {
                                                    destArray.Data[pos] = (byte)comp->data[p];
                                                    pos += offset;
                                                }
                                                catch (Exception e)
                                                {
                                                    throw new CodecException(e.Message, e);
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (metadata.BytesAllocated == 2)
                                {
                                    if (prec <= 16)
                                    {
                                        ushort sign = (ushort)(1 << (ushort)(comp->prec - 1));
                                        ushort mask = (ushort)(0xFFFF ^ sign);
                                        ushort* destData16 = (ushort*)(void*)destArray.Pointer;

                                        if (Convert.ToBoolean(comp->sgnd))
                                        {
                                            try
                                            {
                                                for (int p = 0; p < pixelCount; p++)
                                                {
                                                    int i = comp->data[p];

                                                    if (i < 0)
                                                        destData16[pos] = (ushort)((i & mask) | sign);
                                                    else
                                                        destData16[pos] = (ushort)(i & mask);
                                                    pos += offset;
                                                }
                                            }
                                            catch (Exception e)
                                            {
                                                throw new CodecException(e.Message, e);
                                            }
                                        }
                                        else
                                        {
                                            for (int p = 0; p < pixelCount; p++)
                                            {
                                                try
                                                {
                                                    var pixel = (ushort)comp->data[p];
                                                    destData16[pos] = pixel;
                                                    pos += offset;
                                                }
                                                catch (Exception e)
                                                {
                                                    throw new CodecException(e.Message, e);
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                    throw new CodecException("JPEG 2000 module only supports bits Allocated == 8 or 16!");
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new CodecException(ex.Message, ex);
                        }
                        finally
                        {
                            if (codec != null)
                            {
                                if (Platform.IsWindows)
                                    NativeMethods.Opj_destroy_decompress_win(codec);
                                else
                                    NativeMethods.Opj_destroy_decompress(codec);
                            }

                            if (image != null)
                            {
                                if (Platform.IsWindows)
                                    NativeMethods.Opj_image_destroy_win(image);
                                else
                                    NativeMethods.Opj_image_destroy(image);
                            }

                            if (d_stream != null)
                            {
                                if (Platform.IsWindows)
                                {
                                    NativeMethods.Opj_stream_close_win(d_stream);
                                }
                                else
                                    NativeMethods.Opj_stream_close(d_stream);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new CodecException(ex.Message, ex);
                }
                finally
                {
                    if (j2kArray != null)
                    {
                        j2kArray.Dispose();
                        j2kArray = null;
                    }

                    if (destArray != null)
                    {
                        destArray.Dispose();
                        destArray = null;
                    }
                }
        }

        private OPJ_COLOR_SPACE GetOpenJpegColorSpace(PhotometricInterpretation photometricInterpretation)
            => photometricInterpretation switch
            {
                PhotometricInterpretation.Rgb => OPJ_COLOR_SPACE.CLRSPC_SRGB,
                PhotometricInterpretation.Monochrome1 => OPJ_COLOR_SPACE.CLRSPC_GRAY,
                PhotometricInterpretation.Monochrome2 => OPJ_COLOR_SPACE.CLRSPC_GRAY,
                PhotometricInterpretation.PaletteColor => OPJ_COLOR_SPACE.CLRSPC_GRAY,
                PhotometricInterpretation.YbrFull => OPJ_COLOR_SPACE.CLRSPC_SYCC,
                PhotometricInterpretation.YbrFull422 => OPJ_COLOR_SPACE.CLRSPC_SYCC,
                PhotometricInterpretation.YbrPartial422 => OPJ_COLOR_SPACE.CLRSPC_SYCC,
                _ => OPJ_COLOR_SPACE.CLRSPC_UNKNOWN,
            };
    }
}
