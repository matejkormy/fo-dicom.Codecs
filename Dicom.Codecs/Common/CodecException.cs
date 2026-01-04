
namespace Dicom.Codecs.Common
{
    using System;

    public class CodecException : Exception
    {
        public CodecException(string message) : base(message)
        {
        }

        public CodecException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
