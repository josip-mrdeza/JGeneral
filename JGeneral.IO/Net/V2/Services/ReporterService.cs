using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using JGeneral.IO.Database;
using JGeneral.IO.Net.V2.Services;
using JGeneral.IO.Net.V2.Services.Helpers;

namespace JGeneral.IO.Net.V2.Services
{
    public sealed class ReporterService : NetworkService
    {
        public Dictionary<string, List<string>> Reports = new Dictionary<string, List<string>>();
        public ReporterService()
        {
            Id = "report";
        }

        protected override void Get(HttpListenerContext httpListenerContext, string[] resources, ref ContextInfo info)
        {
            try
            {
                var username = resources[1];
                if (Reports.ContainsKey(username))
                {
                    info.Sent = Reports[username].ToJsonBytes().WriteAllToOutput(httpListenerContext);
                    info.Code = 200;
                }
                else
                {
                    var str = $"Dictionary does not contain values for id: {username}!";
                    info.Sent = Encoding.ASCII.GetBytes(str).WriteAllToOutput(httpListenerContext);
                    info.Code = 404;
                }
            }
            catch (Exception e)
            {
                info.Code = 500;
                Logger.Log(e, nameof(ReporterService), nameof(Get));
            }
        }

        protected override void Post(HttpListenerContext httpListenerContext, string[] resources, ref ContextInfo info)
        {
            try
            {
                var data = httpListenerContext.ReadAllInputAsString(out info.Received);
                var username = resources[1];
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

                info.Code = 201;
            }
            catch (Exception e)
            {
                info.Code = 500;
                Logger.Log(e, nameof(ReporterService), nameof(Post));
            }
        }
    }
}