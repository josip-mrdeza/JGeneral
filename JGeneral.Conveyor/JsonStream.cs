using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace JGeneral.Conveyor
{
    public class JsonStream
    {
        private Stream ioStream;
        private UnicodeEncoding streamEncoding;

        public JsonStream(Stream ioStream)
        {
            this.ioStream = ioStream;
            streamEncoding = new UnicodeEncoding();
        }

        public string ReadJson()
        {
            int len = 0;

            len = ioStream.ReadByte() * 256;
            len += ioStream.ReadByte();
            byte[] inBuffer = new byte[len];
            ioStream.Read(inBuffer, 0, len);

            return streamEncoding.GetString(inBuffer);
        }
        
        public async Task<string> ReadJsonAsync()
        {
            int len = 0;

            len = ioStream.ReadByte() * 256;
            len += ioStream.ReadByte();
            byte[] inBuffer = new byte[len];
            await ioStream.ReadAsync(inBuffer, 0, len);

            return streamEncoding.GetString(inBuffer);
        }

        public int WriteJson(string outString)
        {
            byte[] outBuffer = streamEncoding.GetBytes(outString);
            int len = outBuffer.Length;
            if (len > UInt16.MaxValue)
            {
                len = (int) UInt16.MaxValue;
            }

            ioStream.WriteByte((byte) (len / 256));
            ioStream.WriteByte((byte) (len & 255));
            ioStream.WriteAsync(outBuffer, 0, len);
            ioStream.FlushAsync();

            return outBuffer.Length + 2;
        }
        
        public async Task<int> WriteJsonAsync(string outString)
        {
            byte[] outBuffer = streamEncoding.GetBytes(outString);
            int len = outBuffer.Length;
            if (len > UInt16.MaxValue)
            {
                len = (int) UInt16.MaxValue;
            }

            ioStream.WriteByte((byte) (len / 256));
            ioStream.WriteByte((byte) (len & 255));
            await ioStream.WriteAsync(outBuffer, 0, len);
            await ioStream.FlushAsync();

            return outBuffer.Length + 2;
        }
    }
}