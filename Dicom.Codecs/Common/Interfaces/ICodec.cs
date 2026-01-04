
namespace Dicom.Codecs.Common
{
    public interface ICodec
    {
        TransferSyntax TransferSyntax { get; }

        byte[] Encode(
            byte[] input,
            FrameMetadata inputMetadata,
            out FrameMetadata outputMetadata,
            CodecParams codecParams = null);

        void Decode(
            byte[] input,
            byte[] output,
            FrameMetadata inputMetadata,
            out FrameMetadata outputMetadata,
            CodecParams codecParams = null);
    }
}
