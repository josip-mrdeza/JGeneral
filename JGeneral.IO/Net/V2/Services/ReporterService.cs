using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using JGeneral.IO.Database;
using JGeneral.IO.Net.V2.Services;

namespace JGeneral.IO.Net.V2.Services
{
    public sealed class ReporterService : NetworkService
    {
        public Dictionary<string, List<string>> Reports = new Dictionary<string, List<string>>();
        public ReporterService()
        {
            Id = "report";
        }

        protected override (int code, int received, int sent) Get(HttpListenerContext httpListenerContext)
        {
            int code = 0;
            var received = 0;
            var sent = 0;
            try
            {
                var username = httpListenerContext.GetUriSegments()[1];
                if (Reports.ContainsKey(username))
                {
                    Reports[username].ToJsonBytes().WriteAllToOutput(httpListenerContext);
                    code = 200;
                }
                else
                {
                    var str = $"Dictionary does not contain values for id: {username}!";
                    Encoding.ASCII.GetBytes(str).WriteAllToOutput(httpListenerContext);
                    code = 404;
                }
            }
            catch (Exception e)
            {
                code = 500;
                Logger.Log(e, nameof(ReporterService), nameof(Get));
            }
            
            return (code, received, sent);
        }

        protected override (int code, int received, int sent) Post(HttpListenerContext httpListenerContext)
        {
            int code = 0;
            var received = 0;
            var sent = 0;
            try
            {
                var data = httpListenerContext.ReadAllInputAsString(out received);
                var username = httpListenerContext.GetUriSegments()[1];
                if (Reports.ContainsKey(username))
                {
                    Reports[username].Add(data);
                }
                else
                {
                    Reports.Add(username, new List<string>()
                    {
                        data
                    });
                }

                code = 201;
            }
            catch (Exception e)
            {
                code = 500;
                Logger.Log(e, nameof(ReporterService), nameof(Post));
            }

            return (code, received, sent);
        }
    }
}