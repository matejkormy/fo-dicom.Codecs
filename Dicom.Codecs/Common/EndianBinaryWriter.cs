
namespace Dicom.Codecs.Common
{
    using System;
    using System.IO;
    using System.Text;

    internal class EndianBinaryWriter : BinaryWriter
    {
        private bool _swapBytes = false;

        /// <summary>
        /// Initializes an instance of the <see cref="EndianBinaryWriter"/> class.
        /// </summary>
        /// <param name="output">Stream to which output should be written.</param>
        /// <param name="leaveOpen"><see langword="true" /> to leave the stream open after the <see cref="T:System.IO.BinaryWriter" /> object is disposed; otherwise, <see langword="false" />.</param>
        /// <remarks>Uses the endianness of the system.</remarks>
        public EndianBinaryWriter(Stream output, bool leaveOpen)
            : base(output, Encoding.UTF8, leaveOpen)
        {
        }

        /// <summary>
        /// Initializes an instance of the <see cref="EndianBinaryWriter"/> class.
        /// </summary>
        /// <param name="output">Stream to which output should be written.</param>
        /// <param name="encoding">Output encoding.</param>
        /// <param name="leaveOpen"><see langword="true" /> to leave the stream open after the <see cref="T:System.IO.BinaryWriter" /> object is disposed; otherwise, <see langword="false" />.</param>
        /// <remarks>Uses the endianness of the system.</remarks>
        public EndianBinaryWriter(Stream output, Encoding encoding, bool leaveOpen)
            : base(output, encoding, leaveOpen)
        {
        }

        /// <summary>
        /// Initializes an instance of the <see cref="EndianBinaryWriter"/> class.
        /// </summary>
        /// <param name="output">Stream to which output should be written.</param>
        /// <param name="endian">Endianness of the output.</param>
        /// <param name="leaveOpen"><see langword="true" /> to leave the stream open after the <see cref="T:System.IO.BinaryWriter" /> object is disposed; otherwise, <see langword="false" />.</param>
        public EndianBinaryWriter(Stream output, Endian endian, bool leaveOpen)
            : base(output, Encoding.UTF8, leaveOpen)
        {
            Endian = endian;
        }

        /// <summary>
        /// Initializes an instance of the <see cref="EndianBinaryWriter"/> class.
        /// </summary>
        /// <param name="output">Stream to which output should be written.</param>
        /// <param name="encoding">Output encoding.</param>
        /// <param name="endian">Endianness of the output.</param>
        /// <param name="leaveOpen"><see langword="true" /> to leave the stream open after the <see cref="T:System.IO.BinaryWriter" /> object is disposed; otherwise, <see langword="false" />.</param>
        public EndianBinaryWriter(Stream output, Encoding encoding, Endian endian, bool leaveOpen)
            : base(output, encoding, leaveOpen)
        {
            Endian = endian;
        }

        /// <summary>
        /// Convenience method for creating a sufficient binary writer based on specified <paramref name="endian">endianness</paramref>.
        /// </summary>
        /// <param name="output">Stream to which output should be written.</param>
        /// <param name="endian">Endianness of the output.</param>
        /// <param name="leaveOpen"><see langword="true" /> to leave the stream open after the <see cref="T:System.IO.BinaryWriter" /> object is disposed; otherwise, <see langword="false" />.</param>
        /// <returns>Binary writer with desired <paramref name="endian">endianness</paramref>-</returns>
        public static BinaryWriter Create(Stream output, Endian endian, bool leaveOpen)
        {
            if (output == null) throw new ArgumentNullException(nameof(output));

            if (BitConverter.IsLittleEndian)
            {
                if (Endian.Little == endian)
                {
                    return new BinaryWriter(output, Encoding.UTF8, leaveOpen);
                }
                else
                {
                    return new EndianBinaryWriter(output, endian, leaveOpen);
                }
            }
            else
            {
                if (Endian.Big == endian)
                {
                    return new BinaryWriter(output, Encoding.UTF8, leaveOpen);
                }
                else
                {
                    return new EndianBinaryWriter(output, endian, leaveOpen);
                }
            }
        }

        /// <summary>
        /// Convenience method for creating a sufficient binary writer based on specified <paramref name="endian">endianness</paramref>.
        /// </summary>
        /// <param name="output">Stream to which output should be written.</param>
        /// <param name="encoding">Output encoding.</param>
        /// <param name="endian">Endianness of the output.</param>
        /// <param name="leaveOpen"><see langword="true" /> to leave the stream open after the <see cref="T:System.IO.BinaryWriter" /> object is disposed; otherwise, <see langword="false" />.</param>
        /// <returns>Binary writer with desired <paramref name="endian">endianness</paramref>-</returns>
        public static BinaryWriter Create(Stream output, Encoding encoding, Endian endian, bool leaveOpen)
        {
            if (encoding == null) return Create(output, endian, leaveOpen);
            if (output == null) throw new ArgumentNullException(nameof(output));

            if (BitConverter.IsLittleEndian)
            {
                if (Endian.Little == endian)
                {
                    return new BinaryWriter(output, encoding, leaveOpen);
                }
                else
                {
                    return new EndianBinaryWriter(output, encoding, endian, leaveOpen);
                }
            }
            else
            {
                if (Endian.Big == endian)
                {
                    return new BinaryWriter(output, encoding, leaveOpen);
                }
                else
                {
                    return new EndianBinaryWriter(output, encoding, endian, leaveOpen);
                }
            }
        }

        /// <summary>
        /// Gets or sets the endianness of the binary writer.
        /// </summary>
        public Endian Endian
        {
            get
            {
                if (BitConverter.IsLittleEndian)
                {
                    return _swapBytes ? Endian.Big : Endian.Little;
                }
                else
                {
                    return _swapBytes ? Endian.Little : Endian.Big;
                }
            }
            protected set
            {
                if (BitConverter.IsLittleEndian)
                {
                    _swapBytes = Endian.Big == value;
                }
                else
                {
                    _swapBytes = Endian.Little == value;
                }
            }
        }

        private void WriteInternal(byte[] buffer)
        {
            if (_swapBytes)
            {
                Array.Reverse(buffer);
            }
            base.Write(buffer);
        }

        /// <inheritdoc />
        public override void Write(double value)
        {
            if (_swapBytes)
            {
                var b = BitConverter.GetBytes(value);
                WriteInternal(b);
            }
            else
            {
                base.Write(value);
            }
        }

        /// <inheritdoc />
        public override void Write(float value)
        {
            if (_swapBytes)
            {
                var b = BitConverter.GetBytes(value);
                WriteInternal(b);
            }
            else
            {
                base.Write(value);
            }
        }

        /// <inheritdoc />
        public override void Write(int value)
        {
            if (_swapBytes)
            {
                var b = BitConverter.GetBytes(value);
                WriteInternal(b);
            }
            else
            {
                base.Write(value);
            }
        }

        /// <inheritdoc />
        public override void Write(long value)
        {
            if (_swapBytes)
            {
                var b = BitConverter.GetBytes(value);
                WriteInternal(b);
            }
            else
            {
                base.Write(value);
            }
        }

        /// <inheritdoc />
        public override void Write(short value)
        {
            if (_swapBytes)
            {
                var b = BitConverter.GetBytes(value);
                WriteInternal(b);
            }
            else
            {
                base.Write(value);
            }
        }

        /// <inheritdoc />
        public override void Write(uint value)
        {
            if (_swapBytes)
            {
                byte[] b = BitConverter.GetBytes(value);
                WriteInternal(b);
            }
            else
            {
                base.Write(value);
            }
        }

        /// <inheritdoc />
        public override void Write(ulong value)
        {
            if (_swapBytes)
            {
                byte[] b = BitConverter.GetBytes(value);
                WriteInternal(b);
            }
            else
            {
                base.Write(value);
            }
        }

        /// <inheritdoc />
        public override void Write(ushort value)
        {
            if (_swapBytes)
            {
                byte[] b = BitConverter.GetBytes(value);
                WriteInternal(b);
            }
            else
            {
                base.Write(value);
            }
        }
    }

}
