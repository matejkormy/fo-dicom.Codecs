
namespace Dicom.Codecs
{
    using Dicom.Codecs.Common;
    using Dicom.Codecs.HTJ2K;
    using Dicom.Codecs.Jpeg;
    using Dicom.Codecs.Jpeg2K;
    using Dicom.Codecs.JpegLs;
    using Dicom.Codecs.Rle;

    public static class Codecs
    {
        public static ICodec GetCodec(TransferSyntax inputSyntax, TransferSyntax outputSyntax)
        {
            return inputSyntax switch
            {
                TransferSyntax.JPEGProcess1 => new JpegProcess1Codec(outputSyntax),
                TransferSyntax.JPEGProcess2_4 => new JpegProcess4Codec(outputSyntax),
                TransferSyntax.JPEGProcess14 => new JpegLossless14Codec(outputSyntax),
                TransferSyntax.JPEGProcess14SV1 => new JpegLossless14SV1Codec(outputSyntax),
                TransferSyntax.HTJ2K => new HtJpeg2000LossyCodec(outputSyntax),
                TransferSyntax.HTJ2KLosslessRPCL => new HtJpeg2000LosslessRPCLCodec(outputSyntax),
                TransferSyntax.HTJ2KLossless => new HtJpeg2000LosslessCodec(outputSyntax),
                TransferSyntax.JPEG2000Lossless => new Jpeg2KLosslessCodec(outputSyntax),
                TransferSyntax.JPEG2000Lossy => new Jpeg2KLossyCodec(outputSyntax),
                TransferSyntax.JPEGLSLossless => new JpegLsLosslessCodec(outputSyntax),
                TransferSyntax.JPEGLSNearLossless => new JpegLsNearLosslessCodec(outputSyntax),
                TransferSyntax.RLELossless => new RleCodec(outputSyntax),
                _ => throw new CodecException($"No codec available for transfer syntax {inputSyntax}"),
            };
        }
    }
}
