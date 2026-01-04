
namespace Dicom.Codecs.Common
{
    using System;
    using System.Runtime.InteropServices;

    internal class PinnedByteArray : IDisposable
    {
        private readonly GCHandle _handle;

        public PinnedByteArray(int count)
        {
            Count = count;
            ByteSize = Marshal.SizeOf<byte>() * Count;
            Data = new byte[Count];
            _handle = GCHandle.Alloc(Data, GCHandleType.Pinned);
            Pointer = _handle.AddrOfPinnedObject();
        }

        public PinnedByteArray(byte[] data)
        {
            _handle = GCHandle.Alloc(data, GCHandleType.Pinned);
            Count = data.Length;
            ByteSize = Marshal.SizeOf<byte>() * Count;
            Data = data;
            Pointer = _handle.AddrOfPinnedObject();
        }

        public byte[] Data { get; private set; }

        public int Count { get ; private set; }

        public int ByteSize { get; private set; }

        public IntPtr Pointer { get; private set; }

        public byte this[int index]
        {
            get => Data[index];
            set => Data[index] = value;
        }

        ~PinnedByteArray()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public static implicit operator IntPtr(PinnedByteArray array)
        {
            return array.Pointer;
        }

        private void Dispose(bool disposing)
        {
            if (Data != null)
            {
                if (_handle.IsAllocated)
                {
                    _handle.Free();
                }

                Pointer = IntPtr.Zero;
                Data = null;
            }
        }
    }
}
