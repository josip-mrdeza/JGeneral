using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using JGeneral.IO.Database;

namespace JGeneral.IO.Net.V2.Services
{
    public sealed class ManagerService : NetworkService
    {
        private Dictionary<string, NetworkService> NetworkServices;
        public ManagerService()
        {
            Id = "manager";
            NetworkServices = NetworkRouter.Instance.Services;
        }

        protected override (int code, int received, int sent) Get(HttpListenerContext httpListenerContext)
        {
            int code, received, sent;
            received = 0;
            sent = 0;
            try
            {
                var segments = httpListenerContext.GetUriSegments();
                string json = string.Empty;
                if (segments.Length == 1)
                {
                    json = NetworkServices.ToJson();
                }
                else if (segments.Length > 1)
                {
                    var id = segments[1];
                    var service = NetworkServices[id];
                    json = service.ToJson();
                }

                code = 200;
                sent = Encoding.UTF8.GetBytes(json).WriteAllToOutput(httpListenerContext);
            }
            catch (Exception e)
            {
                code = 500;
                Logger.Log(e, nameof(ManagerService), nameof(Get));
                LatestException = e;
            }

            return (code, received, sent);
        }
    }
}