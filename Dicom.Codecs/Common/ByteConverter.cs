
namespace Dicom.Codecs.Common
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;

    internal static class ByteConverter
    {
        public static byte[] ToByteBuffer(string value, Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8;
            return encoding.GetBytes(value);
        }

        public static byte[] ToByteBuffer(string value, Encoding encoding, byte padding)
        {
            encoding ??= Encoding.UTF8;

            byte[] bytes = encoding.GetBytes(value);

            if ((bytes.Length & 1) == 1) // is odd length
            {
                Array.Resize(ref bytes, bytes.Length + 1);
                bytes[bytes.Length - 1] = padding;
            }

            return bytes;
        }

        public static byte[] ToByteBuffer<T>(T[] values) where T : struct
        {
            int size = System.Buffer.ByteLength(values);
            byte[] data = new byte[size];
            System.Buffer.BlockCopy(values, 0, data, 0, size);
            return data;
        }

        public static T[] ToArray<T>(byte[] buffer)
        {
            uint size = (uint)Marshal.SizeOf<T>();
            uint padding = (uint)(buffer.Length % size);
            uint count = (uint)(buffer.Length / size);
            var values = new T[count];
            System.Buffer.BlockCopy(buffer, 0, values, 0, (int)(buffer.Length - padding));
            return values;
        }

        public static T[] ToArray<T>(byte[] buffer, int bitsAllocated)
        {
            var bytesRequested = Marshal.SizeOf<T>();
            var bitsRequested = 8 * bytesRequested;
            if (bitsAllocated > bitsRequested)
            {
                throw new ArgumentOutOfRangeException(nameof(bitsAllocated), "Bits allocated too large for array type");
            }
            if (bitsAllocated == bitsRequested)
            {
                return ToArray<T>(buffer);
            }

            var count = (int)(8 * buffer.Length / bitsAllocated);
            var src = buffer;
            var dst = new byte[bytesRequested * count];

            for (int j = 0, sij = 0; j < count; ++j)
            {
                for (int i = 0, dij = j * bitsRequested; i < bitsAllocated; ++i, ++sij, ++dij)
                {
                    if ((src[sij / 8] & (1 << (sij % 8))) != 0)
                    {
                        dst[dij / 8] |= (byte)(1 << (dij % 8));
                    }
                }
            }

            var values = new T[count];
            System.Buffer.BlockCopy(dst, 0, values, 0, dst.Length);

            return values;
        }

        public static T Get<T>(byte[] buffer, int n)
        {
            int size = Marshal.SizeOf<T>();
            var values = new T[1];

            System.Buffer.BlockCopy(buffer, size * n, values, 0, size);

            return values[0];
        }

        public static byte[] UnpackLow16(byte[] data)
        {
            byte[] bytes = new byte[data.Length / 2];
            for (int i = 0; i < bytes.Length && (i * 2) < data.Length; i++)
            {
                bytes[i] = data[i * 2];
            }
            return bytes;
        }

        public static byte[] UnpackHigh16(byte[] data)
        {
            byte[] bytes = new byte[data.Length / 2];
            for (int i = 0; i < bytes.Length && ((i * 2) + 1) < data.Length; i++)
            {
                bytes[i] = data[(i * 2) + 1];
            }
            return bytes;
        }
    }
}
