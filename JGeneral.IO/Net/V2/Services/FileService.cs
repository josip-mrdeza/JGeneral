using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using JGeneral.IO.Net.V2.Services.Helpers;

namespace JGeneral.IO.Net.V2.Services
{
    public sealed class FileService : NetworkService
    {
        public FileService()
        {
            Id = "ftp";
        }

        protected override void Get(HttpListenerContext httpListenerContext, string[] resources, ref ContextInfo info)
        {
            try
            {
                var data = File.ReadAllBytes(resources[1]);
                data.WriteAllToOutput(httpListenerContext);
                info.Received = (int)httpListenerContext.Request.ContentLength64;
                info.Sent = data.Length;
                info.Code = 200;
            }
            catch (Exception e)
            {
                info.Code = 500;
                Logger.Log(e, nameof(FileService), nameof(Get));
                LatestException = e;
            }
        }

        protected override void Post(HttpListenerContext httpListenerContext, string[] resources, ref ContextInfo info)
        {
            try
            {
                var data = httpListenerContext.ReadAllInput();
                info.Received = data.Length;
                var filename = resources[1];
                File.WriteAllBytes(filename, data);
                data = Encoding.UTF8.GetBytes($"Successfully created/updated file \"{filename}\"!");
                data.WriteAllToOutput(httpListenerContext);
                info.Sent = data.Length;
                info.Code = 201;
            }
            catch (Exception e)
            {
                info.Code = 500;
                Logger.Log(e, nameof(FileService), nameof(Post));
                LatestException = e;
            }
        }
    }
}