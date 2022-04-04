using System;
using System.Buffers;
using System.IO;
using System.Threading.Tasks;

namespace JGeneral.IO.Net.Tcp
{
    public struct TcpContext : IDisposable
    {
        private static ArrayPool<byte> Pool;
        private byte[] PoolArray;
        internal Stream _stream;
        public int Length
        {
            get => PoolArray.Length;
        }
        public byte[] Data
        {
            get => PoolArray;
        }

        public byte[] Read()
        {
            _stream.Read(PoolArray, 0, Length);

            return PoolArray;
        }

        public async Task<byte[]> ReadAsync()
        {
            await _stream.ReadAsync(PoolArray, 0, Length);

            return PoolArray;
        }

        public void Write(byte[] data)
        {
            _stream.Write(BitConverter.GetBytes(data.Length), 0, 4);
            _stream.Write(data, 0, data.Length);
        }
        
        public async Task WriteAsync(byte[] data)
        {
            await _stream.WriteAsync(BitConverter.GetBytes(data.Length), 0, 4);
            await _stream.WriteAsync(data, 0, data.Length);
        }
        
        public static TcpContext CreateFromStream(Stream stream)
        {
            TcpContext context = new TcpContext();
            Pool = ArrayPool<byte>.Create();
            context._stream = stream;
            byte[] encodedLength = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                encodedLength[i] = (byte)stream.ReadByte();
            }

            context.PoolArray = Pool.Rent(BitConverter.ToInt32(encodedLength, 0));
            return context;
        }

        public static TcpContext CreateNew(byte[] data)
        {
            TcpContext context = new TcpContext();
            Pool = ArrayPool<byte>.Create();
            context.PoolArray = Pool.Rent(data.Length);
            context.PoolArray = data;
            return context;
        }
        public void Dispose()
        {
            Pool.Return(PoolArray, true);
        }

        public static implicit operator TcpContext(byte[] data)
        {
            return CreateNew(data);
        }
    }
}