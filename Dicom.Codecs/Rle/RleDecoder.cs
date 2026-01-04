
namespace Dicom.Codecs.Rle
{
    using System;
    using System.Buffers.Binary;

    internal class RleDecoder
    {
        private readonly int[] _offsets;
        private readonly byte[] _data;

        internal RleDecoder(byte[] data)
        {
            var span = data.AsSpan();
            NumberOfSegments = BinaryPrimitives.ReadInt32LittleEndian(span);

            _offsets = new int[15];
            for (var i = 0; i < 15; ++i)
            {
                span = span.Slice(4);
                _offsets[i] = BinaryPrimitives.ReadInt32LittleEndian(span);
            }

            _data = data;
        }


        internal int NumberOfSegments { get; }

        internal void DecodeSegment(int segment, byte[] buffer, int start, int sampleOffset)
        {
            if (segment < 0 || segment >= NumberOfSegments)
            {
                throw new ArgumentOutOfRangeException("Segment number out of range");
            }

            var offset = GetSegmentOffset(segment);
            var length = GetSegmentLength(segment);

            Decode(buffer, start, sampleOffset, _data, offset, length);
        }

        private static void Decode(byte[] buffer, int start, int sampleOffset, byte[] rleData, int offset, int count)
        {
            var pos = start;
            var end = offset + count;
            var bufferLength = buffer.Length;

            for (var i = offset; i < end && pos < bufferLength;)
            {
                var control = (sbyte)rleData[i++];

                if (control >= 0)
                {
                    var length = control + 1;

                    if ((end - i) < length)
                    {
                        throw new InvalidOperationException("RLE literal run exceeds input buffer length.");
                    }
                    if ((pos + ((length - 1) * sampleOffset)) >= bufferLength)
                    {
                        throw new InvalidOperationException("RLE literal run exceeds output buffer length.");
                    }

                    if (sampleOffset == 1)
                    {
                        for (var j = 0; j < length; ++j, ++i, ++pos)
                        {
                            buffer[pos] = rleData[i];
                        }
                    }
                    else
                    {
                        while (length-- > 0)
                        {
                            buffer[pos] = rleData[i++];
                            pos += sampleOffset;
                        }
                    }
                }
                else if (control >= -127)
                {
                    int length = -control;

                    if ((pos + ((length - 1) * sampleOffset)) >= bufferLength)
                    {
                        throw new InvalidOperationException("RLE repeat run exceeds output buffer length.");
                    }

                    var b = rleData[i++];

                    if (sampleOffset == 1)
                    {
                        while (length-- >= 0)
                        {
                            buffer[pos++] = b;
                        }
                    }
                    else
                    {
                        while (length-- >= 0)
                        {
                            buffer[pos] = b;
                            pos += sampleOffset;
                        }
                    }
                }

                if ((i + 1) >= end)
                {
                    break;
                }
            }
        }

        private int GetSegmentOffset(int segment)
        {
            return _offsets[segment];
        }

        private int GetSegmentLength(int segment)
        {
            var offset = GetSegmentOffset(segment);
            if (segment < (NumberOfSegments - 1))
            {
                var next = GetSegmentOffset(segment + 1);
                return next - offset;
            }

            return _data.Length - offset;
        }
    }
}
