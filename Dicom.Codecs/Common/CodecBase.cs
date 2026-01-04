
namespace Dicom.Codecs.Common
{
    using System;
    using System.Runtime.CompilerServices;

    internal abstract class CodecBase : ICodec
    {
        protected CodecBase()
        {
            if (Platform.Current == Platform.Type.unsupported)
            {
                throw new InvalidOperationException("Unsupported OS Platform");
            }
        }

        public abstract TransferSyntax TransferSyntax { get; }

        public abstract TransferSyntax OutputTransferSyntax { get; }

        public abstract byte[] Encode(
            byte[] input,
            FrameMetadata metadata,
            out FrameMetadata outputMetadata,
            CodecParams codecParams = null);

        public abstract void Decode(
            byte[] input,
            byte[] output,
            FrameMetadata metadata,
            out FrameMetadata outputMetadata,
            CodecParams codecParams = null);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected byte[] ConvertToRgbIfNeeded(byte[] input, FrameMetadata metadata) => metadata.PhotometricInterpretation switch
        {
            PhotometricInterpretation.YbrFull => PixelDataConverter.YbrFullToRgb(input),
            PhotometricInterpretation.YbrFull422 => PixelDataConverter.YbrFull422ToRgb(input, metadata.Width),
            _ => input,
        };
    }
}
