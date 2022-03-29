using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JGeneral.IO.Net.V2.Services
{
    public sealed class FileService : NetworkService
    {
        public FileService()
        {
            Id = "ftp";
        }

        protected override (int code, int received, int sent) Get(HttpListenerContext httpListenerContext)
        {
            int code = 0;
            int received = 0;
            int sent = 0;
            try
            {
                var data = File.ReadAllBytes(httpListenerContext.GetUriSegments()[1]);
                data.WriteAllToOutput(httpListenerContext);
                received = (int)httpListenerContext.Request.ContentLength64;
                sent = data.Length;
                code = 200;
            }
            catch (Exception e)
            {
                code = 500;
                Logger.Log(e, nameof(FileService), nameof(Get));
                LatestException = e;
            }
            
            return (code, received, sent);
        }

        protected override (int code, int received, int sent) Post(HttpListenerContext httpListenerContext)
        {
            int code = 0;
            int received = 0;
            int sent = 0;
            try
            {
                var data = httpListenerContext.ReadAllInput();
                received = data.Length;
                var filename = httpListenerContext.GetUriSegments()[1];
                File.WriteAllBytes(filename, data);
                data = Encoding.UTF8.GetBytes($"Successfully created/updated file \"{filename}\"!");
                data.WriteAllToOutput(httpListenerContext);
                sent = data.Length;
                code = 201;
            }
            catch (Exception e)
            {
                code = 500;
                Logger.Log(e, nameof(FileService), nameof(Post));
                LatestException = e;
            }
            return (code, received, sent);
        }
    }
}