using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace JGeneral.IO.Net.Tcp
{
    public static class ContextExtensions
    {
        public static void WriteTcp(this NetworkStream stream, TcpContext context)
        {
            stream.Write(BitConverter.GetBytes(context.Length), 0, 4);
            stream.Write(context.Data, 0, context.Length);
        }

        public static void WriteTcp(this NetworkStream stream, string data)
        {
            stream.WriteTcp(Encoding.ASCII.GetBytes(data));
        }
        
        public static async Task WriteTcpAsync(this NetworkStream stream, TcpContext context)
        {
            await stream.WriteAsync(BitConverter.GetBytes(context.Length), 0, 4);
            await stream.WriteAsync(context.Data, 4, context.Length);
        }

        public static async Task WriteTcpAsync(this NetworkStream stream, string data)
        {
            await stream.WriteTcpAsync(Encoding.ASCII.GetBytes(data));
        }

        public static byte[] ReadTcp(this NetworkStream stream)
        {
            return stream.GetContext().Read();
        }
        
        public static async Task<byte[]> ReadTcpAsync(this NetworkStream stream)
        {
            return await stream.GetContext().ReadAsync();
        }
        
        public static TcpContext GetContext(this NetworkStream stream)
        {
            return TcpContext.CreateFromStream(stream);
        }
    }
}