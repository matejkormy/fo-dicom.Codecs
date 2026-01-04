
namespace Dicom.Codecs.Common
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Representation and convenience methods associated with endianness.
    /// </summary>
    internal struct Endian
    {
        /// <summary>
        /// Little endian.
        /// </summary>
        public static readonly Endian Little = new Endian(false);

        /// <summary>
        /// Big endian.
        /// </summary>
        public static readonly Endian Big = new Endian(true);

        /// <summary>
        /// Endianness of the local machine, according to <see cref="System.BitConverter.IsLittleEndian"/>.
        /// </summary>
        public static readonly Endian LocalMachine = BitConverter.IsLittleEndian ? Little : Big;

        /// <summary>
        /// Network endian (big).
        /// </summary>
        public static readonly Endian Network = Big;

        private readonly bool _isBigEndian;

        /// <summary>
        /// Initializes an instance of the <see cref="Endian"/> struct.
        /// </summary>
        /// <param name="isBigEndian"></param>
        private Endian(bool isBigEndian)
        {
            _isBigEndian = isBigEndian;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj is Endian) return this == (Endian)obj;
            return false;
        }

        public override int GetHashCode()
        {
            return _isBigEndian.GetHashCode();
        }

        public override string ToString()
        {
            return _isBigEndian ? "Big Endian" : "Little Endian";
        }

        /// <summary>
        /// <see cref="Endian"/> equivalence operator.
        /// </summary>
        /// <param name="e1">First <see cref="Endian"/> object.</param>
        /// <param name="e2">Second <see cref="Endian"/> object.</param>
        /// <returns>True if <paramref name="e1"/> equals <paramref name="e2"/>, false otherwise.</returns>
        public static bool operator ==(Endian e1, Endian e2)
        {
            return e1._isBigEndian == e2._isBigEndian;
        }

        /// <summary>
        /// <see cref="Endian"/> non-equivalence operator.
        /// </summary>
        /// <param name="e1">First <see cref="Endian"/> object.</param>
        /// <param name="e2">Second <see cref="Endian"/> object.</param>
        /// <returns>True if <paramref name="e1"/> does not equal <paramref name="e2"/>, false otherwise.</returns>
        public static bool operator !=(Endian e1, Endian e2)
        {
            return !(e1 == e2);
        }

        /// <summary>
        /// Swap bytes in sequences of <paramref name="bytesToSwap"/>.
        /// </summary>
        /// <param name="bytesToSwap">Number of bytes to swap.</param>
        /// <param name="bytes">Array of bytes.</param>
        public static void SwapBytes(int bytesToSwap, byte[] bytes) => SwapBytes(bytesToSwap, bytes, bytes.Length);

        /// <summary>
        /// Swap bytes in sequences of <paramref name="bytesToSwap"/>.
        /// </summary>
        /// <param name="bytesToSwap">Number of bytes to swap.</param>
        /// <param name="bytes">Array of bytes.</param>
        /// <param name="count">The maximum number of bytes in the array that should be processed</param>
        public static void SwapBytes(int bytesToSwap, byte[] bytes, int count)
        {
            if (count > bytes.Length)
            {
                throw new ArgumentException($"Cannot swap {count} bytes of an array that only contains {bytes.Length} bytes");
            }

            if (count == 0)
            {
                return;
            }

            if (bytesToSwap == 1)
            {
                return;
            }

            if (bytesToSwap == 2)
            {
                SwapBytes2(bytes, count);
                return;
            }
            if (bytesToSwap == 4)
            {
                SwapBytes4(bytes, count);
                return;
            }

            unchecked
            {
                var l = count - (count % bytesToSwap);
                for (var i = 0; i < l; i += bytesToSwap)
                {
                    Array.Reverse(bytes, i, bytesToSwap);
                }
            }
        }

        /// <summary>
        /// Swap bytes in sequences of 2.
        /// </summary>
        /// <param name="bytes">Array of bytes.</param>
        public static void SwapBytes2(byte[] bytes) => SwapBytes2(bytes, bytes.Length);

        /// <summary>
        /// Swap bytes in sequences of 2.
        /// </summary>
        /// <param name="bytes">Array of bytes.</param>
        /// <param name="count">The maximum number of bytes in the array that should be processed</param>
        public static void SwapBytes2(byte[] bytes, int count)
        {
            unchecked
            {
                var l = count - count % 2;
                for (var i = 0; i < l; i += 2)
                {
                    (bytes[i + 1], bytes[i]) = (bytes[i], bytes[i + 1]);
                }
            }
        }

        /// <summary>
        /// Swap bytes in sequences of 4.
        /// </summary>
        /// <param name="bytes">Array of bytes.</param>
        public static void SwapBytes4(byte[] bytes) => SwapBytes4(bytes, bytes.Length);

        /// <summary>
        /// Swap bytes in sequences of 4.
        /// </summary>
        /// <param name="bytes">Array of bytes.</param>
        /// <param name="count">The maximum number of bytes in the array that should be processed</param>
        public static void SwapBytes4(byte[] bytes, int count)
        {
            unchecked
            {
                var l = count - (count % 4);
                for (var i = 0; i < l; i += 4)
                {
                    var b = bytes[i + 3];
                    bytes[i + 3] = bytes[i];
                    bytes[i] = b;
                    b = bytes[i + 2];
                    bytes[i + 2] = bytes[i + 1];
                    bytes[i + 1] = b;
                }
            }
        }

        /// <summary>
        /// Swap byte order in <see cref="short"/> value.
        /// </summary>
        /// <param name="value">Value in which bytes should be swapped.</param>
        /// <returns>Byte order swapped value.</returns>
        public static short Swap(short value)
        {
            return (short)Swap((ushort)value);
        }

        /// <summary>
        /// Swap byte order in <see cref="ushort"/> value.
        /// </summary>
        /// <param name="value">Value in which bytes should be swapped.</param>
        /// <returns>Byte order swapped value.</returns>
        public static ushort Swap(ushort value)
        {
            return unchecked((ushort)((value >> 8) | (value << 8)));
        }

        /// <summary>
        /// Swap byte order in <see cref="int"/> value.
        /// </summary>
        /// <param name="value">Value in which bytes should be swapped.</param>
        /// <returns>Byte order swapped value.</returns>
        public static int Swap(int value)
        {
            return (int)Swap((uint)value);
        }

        /// <summary>
        /// Swap byte order in <see cref="uint"/> value.
        /// </summary>
        /// <param name="value">Value in which bytes should be swapped.</param>
        /// <returns>Byte order swapped value.</returns>
        public static uint Swap(uint value)
        {
            return
                unchecked(
                    ((value & 0x000000ffU) << 24) | ((value & 0x0000ff00U) << 8) | ((value & 0x00ff0000U) >> 8)
                    | ((value & 0xff000000U) >> 24));
        }

        /// <summary>
        /// Swap byte order in <see cref="long"/> value.
        /// </summary>
        /// <param name="value">Value in which bytes should be swapped.</param>
        /// <returns>Byte order swapped value.</returns>
        public static long Swap(long value)
        {
            return (long)Swap((ulong)value);
        }

        /// <summary>
        /// Swap byte order in <see cref="ulong"/> value.
        /// </summary>
        /// <param name="value">Value in which bytes should be swapped.</param>
        /// <returns>Byte order swapped value.</returns>
        public static ulong Swap(ulong value)
        {
            return
                unchecked(
                    ((value & 0x00000000000000ffU) << 56) | ((value & 0x000000000000ff00U) << 40)
                    | ((value & 0x0000000000ff0000U) << 24) | ((value & 0x00000000ff000000U) << 8)
                    | ((value & 0x000000ff00000000U) >> 8) | ((value & 0x0000ff0000000000U) >> 24)
                    | ((value & 0x00ff000000000000U) >> 40) | ((value & 0xff00000000000000U) >> 56));
        }

        /// <summary>
        /// Swap byte order in <see cref="float"/> value.
        /// </summary>
        /// <param name="value">Value in which bytes should be swapped.</param>
        /// <returns>Byte order swapped value.</returns>
        public static float Swap(float value)
        {
            var b = BitConverter.GetBytes(value);
            Array.Reverse(b);
            return BitConverter.ToSingle(b, 0);
        }

        /// <summary>
        /// Swap byte order in <see cref="double"/> value.
        /// </summary>
        /// <param name="value">Value in which bytes should be swapped.</param>
        /// <returns>Byte order swapped value.</returns>
        public static double Swap(double value)
        {
            var b = BitConverter.GetBytes(value);
            Array.Reverse(b);
            return BitConverter.ToDouble(b, 0);
        }

        /// <summary>
        /// Swap byte order in array of <see cref="short"/> values.
        /// </summary>
        /// <param name="values">Array of <see cref="short"/> values.</param>
        public static void Swap(short[] values)
        {
            Parallel.For(0, values.Length, i => values[i] = Swap(values[i]));
        }

        /// <summary>
        /// Swap byte order in array of <see cref="ushort"/> values.
        /// </summary>
        /// <param name="values">Array of <see cref="ushort"/> values.</param>
        public static void Swap(ushort[] values)
        {
            Parallel.For(0, values.Length, i => values[i] = Swap(values[i]));
        }

        /// <summary>
        /// Swap byte order in array of <see cref="int"/> values.
        /// </summary>
        /// <param name="values">Array of <see cref="int"/> values.</param>
        public static void Swap(int[] values)
        {
            Parallel.For(0, values.Length, i => values[i] = Swap(values[i]));
        }

        /// <summary>
        /// Swap byte order in array of <see cref="uint"/> values.
        /// </summary>
        /// <param name="values">Array of <see cref="uint"/> values.</param>
        public static void Swap(uint[] values)
        {
            Parallel.For(0, values.Length, i => values[i] = Swap(values[i]));
        }

        /// <summary>
        /// Swap byte order in array of values.
        /// </summary>
        /// <typeparam name="T">Array element type, must be one of <see cref="short"/>, <see cref="ushort"/>, <see cref="int"/> or <see cref="uint"/>.</typeparam>
        /// <param name="values">Array of values to swap.</param>
        /// <exception cref="System.InvalidOperationException">if array element type is not <see cref="short"/>, <see cref="ushort"/>, <see cref="int"/> or <see cref="uint"/>.</exception>
        public static void Swap<T>(T[] values)
        {
            if (typeof(T) == typeof(short)) Swap(values as short[]);
            else if (typeof(T) == typeof(ushort)) Swap(values as ushort[]);
            else if (typeof(T) == typeof(int)) Swap(values as int[]);
            else if (typeof(T) == typeof(uint)) Swap(values as uint[]);
            else throw new InvalidOperationException("Attempted to byte swap non-specialized type: " + typeof(T).Name);
        }
    }

}
