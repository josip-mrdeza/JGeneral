using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;

namespace JGeneral.IO.Net.V2
{
    public static class NetworkExtensions
    {
        public static Stream GetInputStream(this HttpListenerContext context) => context.Request.InputStream;
        public static Stream GetOutputStream(this HttpListenerContext context) => context.Response.OutputStream;
        public static Uri GetUri(this HttpListenerContext context) => context.Request.Url;

        public static string[] GetUriSegments(this HttpListenerContext context) => context.GetUri().Segments
            .Select(x => x.Replace("/", string.Empty))
            .Except(new []{"/", "", " "}).ToArray();

        public static byte[] ReadAllInput(this HttpListenerContext context)
        {
            var bytes = new byte[context.Request.ContentLength64];
            context.GetInputStream().Read(bytes, 0, bytes.Length);

            return bytes;
        }
        public static string ReadAllInputAsString(this HttpListenerContext context)
        {
            var bytes = new byte[context.Request.ContentLength64];
            context.GetInputStream().Read(bytes, 0, bytes.Length);

            return Encoding.ASCII.GetString(bytes);
        }
        public static string ReadAllInputAsString(this HttpListenerContext context, out int bytesRead)
        {
            var bytes = new byte[context.Request.ContentLength64];
            context.GetInputStream().Read(bytes, 0, bytes.Length);
            bytesRead = bytes.Length;
            return Encoding.ASCII.GetString(bytes);
        }
        
        public static int WriteAllToOutput(this byte[] data, HttpListenerContext context)
        {
            var length = data.Length;
            context.Response.ContentLength64 = data.Length;
            context.GetOutputStream().Write(data, 0, data.Length);

            return length;
        }
        /// <summary>
        /// Writes all text to the output stream in an ASCII format.
        /// </summary>
        /// <returns>The number of bytes written to stream.</returns>
        public static int WriteStringToOutput(this string text, HttpListenerContext context)
        {
            var data = Encoding.ASCII.GetBytes(text).WriteAllToOutput(context);

            return data;
        }
    }
}