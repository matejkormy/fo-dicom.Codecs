
namespace Dicom.Codecs.Jpeg
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using Dicom.Codecs.Common;

    internal class JpegCodec : JpegCodecBase
    {
        public JpegCodec(
            TransferSyntax inputSyntax,
            TransferSyntax outputSyntax,
            JpegMode mode,
            int predictor,
            int point_transform,
            int bits)
            : base(
                  mode,
                  predictor,
                  point_transform,
                  bits)
        {
            TransferSyntax = inputSyntax;
            OutputTransferSyntax = outputSyntax;
        }

        [ThreadStatic]
        protected static JpegCodec This;

        public override TransferSyntax TransferSyntax { get; }

        public override TransferSyntax OutputTransferSyntax { get; }

        public override byte[] Encode(byte[] input, FrameMetadata metadata, out FrameMetadata outMetadata, CodecParams codecParams = null)
        {
            JpegCodecParams jpegParams = codecParams as JpegCodecParams ?? DefaultParams;
            outMetadata = (FrameMetadata)metadata.Clone();

            if (metadata.PhotometricInterpretation == PhotometricInterpretation.YbrIct ||
                metadata.PhotometricInterpretation == PhotometricInterpretation.YbrRct)
            {
                throw new CodecException($"Photometric Interpretation {metadata.PhotometricInterpretation} not supported by JPEG encoder!");
            }

            PinnedByteArray frameArray = null;

            if (metadata.BitsAllocated == 16 && metadata.BitsStored <= 8)
            {
                frameArray = new PinnedByteArray(ByteConverter.UnpackLow16(input));
            }
            else
            {
                frameArray = new PinnedByteArray(input);

                if (metadata.PhotometricInterpretation == PhotometricInterpretation.YbrFull422)
                {
                    frameArray = new PinnedByteArray(PixelDataConverter.YbrFull422ToRgb(frameArray.Data, metadata.Width));
                }
            }

            try
            {
                if (metadata.PlanarConfiguration == PlanarConfiguration.Planar && metadata.SamplesPerPixel > 1)
                {
                    if (metadata.SamplesPerPixel != 3 || metadata.BitsStored > 8)
                        throw new CodecException("Planar reconfiguration only implemented for SamplesPerPixel=3 && BitsStored <= 8");

                    outMetadata.PlanarConfiguration = PlanarConfiguration.Interleaved;
                    frameArray = new PinnedByteArray(PixelDataConverter.PlanarToInterleaved24(frameArray.Data));
                }

                unsafe
                {
                    j_compress_ptr cinfo = new j_compress_ptr();
                    jpeg_error_mgr jerr = new jpeg_error_mgr();

                    //jpeg_std_error 8, 12 and 16 bit for Linux, Windows and Osx64 for 64 bits
                    if (Bits == 8)
                    {
                        if (Platform.IsWindows)
                            cinfo.err = NativeMethods.jpeg_std_error_8_win(ref jerr);
                        else
                            cinfo.err = NativeMethods.jpeg_std_error_8(ref jerr);
                    }
                    else if (Bits <= 12 && Bits > 8)
                    {
                        if (Platform.IsWindows)
                            cinfo.err = NativeMethods.jpeg_std_error_12_win(ref jerr);
                        else
                            cinfo.err = NativeMethods.jpeg_std_error_12(ref jerr);
                    }
                    else if (Bits <= 16 && Bits > 12)
                    {
                        if (Platform.IsWindows)
                            cinfo.err = NativeMethods.jpeg_std_error_16_win(ref jerr);
                        else
                            cinfo.err = NativeMethods.jpeg_std_error_16(ref jerr);
                    }

                    ouput_Message errorexit_ = null;
                    ouput_Message ouput_Message_ = null;

                    errorexit_ = OutputMessage;
                    jerr.error_exit = Marshal.GetFunctionPointerForDelegate(errorexit_);

                    ouput_Message_ = OutputMessage;
                    jerr.output_message = Marshal.GetFunctionPointerForDelegate(ouput_Message_);

                    //jpeg_create_compress 8, 12 and 16 bit for Linux, Windows and Osx for 64 bits
                    if (Bits.Equals(8))
                    {
                        if (Platform.IsWindows)
                            NativeMethods.jpeg_create_compress_8_win(ref cinfo);
                        else
                            NativeMethods.jpeg_create_compress_8(ref cinfo);
                    }
                    else if (Bits <= 12 && Bits > 8)
                    {
                        if (Platform.IsWindows)
                            NativeMethods.jpeg_create_compress_12_win(ref cinfo);
                        else
                            NativeMethods.jpeg_create_compress_12(ref cinfo);
                    }
                    else if (Bits <= 16 && Bits > 12)
                    {
                        if (Platform.IsWindows)
                            NativeMethods.jpeg_create_compress_16_win(ref cinfo);
                        else
                            NativeMethods.jpeg_create_compress_16(ref cinfo);
                    }

                    cinfo.client_data = null;

                    This = this;
                    // Specify destination manager
                    jpeg_destination_mgr dest = new jpeg_destination_mgr();

                    init_destination init_Destination_ = initDestination;
                    dest.init_Destination = Marshal.GetFunctionPointerForDelegate(init_Destination_);

                    empty_output_buffer empty_Output_Buffer_ = emptyOutputBuffer;
                    dest.empty_Output_Buffer = Marshal.GetFunctionPointerForDelegate(empty_Output_Buffer_);

                    term_destination term_Destination_ = termDestination;
                    dest.term_Destination = Marshal.GetFunctionPointerForDelegate(term_Destination_);

                    cinfo.dest = &dest;
                    cinfo.image_width = metadata.Width;
                    cinfo.image_height = metadata.Height;
                    cinfo.input_components = metadata.SamplesPerPixel;
                    cinfo.in_color_space = GetJpegColorSpace(metadata.PhotometricInterpretation);

                    //jpeg_set_defaults 8, 12 and 16 bit for Linux, Windows and Osx for 64 bits
                    if (Bits.Equals(8))
                    {
                        if (Platform.IsWindows)
                            NativeMethods.jpeg_set_defaults_8_win(ref cinfo);
                        else
                            NativeMethods.jpeg_set_defaults_8(ref cinfo);
                    }
                    else if (Bits <= 12 && Bits > 8)
                    {
                        if (Platform.IsWindows)
                            NativeMethods.jpeg_set_defaults_12_win(ref cinfo);
                        else
                            NativeMethods.jpeg_set_defaults_12(ref cinfo);
                    }
                    else if (Bits <= 16 && Bits > 12)
                    {
                        if (Platform.IsWindows)
                            NativeMethods.jpeg_set_defaults_16_win(ref cinfo);
                        else
                            NativeMethods.jpeg_set_defaults_16(ref cinfo);
                    }

                    cinfo.optimize_coding = 1;

                    if (Mode == JpegMode.Baseline || Mode == JpegMode.Sequential)
                    {
                        //jpeg_set_quality 8, 12 and 16 bit for Linux, Windows and Osx for 64 bits
                        if (Bits.Equals(8))
                        {
                            if (Platform.IsWindows)
                                NativeMethods.jpeg_set_quality_8_win(ref cinfo, jpegParams.Quality, Convert.ToInt32(false));
                            else
                                NativeMethods.jpeg_set_quality_8(ref cinfo, jpegParams.Quality, Convert.ToInt32(false));
                        }
                        else if (Bits <= 12 && Bits > 8)
                        {
                            if (Platform.IsWindows)
                                NativeMethods.jpeg_set_quality_12_win(ref cinfo, jpegParams.Quality, Convert.ToInt32(false));
                            else
                                NativeMethods.jpeg_set_quality_12(ref cinfo, jpegParams.Quality, Convert.ToInt32(false));
                        }
                        else if (Bits <= 16 && Bits > 12)
                        {
                            if (Platform.IsWindows)
                                NativeMethods.jpeg_set_quality_16_win(ref cinfo, jpegParams.Quality, Convert.ToInt32(false));
                            else
                                NativeMethods.jpeg_set_quality_16(ref cinfo, jpegParams.Quality, Convert.ToInt32(false));
                        }
                    }
                    else if (Mode == JpegMode.SpectralSelection)
                    {
                        //jpeg_set_quality 8, 12 and 16 bit for Linux, Windows and Osx for 64 bits
                        if (Bits.Equals(8))
                        {
                            if (Platform.IsWindows)
                                NativeMethods.jpeg_set_quality_8_win(ref cinfo, jpegParams.Quality, Convert.ToInt32(false));
                            else
                                NativeMethods.jpeg_set_quality_8(ref cinfo, jpegParams.Quality, Convert.ToInt32(false));
                        }
                        else if (Bits <= 12 && Bits > 8)
                        {
                            if (Platform.IsWindows)
                                NativeMethods.jpeg_set_quality_12_win(ref cinfo, jpegParams.Quality, Convert.ToInt32(false));
                            else
                                NativeMethods.jpeg_set_quality_12(ref cinfo, jpegParams.Quality, Convert.ToInt32(false));
                        }
                        else if (Bits <= 16 && Bits > 12)
                        {
                            if (Platform.IsWindows)
                                NativeMethods.jpeg_set_quality_16_win(ref cinfo, jpegParams.Quality, Convert.ToInt32(false));
                            else
                                NativeMethods.jpeg_set_quality_16(ref cinfo, jpegParams.Quality, Convert.ToInt32(false));
                        }

                        jpeg_simple_spectral_selection(ref cinfo);

                    }
                    else if (Mode == JpegMode.Progressive)
                    {
                        //jpeg_set_quality 8, 12 and 16 bit for Linux, Windows and Osx for 64 bits
                        if (Bits.Equals(8))
                        {
                            if (Platform.IsWindows)
                            {
                                NativeMethods.jpeg_set_quality_8_win(ref cinfo, jpegParams.Quality, Convert.ToInt32(false));
                                NativeMethods.jpeg_simple_progression_8_win(ref cinfo);
                            }
                            else
                            {
                                NativeMethods.jpeg_set_quality_8(ref cinfo, jpegParams.Quality, Convert.ToInt32(false));
                                NativeMethods.jpeg_simple_progression_8(ref cinfo);
                            }
                        }
                        else if (Bits <= 12 && Bits > 8)
                        {
                            if (Platform.IsWindows)
                            {
                                NativeMethods.jpeg_set_quality_12_win(ref cinfo, jpegParams.Quality, Convert.ToInt32(false));
                                NativeMethods.jpeg_simple_progression_12_win(ref cinfo);
                            }
                            else
                            {
                                NativeMethods.jpeg_set_quality_12(ref cinfo, jpegParams.Quality, Convert.ToInt32(false));
                                NativeMethods.jpeg_simple_progression_12(ref cinfo);
                            }
                        }
                        else if (Bits <= 16 && Bits > 12)
                        {
                            if (Platform.IsWindows)
                            {
                                NativeMethods.jpeg_set_quality_16_win(ref cinfo, jpegParams.Quality, Convert.ToInt32(false));
                                NativeMethods.jpeg_simple_progression_16_win(ref cinfo);
                            }
                            else
                            {
                                NativeMethods.jpeg_set_quality_16(ref cinfo, jpegParams.Quality, Convert.ToInt32(false));
                                NativeMethods.jpeg_simple_progression_16(ref cinfo);
                            }
                        }
                    }
                    else
                    {
                        //jpeg_simple_lossless 8, 12 and 16 bit for Linux, Windows and Osx for 64 bits
                        if (Bits.Equals(8))
                        {
                            if (Platform.IsWindows)
                                NativeMethods.jpeg_simple_lossless_8_win(ref cinfo, Predictor, PointTransform);
                            else
                                NativeMethods.jpeg_simple_lossless_8(ref cinfo, Predictor, PointTransform);
                        }
                        else if (Bits <= 12 && Bits > 8)
                        {
                            if (Platform.IsWindows)
                                NativeMethods.jpeg_simple_lossless_12_win(ref cinfo, Predictor, PointTransform);
                            else
                                NativeMethods.jpeg_simple_lossless_12(ref cinfo, Predictor, PointTransform);
                        }
                        else if (Bits <= 16 && Bits > 12)
                        {
                            if (Platform.IsWindows)
                                NativeMethods.jpeg_simple_lossless_16_win(ref cinfo, Predictor, PointTransform);
                            else
                                NativeMethods.jpeg_simple_lossless_16(ref cinfo, Predictor, PointTransform);
                        }
                    }

                    cinfo.smoothing_factor = jpegParams.SmoothingFactor;

                    if (Mode == JpegMode.Lossless)
                    {
                        //jpeg_set_colorspace 8, 12 and 16 bit for Linux, Windows and Osx for 64 bits
                        if (Bits.Equals(8))
                        {
                            if (Platform.IsWindows)
                                NativeMethods.jpeg_set_colorspace_8_win(ref cinfo, cinfo.in_color_space);
                            else
                                NativeMethods.jpeg_set_colorspace_8(ref cinfo, cinfo.in_color_space);
                        }
                        else if (Bits <= 12 && Bits > 8)
                        {
                            if (Platform.IsWindows)
                                NativeMethods.jpeg_set_colorspace_12_win(ref cinfo, cinfo.in_color_space);
                            else
                                NativeMethods.jpeg_set_colorspace_12(ref cinfo, cinfo.in_color_space);
                        }
                        else if (Bits <= 16 && Bits > 12)
                        {
                            if (Platform.IsWindows)
                                NativeMethods.jpeg_set_colorspace_16_win(ref cinfo, cinfo.in_color_space);
                            else
                                NativeMethods.jpeg_set_colorspace_16(ref cinfo, cinfo.in_color_space);
                        }

                        cinfo.comp_info->h_samp_factor = 1;
                        cinfo.comp_info->v_samp_factor = 1;
                    }
                    else
                    {
                        if (cinfo.jpeg_color_space == J_COLOR_SPACE.JCS_YCbCr && jpegParams.SampleFactor != DicomJpegSampleFactor.Unknown)
                        {
                            switch (jpegParams.SampleFactor)
                            {
                                case DicomJpegSampleFactor.SF444:
                                    cinfo.comp_info->h_samp_factor = 1;
                                    cinfo.comp_info->v_samp_factor = 1;
                                    break;
                                case DicomJpegSampleFactor.SF422:
                                    cinfo.comp_info->h_samp_factor = 2;
                                    cinfo.comp_info->v_samp_factor = 1;
                                    break;
                            }
                        }
                        else
                        {
                            if (jpegParams.SampleFactor == DicomJpegSampleFactor.Unknown)
                            {
                                //jpeg_set_colorspace 8, 12 and 16 bit for Linux, Windows and Osx for 64 bits
                                if (Bits.Equals(8))
                                {
                                    if (Platform.IsWindows)
                                        NativeMethods.jpeg_set_colorspace_8_win(ref cinfo, cinfo.in_color_space);
                                    else
                                        NativeMethods.jpeg_set_colorspace_8(ref cinfo, cinfo.in_color_space);
                                }
                                else if (Bits <= 12 && Bits > 8)
                                {
                                    if (Platform.IsWindows)
                                        NativeMethods.jpeg_set_colorspace_12_win(ref cinfo, cinfo.in_color_space);
                                    else
                                        NativeMethods.jpeg_set_colorspace_12(ref cinfo, cinfo.in_color_space);
                                }
                                else if (Bits <= 16 && Bits > 12)
                                {
                                    if (Platform.IsWindows)
                                        NativeMethods.jpeg_set_colorspace_16_win(ref cinfo, cinfo.in_color_space);
                                    else
                                        NativeMethods.jpeg_set_colorspace_16(ref cinfo, cinfo.in_color_space);
                                }
                            }

                            cinfo.comp_info[0].h_samp_factor = 1;
                            cinfo.comp_info[0].v_samp_factor = 1;
                        }
                    }

                    for (int sfi = 1; sfi < 10; sfi++)
                    {
                        cinfo.comp_info[sfi].h_samp_factor = 1;
                        cinfo.comp_info[sfi].v_samp_factor = 1;
                    }

                    //jpeg_start_compress 8, 12 and 16 bit for Linux, Windows and Osx for 64 bits
                    if (Bits.Equals(8))
                    {
                        if (Platform.IsWindows)
                            NativeMethods.jpeg_start_compress_8_win(ref cinfo, Convert.ToInt32(true));
                        else
                            NativeMethods.jpeg_start_compress_8(ref cinfo, Convert.ToInt32(true));
                    }
                    else if (Bits <= 12 && Bits > 8)
                    {
                        if (Platform.IsWindows)
                            NativeMethods.jpeg_start_compress_12_win(ref cinfo, Convert.ToInt32(true));
                        else
                            NativeMethods.jpeg_start_compress_12(ref cinfo, Convert.ToInt32(true));
                    }
                    else if (Bits <= 16 && Bits > 12)
                    {
                        if (Platform.IsWindows)
                            NativeMethods.jpeg_start_compress_16_win(ref cinfo, Convert.ToInt32(true));
                        else
                            NativeMethods.jpeg_start_compress_16(ref cinfo, Convert.ToInt32(true));
                    }

                    byte* row_pointer = null;
                    int row_stride = metadata.Width * metadata.SamplesPerPixel * (metadata.BitsStored <= 8 ? 1 : metadata.BytesAllocated);

                    byte* framePtr = (byte*)(void*)frameArray.Pointer;

                    while (cinfo.next_scanline < cinfo.image_height)
                    {
                        row_pointer = &framePtr[cinfo.next_scanline * row_stride];

                        //jpeg_write_scanlines 8, 12 and 16 bit for Linux, Windows and Osx for 64 bits
                        if (Bits.Equals(8))
                        {
                            if (Platform.IsWindows)
                                NativeMethods.jpeg_write_scanlines_8_win(ref cinfo, &row_pointer, 1);
                            else
                                NativeMethods.jpeg_write_scanlines_8(ref cinfo, &row_pointer, 1);
                        }
                        else if (Bits <= 12 && Bits > 8)
                        {
                            if (Platform.IsWindows)
                                NativeMethods.jpeg_write_scanlines_12_win(ref cinfo, (short**)(&row_pointer), 1);
                            else
                                NativeMethods.jpeg_write_scanlines_12(ref cinfo, (short**)(&row_pointer), 1);
                        }
                        else if (Bits <= 16 && Bits > 12)
                        {
                            if (Platform.IsWindows)
                                NativeMethods.jpeg_write_scanlines_16_win(ref cinfo, (ushort**)(&row_pointer), 1);
                            else
                                NativeMethods.jpeg_write_scanlines_16(ref cinfo, (ushort**)(&row_pointer), 1);
                        }
                    }

                    //jpeg_finish_compress and jpeg_destroy_compress 8, 12 and 16 bit for Linux, Windows and Osx for 64 bits
                    if (Bits.Equals(8))
                    {
                        if (Platform.IsWindows)
                        {
                            NativeMethods.jpeg_finish_compress_8_win(ref cinfo);
                            NativeMethods.jpeg_destroy_compress_8_win(ref cinfo);
                        }
                        else
                        {
                            NativeMethods.jpeg_finish_compress_8(ref cinfo);
                            NativeMethods.jpeg_destroy_compress_8(ref cinfo);
                        }
                    }
                    else if (Bits <= 12 && Bits > 8)
                    {
                        if (Platform.IsWindows)
                        {
                            NativeMethods.jpeg_finish_compress_12_win(ref cinfo);
                            NativeMethods.jpeg_destroy_compress_12_win(ref cinfo);
                        }
                        else
                        {
                            NativeMethods.jpeg_finish_compress_12(ref cinfo);
                            NativeMethods.jpeg_destroy_compress_12(ref cinfo);
                        }
                    }
                    else if (Bits <= 16 && Bits > 12)
                    {
                        if (Platform.IsWindows)
                        {
                            NativeMethods.jpeg_finish_compress_16_win(ref cinfo);
                            NativeMethods.jpeg_destroy_compress_16_win(ref cinfo);
                        }
                        else
                        {
                            NativeMethods.jpeg_finish_compress_16(ref cinfo);
                            NativeMethods.jpeg_destroy_compress_16(ref cinfo);
                        }
                    }

                    // TODO: Introduce output metadata
                    /*if (metadata.PhotometricInterpretation == PhotometricInterpretation.Rgb
                        && cinfo.jpeg_color_space == J_COLOR_SPACE.JCS_YCbCr)
                    {
                        newPixelData.PhotometricInterpretation = PhotometricInterpretation.YbrFull422;
                    }

                    if (metadata.PhotometricInterpretation == PhotometricInterpretation.YbrFull422)
                    {
                        newPixelData.PhotometricInterpretation = PhotometricInterpretation.Rgb;
                    }*/

                    GC.KeepAlive(errorexit_);
                    GC.KeepAlive(ouput_Message_);
                    GC.KeepAlive(init_Destination_);
                    GC.KeepAlive(empty_Output_Buffer_);
                    GC.KeepAlive(term_Destination_);

                    return MemoryBuffer.ToArray(); // Think about event size
                }
            }
            catch (Exception e)
            {
                throw new CodecException(e.Message, e);
            }
            finally
            {
                if (MemoryBuffer != null)
                {
                    MemoryBuffer.Dispose();
                    MemoryBuffer = null;
                }

                if (frameArray != null)
                {
                    frameArray.Dispose();
                    frameArray = null;
                }
            }
        }

        public override void Decode(byte[] input, byte[] output, FrameMetadata metadata, out FrameMetadata outMetadata, CodecParams codecParams = null)
        {
            JpegCodecParams jpegParams = codecParams as JpegCodecParams ?? DefaultParams;
            outMetadata = (FrameMetadata)metadata.Clone();

            unsafe
            {
                PinnedByteArray jpegArray = new PinnedByteArray(this.TrytoFixPixelData(input));
                PinnedByteArray frameArray = null;

                try
                {
                    j_decompress_ptr dinfo = new j_decompress_ptr();
                    SourceManagerStruct src = new SourceManagerStruct();

                    Init_source init_Source_ = initSource;
                    src.pub.init_source = Marshal.GetFunctionPointerForDelegate(init_Source_);

                    Fill_input_buffer fill_input_buffer_ = fillInputBuffer;
                    src.pub.fill_input_buffer = Marshal.GetFunctionPointerForDelegate(fill_input_buffer_);

                    Skip_input_data skip_input_data_ = skipInputData;
                    src.pub.skip_input_data = Marshal.GetFunctionPointerForDelegate(skip_input_data_);

                    //jpeg_resync_to_restart 8, 12 and 16 bit for Linux, Windows and Osx for 64 bits
                    Resync_to_restart resync_to_restart_;

                    if (Bits.Equals(8))
                    {
                        if (Platform.IsWindows)
                            resync_to_restart_ = NativeMethods.jpeg_resync_to_restart_8_win;
                        else
                            resync_to_restart_ = NativeMethods.jpeg_resync_to_restart_8;

                        src.pub.resync_to_restart = Marshal.GetFunctionPointerForDelegate(resync_to_restart_);
                    }
                    else if (Bits > 8 && Bits <= 12)
                    {
                        if (Platform.IsWindows)
                            resync_to_restart_ = NativeMethods.jpeg_resync_to_restart_12_win;
                        else
                            resync_to_restart_ = NativeMethods.jpeg_resync_to_restart_12;

                        src.pub.resync_to_restart = Marshal.GetFunctionPointerForDelegate(resync_to_restart_);
                    }
                    else if (Bits > 12 && Bits <= 16)
                    {
                        if (Platform.IsWindows)
                            resync_to_restart_ = NativeMethods.jpeg_resync_to_restart_16_win;
                        else
                            resync_to_restart_ = NativeMethods.jpeg_resync_to_restart_16;

                        src.pub.resync_to_restart = Marshal.GetFunctionPointerForDelegate(resync_to_restart_);
                    }

                    src.pub.term_source = IntPtr.Zero;
                    src.pub.bytes_in_buffer = 0;
                    src.pub.next_input_byte = IntPtr.Zero;
                    src.skip_bytes = 0;

                    src.next_buffer = jpegArray.Pointer;
                    src.next_buffer_size = (uint)jpegArray.ByteSize;

                    jpeg_error_mgr jerr = new jpeg_error_mgr();

                    //jpeg_std_error 8, 12 and 16 bit for Linux, Windows and Osx for 64 bits
                    if (Bits.Equals(8))
                    {
                        if (Platform.IsWindows)
                            dinfo.err = NativeMethods.jpeg_std_error_8_win(ref jerr);
                        else
                            dinfo.err = NativeMethods.jpeg_std_error_8(ref jerr);
                    }
                    else if (Bits > 8 && Bits <= 12)
                    {
                        if (Platform.IsWindows)
                            dinfo.err = NativeMethods.jpeg_std_error_12_win(ref jerr);
                        else
                            dinfo.err = NativeMethods.jpeg_std_error_12(ref jerr);
                    }
                    else if (Bits > 12 && Bits <= 16)
                    {
                        if (Platform.IsWindows)
                            dinfo.err = NativeMethods.jpeg_std_error_16_win(ref jerr);
                        else
                            dinfo.err = NativeMethods.jpeg_std_error_16(ref jerr);
                    }

                    ouput_Message errorexit_ = null;
                    ouput_Message ouput_Message_ = null;

                    errorexit_ = OutputMessage;
                    jerr.error_exit = Marshal.GetFunctionPointerForDelegate(errorexit_);

                    ouput_Message_ = OutputMessage;
                    jerr.output_message = Marshal.GetFunctionPointerForDelegate(ouput_Message_);

                    //jpeg_create_decompress 8, 12 and 16 bit for Linux, Windows and Osx for 64 bits
                    if (Bits.Equals(8))
                    {
                        if (Platform.IsWindows)
                            NativeMethods.jpeg_create_decompress_8_win(ref dinfo);
                        else
                            NativeMethods.jpeg_create_decompress_8(ref dinfo);
                    }
                    else if (Bits > 8 && Bits <= 12)
                    {
                        if (Platform.IsWindows)
                            NativeMethods.jpeg_create_decompress_12_win(ref dinfo);
                        else
                            NativeMethods.jpeg_create_decompress_12(ref dinfo);
                    }
                    else if (Bits > 12 && Bits <= 16)
                    {
                        if (Platform.IsWindows)
                            NativeMethods.jpeg_create_decompress_16_win(ref dinfo);
                        else
                            NativeMethods.jpeg_create_decompress_16(ref dinfo);
                    }

                    dinfo.src = (jpeg_source_mgr*)&src.pub;

                    //jpeg_read_header 8, 12 and 16 bit for Linux, Windows and Osx for 64 bits
                    if (Bits.Equals(8))
                    {
                        int jpeg_read_header_value = 0;

                        try
                        {
                            if (Platform.IsWindows)
                                jpeg_read_header_value = NativeMethods.jpeg_read_header_8_win(ref dinfo, 1);
                            else
                                jpeg_read_header_value = NativeMethods.jpeg_read_header_8(ref dinfo, 1);
                        }
                        catch
                        {
                            throw new CodecException("Unable to read header value : Suspended");
                        }

                        if (jpeg_read_header_value == 0)
                        {
                            throw new CodecException("Unable to decompress JPEG: Suspended");
                        }
                    }
                    else if (Bits > 8 && Bits <= 12)
                    {
                        int jpeg_read_header_value = 0;

                        try
                        {
                            if (Platform.IsWindows)
                                jpeg_read_header_value = NativeMethods.jpeg_read_header_12_win(ref dinfo, 1);
                            else
                                jpeg_read_header_value = NativeMethods.jpeg_read_header_12(ref dinfo, 1);
                        }
                        catch
                        {
                            throw new CodecException("Unable to read header value : Suspended");
                        }

                        if (jpeg_read_header_value == 0)
                        {
                            throw new CodecException("Unable to decompress JPEG: Suspended");
                        }
                    }
                    else if (Bits > 12 && Bits <= 16)
                    {
                        int jpeg_read_header_value = 0;

                        try
                        {
                            if (Platform.IsWindows)
                                jpeg_read_header_value = NativeMethods.jpeg_read_header_16_win(ref dinfo, 1);
                            else
                                jpeg_read_header_value = NativeMethods.jpeg_read_header_16(ref dinfo, 1);
                        }
                        catch
                        {
                            throw new CodecException("Unable to read header value : Suspended");
                        }

                        if (jpeg_read_header_value == 0)
                        {
                            throw new CodecException("Unable to decompress JPEG: Suspended");
                        }
                    }

                    if (metadata.PhotometricInterpretation != PhotometricInterpretation.Rgb)
                    {
                        jpegParams.ConvertColorSpaceToRGB = true;
                    }

                    outMetadata.PhotometricInterpretation = metadata.PhotometricInterpretation;

                    if (jpegParams.ConvertColorSpaceToRGB && (dinfo.out_color_space == J_COLOR_SPACE.JCS_YCbCr || dinfo.out_color_space == J_COLOR_SPACE.JCS_RGB))
                    {
                        if (metadata.IsSigned)
                            throw new CodecException("JPEG codec unable to perform colorspace conversion on signed pixel data");

                        dinfo.out_color_space = J_COLOR_SPACE.JCS_RGB;
                        outMetadata.PhotometricInterpretation = PhotometricInterpretation.Rgb;
                        outMetadata.PlanarConfiguration = PlanarConfiguration.Interleaved;
                    }
                    else
                    {
                        dinfo.jpeg_color_space = J_COLOR_SPACE.JCS_UNKNOWN;
                        dinfo.out_color_space = J_COLOR_SPACE.JCS_UNKNOWN;
                    }

                    //jpeg_calc_output_dimensions 8, 12 and 16 bit and jpeg_start_decompress_8 for Linux, Windows and Osx for 64 bits
                    if (Bits.Equals(8))
                    {
                        if (Platform.IsWindows)
                        {
                            NativeMethods.jpeg_calc_output_dimensions_8_win(ref dinfo);
                            NativeMethods.jpeg_start_decompress_8_win(ref dinfo);
                        }
                        else
                        {
                            NativeMethods.jpeg_calc_output_dimensions_8(ref dinfo);
                            NativeMethods.jpeg_start_decompress_8(ref dinfo);
                        }
                    }
                    else if (Bits > 8 && Bits <= 12)
                    {
                        if (Platform.IsWindows)
                        {
                            NativeMethods.jpeg_calc_output_dimensions_12_win(ref dinfo);
                            NativeMethods.jpeg_start_decompress_12_win(ref dinfo);
                        }
                        else
                        {
                            NativeMethods.jpeg_calc_output_dimensions_12(ref dinfo);
                            NativeMethods.jpeg_start_decompress_12(ref dinfo);
                        }
                    }
                    else if (Bits > 12 && Bits <= 16)
                    {
                        if (Platform.IsWindows)
                        {
                            NativeMethods.jpeg_calc_output_dimensions_16_win(ref dinfo);
                            NativeMethods.jpeg_start_decompress_16_win(ref dinfo);
                        }
                        else
                        {
                            NativeMethods.jpeg_calc_output_dimensions_16(ref dinfo);
                            NativeMethods.jpeg_start_decompress_16(ref dinfo);
                        }
                    }

                    uint rowSize = 0;
                    rowSize = Convert.ToUInt32(dinfo.output_width * dinfo.output_components * metadata.BytesAllocated);

                    int frameSize = Convert.ToInt32(rowSize * dinfo.output_height);

                    // TODO: Check if this doesn't create a bug for odd length JPEG images
                    /*if ((frameSize % 2) != 0 && oldPixelData.NumberOfFrames == 1)
                        frameSize++;*/

                    frameArray = new PinnedByteArray(frameSize);
                    byte* framePtr = (byte*)(void*)frameArray.Pointer;

                    if (Bits.Equals(8))
                    {
                        while (dinfo.output_scanline < dinfo.output_height)
                        {
                            uint rows = 0;

                            if (Platform.IsWindows)
                                rows = NativeMethods.jpeg_read_scanlines_8_win(ref dinfo, (byte**)&framePtr, 1);
                            else
                                rows = NativeMethods.jpeg_read_scanlines_8(ref dinfo, (byte**)&framePtr, 1);

                            if (rows == 0)
                            {
                                throw new CodecException("JPEG 8 bit codec unable to perform reading scanlines on pixel data");
                            }

                            framePtr += rows * rowSize;
                        }
                    }
                    else if (Bits > 8 && Bits <= 12)
                    {
                        while (dinfo.output_scanline < dinfo.output_height)
                        {
                            uint rows = 0;

                            if (Platform.IsWindows)
                                rows = NativeMethods.jpeg_read_scanlines_12_win(ref dinfo, (short**)&framePtr, 1);
                            else
                                rows = NativeMethods.jpeg_read_scanlines_12(ref dinfo, (short**)&framePtr, 1);

                            if (rows == 0)
                            {
                                throw new CodecException("JPEG 12 bit codec unable to perform reading scanlines pixel data");
                            }

                            framePtr += rows * rowSize;
                        }
                    }
                    else if (Bits > 12 && Bits <= 16)
                    {
                        while (dinfo.output_scanline < dinfo.output_height)
                        {
                            uint rows = 0;

                            if (Platform.IsWindows)
                                rows = NativeMethods.jpeg_read_scanlines_16_win(ref dinfo, (ushort**)&framePtr, 1);
                            else
                                rows = NativeMethods.jpeg_read_scanlines_16(ref dinfo, (ushort**)&framePtr, 1);

                            if (rows == 0)
                            {
                                throw new CodecException("JPEG 16 bit codec unable to perform reading scanlines on pixel data");
                            }

                            framePtr += rows * rowSize;
                        }
                    }

                    //jpeg_destroy_decompress 8, 12 and 16 bit for Linux, Windows and Osx for 64 bits
                    if (Bits.Equals(8))
                    {
                        if (Platform.IsWindows)
                            NativeMethods.jpeg_destroy_decompress_8_win(ref dinfo);
                        else
                            NativeMethods.jpeg_destroy_decompress_8(ref dinfo);
                    }
                    else if (Bits > 8 && Bits <= 12)
                    {
                        if (Platform.IsWindows)
                            NativeMethods.jpeg_destroy_decompress_12_win(ref dinfo);
                        else
                            NativeMethods.jpeg_destroy_decompress_12(ref dinfo);
                    }
                    else if (Bits > 12 && Bits <= 16)
                    {
                        if (Platform.IsWindows)
                            NativeMethods.jpeg_destroy_decompress_16_win(ref dinfo);
                        else
                            NativeMethods.jpeg_destroy_decompress_16(ref dinfo);
                    }

                    byte[] buffer = frameArray.Data;

                    if (outMetadata.PlanarConfiguration == PlanarConfiguration.Planar && outMetadata.SamplesPerPixel > 1)
                    {
                        if (metadata.SamplesPerPixel != 3 || metadata.BitsStored > 8)
                            throw new CodecException("Planar reconfiguration only implemented for SamplesPerPixel = 3 && BitsStored <= 8");

                        buffer = PixelDataConverter.InterleavedToPlanar24(buffer);
                    }

                    buffer.CopyTo(output.AsSpan());

                    GC.KeepAlive(init_Source_);
                    GC.KeepAlive(fill_input_buffer_);
                    GC.KeepAlive(skip_input_data_);
                    GC.KeepAlive(errorexit_);
                    GC.KeepAlive(ouput_Message_);
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
                    if (jpegArray != null)
                    {
                        jpegArray.Dispose();
                        jpegArray = null;
                    }
                }
            }
        }

        protected override unsafe int ScanHeaderForPrecision(byte[] frame, FrameMetadata metadata, bool isjPEG)
        {
            PinnedByteArray jpegArray = new PinnedByteArray(frame);
            j_decompress_ptr dinfo = new j_decompress_ptr();

            var jpegFile = new byte[] { 255, 216, 255 };

            if (!jpegFile.SequenceEqual(jpegArray.Data.Take(jpegFile.Length)))
            {
                throw new CodecException("Not a JPEG file.");
            }

            isjPEG = true;

            SourceManagerStruct src = new SourceManagerStruct();

            Init_source init_Source_ = initSource;
            src.pub.init_source = Marshal.GetFunctionPointerForDelegate(init_Source_);

            Fill_input_buffer fillInputBuffer_ = fillInputBuffer;
            src.pub.fill_input_buffer = Marshal.GetFunctionPointerForDelegate(fillInputBuffer_);

            Skip_input_data skip_input_data_ = skipInputData;
            src.pub.skip_input_data = Marshal.GetFunctionPointerForDelegate(skip_input_data_);

            //jpeg_resync_to_restart 8, 12 and 16 bit for Linux, Windows and Osx for 64 bits
            Resync_to_restart resync_to_restart_ = null;

            if (Bits.Equals(8))
            {
                if (Platform.Current.Equals(Platform.Type.win_x64) || Platform.Current.Equals(Platform.Type.win_arm64))
                    resync_to_restart_ = NativeMethods.jpeg_resync_to_restart_8_win;
                else
                    resync_to_restart_ = NativeMethods.jpeg_resync_to_restart_8;
            }
            else if (Bits > 8 && Bits <= 12)
            {
                if (Platform.Current.Equals(Platform.Type.win_x64) || Platform.Current.Equals(Platform.Type.win_arm64))
                    resync_to_restart_ = NativeMethods.jpeg_resync_to_restart_12_win;
                else
                    resync_to_restart_ = NativeMethods.jpeg_resync_to_restart_12;
            }
            else if (Bits > 12 && Bits <= 16)
            {
                if (Platform.Current.Equals(Platform.Type.win_x64) || Platform.Current.Equals(Platform.Type.win_arm64))
                    resync_to_restart_ = NativeMethods.jpeg_resync_to_restart_16_win;
                else
                    resync_to_restart_ = NativeMethods.jpeg_resync_to_restart_16;
            }

            src.pub.resync_to_restart = Marshal.GetFunctionPointerForDelegate(resync_to_restart_);
            src.pub.term_source = IntPtr.Zero;
            src.pub.bytes_in_buffer = 0;
            src.pub.next_input_byte = IntPtr.Zero;
            src.skip_bytes = 0;
            src.next_buffer = jpegArray.Pointer;
            src.next_buffer_size = (uint)jpegArray.ByteSize;

            jpeg_error_mgr jerr = new jpeg_error_mgr();

            //jpeg_std_error 8, 12 and 16 bit for Linux, Windows and Osx for 64 bits
            if (Bits.Equals(8))
            {
                if (Platform.Current.Equals(Platform.Type.win_x64) || Platform.Current.Equals(Platform.Type.win_arm64))
                    dinfo.err = NativeMethods.jpeg_std_error_8_win(ref jerr);
                else
                    dinfo.err = NativeMethods.jpeg_std_error_8(ref jerr);
            }
            else if (Bits > 8 && Bits <= 12)
            {
                if (Platform.Current.Equals(Platform.Type.win_x64) || Platform.Current.Equals(Platform.Type.win_arm64))
                    dinfo.err = NativeMethods.jpeg_std_error_12_win(ref jerr);
                else
                    dinfo.err = NativeMethods.jpeg_std_error_12(ref jerr);
            }
            else if (Bits > 12 && Bits <= 16)
            {
                if (Platform.Current.Equals(Platform.Type.win_x64) || Platform.Current.Equals(Platform.Type.win_arm64))
                    dinfo.err = NativeMethods.jpeg_std_error_16_win(ref jerr);
                else
                    dinfo.err = NativeMethods.jpeg_std_error_16(ref jerr);
            }

            ouput_Message errorexit_ = null;
            ouput_Message ouput_Message_ = null;

            errorexit_ = OutputMessage;
            jerr.error_exit = Marshal.GetFunctionPointerForDelegate(errorexit_);

            ouput_Message_ = OutputMessage;
            jerr.output_message = Marshal.GetFunctionPointerForDelegate(ouput_Message_);

            //jpeg_create_decompress 8, 12 and 16 bit for Linux, Windows and Osx for 64 bits
            if (Bits.Equals(8))
            {
                if (Platform.Current.Equals(Platform.Type.win_x64) || Platform.Current.Equals(Platform.Type.win_arm64))
                    NativeMethods.jpeg_create_decompress_8_win(ref dinfo);
                else
                    NativeMethods.jpeg_create_decompress_8(ref dinfo);
            }
            else if (Bits > 8 && Bits <= 12)
            {
                if (Platform.Current.Equals(Platform.Type.win_x64) || Platform.Current.Equals(Platform.Type.win_arm64))
                    NativeMethods.jpeg_create_decompress_12_win(ref dinfo);
                else
                    NativeMethods.jpeg_create_decompress_12(ref dinfo);
            }
            else if (Bits > 12 && Bits <= 16)
            {
                if (Platform.Current.Equals(Platform.Type.win_x64) || Platform.Current.Equals(Platform.Type.win_arm64))
                    NativeMethods.jpeg_create_decompress_16_win(ref dinfo);
                else
                    NativeMethods.jpeg_create_decompress_16(ref dinfo);
            }

            dinfo.src = &src.pub;

            //jpeg_read_header 8, 12 and 16 bit for Linux, Windows and Osx for 64 bits
            if (Bits.Equals(8))
            {
                if (Platform.Current.Equals(Platform.Type.win_x64) || Platform.Current.Equals(Platform.Type.win_arm64))
                {
                    if (NativeMethods.jpeg_read_header_8_win(ref dinfo, 1) == 0)
                    {
                        NativeMethods.jpeg_destroy_decompress_8_win(ref dinfo);
                        throw new CodecException("Unable to read JPEG header: Suspended");
                    }

                    NativeMethods.jpeg_destroy_decompress_8_win(ref dinfo);
                }
                else
                {
                    if (NativeMethods.jpeg_read_header_8(ref dinfo, 1) == 0)
                    {
                        NativeMethods.jpeg_destroy_decompress_8(ref dinfo);
                        throw new CodecException("Unable to read JPEG header: Suspended");
                    }

                    NativeMethods.jpeg_destroy_decompress_8(ref dinfo);
                }
            }
            else if (Bits > 8 && Bits <= 12)
            {
                if (Platform.Current.Equals(Platform.Type.win_x64) || Platform.Current.Equals(Platform.Type.win_arm64))
                {
                    if (NativeMethods.jpeg_read_header_12_win(ref dinfo, 1) == 0)
                    {
                        NativeMethods.jpeg_destroy_decompress_12_win(ref dinfo);
                        throw new CodecException("Unable to read JPEG header: Suspended");
                    }

                    NativeMethods.jpeg_destroy_decompress_12_win(ref dinfo);
                }
                else
                {
                    if (NativeMethods.jpeg_read_header_12(ref dinfo, 1) == 0)
                    {
                        NativeMethods.jpeg_destroy_decompress_12(ref dinfo);
                        throw new CodecException("Unable to read JPEG header: Suspended");
                    }

                    NativeMethods.jpeg_destroy_decompress_12(ref dinfo);
                }
            }
            else if (Bits > 12 && Bits <= 16)
            {
                if (Platform.Current.Equals(Platform.Type.win_x64) || Platform.Current.Equals(Platform.Type.win_arm64))
                {
                    if (NativeMethods.jpeg_read_header_16_win(ref dinfo, 1) == 0)
                    {
                        NativeMethods.jpeg_destroy_decompress_16_win(ref dinfo);
                        throw new CodecException("Unable to read JPEG header: Suspended");
                    }

                    NativeMethods.jpeg_destroy_decompress_16_win(ref dinfo);
                }
                else
                {
                    if (NativeMethods.jpeg_read_header_16(ref dinfo, 1) == 0)
                    {
                        NativeMethods.jpeg_destroy_decompress_16(ref dinfo);
                        throw new CodecException("Unable to read JPEG header: Suspended");
                    }

                    NativeMethods.jpeg_destroy_decompress_16(ref dinfo);
                }
            }

            GC.KeepAlive(init_Source_);
            GC.KeepAlive(skip_input_data_);
            GC.KeepAlive(errorexit_);
            GC.KeepAlive(ouput_Message_);

            return dinfo.data_precision;
        }

        private byte[] TrytoFixPixelData(byte[] buffer)
        {
            if (!buffer[buffer.Length - 1].Equals(217) && !buffer[buffer.Length - 2].Equals(255))
            {
                var newbf = new byte[buffer.Length + 2];
                buffer.CopyTo(newbf, 0);
                newbf[buffer.Length] = 255;
                newbf[buffer.Length + 1] = 217;

                return newbf;
            }
            else
            {
                return buffer;
            }
        }

        protected static unsafe void initSource(j_decompress_ptr* dinfo)
        {
        }

        protected static unsafe void initDestination(j_compress_ptr* cinfo)
        {
            JpegCodec thisPtr = This as JpegCodec;
            thisPtr.MemoryBuffer = new MemoryStream();
            thisPtr.DataArray = new PinnedByteArray(16384);
            cinfo->dest->next_output_byte = thisPtr.DataArray.Pointer;
            cinfo->dest->free_in_buffer = 16384;
        }

        protected static unsafe int emptyOutputBuffer(j_compress_ptr* cinfo)
        {
            JpegCodec thisPtr = This;
            thisPtr.MemoryBuffer.Write(thisPtr.DataArray.Data, 0, 16384);
            cinfo->dest->next_output_byte = thisPtr.DataArray.Pointer;
            cinfo->dest->free_in_buffer = 16384;
            return 1;
        }

        protected static unsafe void termDestination(j_compress_ptr* cinfo)
        {
            JpegCodec thisPtr = This;
            int count = 16384 - (int)cinfo->dest->free_in_buffer;
            thisPtr.MemoryBuffer.Write(thisPtr.DataArray.Data, 0, count);
            thisPtr.DataArray = null;
        }

        protected static unsafe void OutputMessage(j_common_ptr* cinfo)
        {
            jpeg_error_mgr* myerr = (jpeg_error_mgr*)cinfo->err;
            char[] buffer = new char[200];

            if (Platform.Current.Equals(Platform.Type.win_x64) || Platform.Current.Equals(Platform.Type.win_arm64))
                NativeMethods.format_message_win(cinfo, buffer);
            else
                NativeMethods.format_message(cinfo, buffer);
        }

        protected static unsafe int fillInputBuffer(j_decompress_ptr* dinfo)
        {
            SourceManagerStruct* src = (SourceManagerStruct*)(dinfo->src);
            if (src->next_buffer != null)
            {
                src->pub.next_input_byte = src->next_buffer;
                src->pub.bytes_in_buffer = src->next_buffer_size;
                src->next_buffer = IntPtr.Zero;
                src->next_buffer_size = 0;

                if (src->skip_bytes > 0)
                {
                    if (src->pub.bytes_in_buffer < (uint)src->skip_bytes)
                    {
                        src->skip_bytes -= Convert.ToInt32(src->pub.bytes_in_buffer);
                        byte* p = (byte*)src->pub.next_input_byte.ToPointer();

                        p += src->pub.bytes_in_buffer;

                        src->pub.bytes_in_buffer = 0;
                        // cause a suspension return
                        return 0;
                    }
                    else
                    {
                        src->pub.bytes_in_buffer -= (uint)src->skip_bytes;
                        src->pub.next_input_byte += src->skip_bytes;
                        src->skip_bytes = 0;
                    }
                }
                return 1;
            }
            return 0;
        }

        protected static unsafe void skipInputData(j_decompress_ptr* dinfo, int num_bytes)
        {
            SourceManagerStruct* src = (SourceManagerStruct*)(dinfo->src);
            if (src->pub.bytes_in_buffer < (uint)num_bytes)
            {
                src->skip_bytes = num_bytes - Convert.ToInt32(src->pub.bytes_in_buffer);
                byte* p = (byte*)src->pub.next_input_byte.ToPointer();
                p += src->pub.bytes_in_buffer;

                src->pub.bytes_in_buffer = 0; // causes a suspension return
            }
            else
            {
                src->pub.bytes_in_buffer -= (uint)num_bytes;
                src->pub.next_input_byte += num_bytes;
                src->skip_bytes = 0;
            }
        }

        protected unsafe static void jpeg_simple_spectral_selection(ref j_compress_ptr cinfo)
        {
            int ncomps = cinfo.num_components;
            jpeg_scan_info* scanptr = null;
            int nscans = 0;

            if (ncomps == 3 && cinfo.jpeg_color_space == J_COLOR_SPACE.JCS_YCbCr) nscans = 7;
            else nscans = 1 + 2 * ncomps;   /* 1 DC scan; 2 AC scans per component */

            if (cinfo.script_space == null || cinfo.script_space_size < nscans)
            {
                cinfo.script_space_size = nscans > 7 ? nscans : 7;
            }

            scanptr = cinfo.script_space;
            cinfo.scan_info = scanptr;
            cinfo.num_scans = nscans;

            if (ncomps == 3 && cinfo.jpeg_color_space == J_COLOR_SPACE.JCS_YCbCr)
            {
                // Custom script for YCbCr color images
                // Interleaved DC scan for Y,Cb,Cr:
                scanptr[0].component_index[0] = 0;
                scanptr[0].component_index[1] = 1;
                scanptr[0].component_index[2] = 2;
                scanptr[0].comps_in_scan = 3;
                scanptr[0].Ss = 0;
                scanptr[0].Se = 0;
                scanptr[0].Ah = 0;
                scanptr[0].Al = 0;

                // AC scans
                // First two Y AC coefficients
                scanptr[1].component_index[0] = 0;
                scanptr[1].comps_in_scan = 1;
                scanptr[1].Ss = 1;
                scanptr[1].Se = 2;
                scanptr[1].Ah = 0;
                scanptr[1].Al = 0;

                // Three more
                scanptr[2].component_index[0] = 0;
                scanptr[2].comps_in_scan = 1;
                scanptr[2].Ss = 3;
                scanptr[2].Se = 5;
                scanptr[2].Ah = 0;
                scanptr[2].Al = 0;

                // All AC coefficients for Cb
                scanptr[3].component_index[0] = 1;
                scanptr[3].comps_in_scan = 1;
                scanptr[3].Ss = 1;
                scanptr[3].Se = 63;
                scanptr[3].Ah = 0;
                scanptr[3].Al = 0;

                // All AC coefficients for Cr
                scanptr[4].component_index[0] = 2;
                scanptr[4].comps_in_scan = 1;
                scanptr[4].Ss = 1;
                scanptr[4].Se = 63;
                scanptr[4].Ah = 0;
                scanptr[4].Al = 0;

                // More Y coefficients
                scanptr[5].component_index[0] = 0;
                scanptr[5].comps_in_scan = 1;
                scanptr[5].Ss = 6;
                scanptr[5].Se = 9;
                scanptr[5].Ah = 0;
                scanptr[5].Al = 0;

                // Remaining Y coefficients
                scanptr[6].component_index[0] = 0;
                scanptr[6].comps_in_scan = 1;
                scanptr[6].Ss = 10;
                scanptr[6].Se = 63;
                scanptr[6].Ah = 0;
                scanptr[6].Al = 0;
            }
            else
            {
                /* All-purpose script for other color spaces. */
                int j = 0;

                // Interleaved DC scan for all components
                for (j = 0; j < ncomps; j++) scanptr[0].component_index[j] = j;
                scanptr[0].comps_in_scan = ncomps;
                scanptr[0].Ss = 0;
                scanptr[0].Se = 0;
                scanptr[0].Ah = 0;
                scanptr[0].Al = 0;

                // first AC scan for each component
                for (j = 0; j < ncomps; j++)
                {
                    scanptr[j + 1].component_index[0] = j;
                    scanptr[j + 1].comps_in_scan = 1;
                    scanptr[j + 1].Ss = 1;
                    scanptr[j + 1].Se = 5;
                    scanptr[j + 1].Ah = 0;
                    scanptr[j + 1].Al = 0;
                }

                // second AC scan for each component
                for (j = 0; j < ncomps; j++)
                {
                    scanptr[j + ncomps + 1].component_index[0] = j;
                    scanptr[j + ncomps + 1].comps_in_scan = 1;
                    scanptr[j + ncomps + 1].Ss = 6;
                    scanptr[j + ncomps + 1].Se = 63;
                    scanptr[j + ncomps + 1].Ah = 0;
                    scanptr[j + ncomps + 1].Al = 0;
                }
            }
        }
    }
}
