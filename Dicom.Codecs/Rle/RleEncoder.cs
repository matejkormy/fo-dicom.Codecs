
namespace Dicom.Codecs.Rle
{
    using System;
    using System.IO;
    using Dicom.Codecs.Common;

    internal class RleEncoder : IDisposable
    {
        private bool _disposed = false;
        private int _count;
        private readonly uint[] _offsets;
        private readonly MemoryStream _stream;
        private readonly BinaryWriter _writer;
        private readonly byte[] _buffer;
        private int _prevByte;
        private int _repeatCount;
        private int _bufferPos;

        internal RleEncoder()
        {
            Length = 0;
            _count = 0;
            _offsets = new uint[15];
            _stream = new MemoryStream();
            _writer = EndianBinaryWriter.Create(_stream, Endian.Little, false);
            _buffer = new byte[132];

            // Write header
            AppendUInt32((uint)_count);
            for (var i = 0; i < 15; i++)
            {
                AppendUInt32(_offsets[i]);
            }

            _prevByte = -1;
            _repeatCount = 0;
            _bufferPos = 0;
        }

        internal long Length { get; private set; }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _writer.Dispose();
            _stream.Dispose();

            _disposed = true;
        }

        internal byte[] GetBuffer()
        {
            Flush();

            // Re-write header
            _stream.Seek(0, SeekOrigin.Begin);
            _writer.Write((uint)_count);
            for (var i = 0; i < 15; i++)
            {
                _writer.Write(_offsets[i]);
            }

            return _stream.ToArray();
        }

        internal void NextSegment()
        {
            Flush();
            if ((Length & 1) == 1)
            {
                AppendByte(0x00);
            }
            _offsets[_count++] = (uint)Length;
        }

        internal void Encode(byte b)
        {
            if (b == _prevByte)
            {
                _repeatCount++;

                if (_repeatCount > 2 && _bufferPos > 0)
                {
                    // We're starting a run, flush out the buffer
                    while (_bufferPos > 0)
                    {
                        var count = Math.Min(128, _bufferPos);
                        AppendByte((byte)(count - 1));
                        MoveBuffer(count);
                    }
                }
                else if (_repeatCount > 128)
                {
                    var count = Math.Min(_repeatCount, 128);
                    AppendByte((byte)(257 - count));
                    AppendByte((byte)_prevByte);
                    _repeatCount -= count;
                }
            }
            else
            {
                switch (_repeatCount)
                {
                    case 0:
                        break;
                    case 1:
                        {
                            _buffer[_bufferPos++] = (byte)_prevByte;
                            break;
                        }
                    case 2:
                        {
                            _buffer[_bufferPos++] = (byte)_prevByte;
                            _buffer[_bufferPos++] = (byte)_prevByte;
                            break;
                        }
                    default:
                        {
                            while (_repeatCount > 0)
                            {
                                var count = Math.Min(_repeatCount, 128);
                                AppendByte((byte)(257 - count));
                                AppendByte((byte)_prevByte);
                                _repeatCount -= count;
                            }
                            break;
                        }
                }

                while (_bufferPos > 128)
                {
                    var count = Math.Min(128, _bufferPos);
                    AppendByte((byte)(count - 1));
                    MoveBuffer(count);
                }

                _prevByte = b;
                _repeatCount = 1;
            }
        }

        internal void MakeEvenLength()
        {
            if ((Length & 1) == 1) { AppendByte(0x00); }
        }

        internal void Flush()
        {
            if (_repeatCount < 2)
            {
                while (_repeatCount > 0)
                {
                    _buffer[_bufferPos++] = (byte)_prevByte;
                    _repeatCount--;
                }
            }

            while (_bufferPos > 0)
            {
                var count = Math.Min(128, _bufferPos);
                AppendByte((byte)(count - 1));
                MoveBuffer(count);
            }

            if (_repeatCount >= 2)
            {
                while (_repeatCount > 0)
                {
                    var count = Math.Min(_repeatCount, 128);
                    AppendByte((byte)(257 - count));
                    AppendByte((byte)_prevByte);
                    _repeatCount -= count;
                }
            }

            _prevByte = -1;
            _repeatCount = 0;
            _bufferPos = 0;
        }

        private void MoveBuffer(int count)
        {
            AppendBytes(_buffer, 0, count);
            for (int i = count, n = 0; i < _bufferPos; i++, n++)
            {
                _buffer[n] = _buffer[i];
            }
            _bufferPos -= count;
        }

        private void AppendBytes(byte[] bytes, int offset, int count)
        {
            _writer.Write(bytes, offset, count);
            Length += count;
        }

        private void AppendByte(byte value)
        {
            _writer.Write(value);
            ++Length;
        }

        private void AppendUInt32(uint value)
        {
            _writer.Write(value);
            Length += 4;
        }
    }
}
