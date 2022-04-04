using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using JGeneral.IO.Database;
using JGeneral.IO.Net.V2.Services.Helpers;

namespace JGeneral.IO.Net.V2.Services
{
    public sealed class ManagerService : NetworkService
    {
        private Dictionary<string, NetworkService> NetworkServices;
        public ManagerService()
        {
            Id = "manager";
            NetworkServices = RouterReference.Services;
        }

        protected override void Get(HttpListenerContext httpListenerContext, string[] resources, ref ContextInfo info)
        {
            try
            {
                string json = string.Empty;
                if (resources.Length == 1)
                {
                    json = NetworkServices.ToJson();
                }
                else if (resources.Length > 1)
                {
                    var id = resources[1];
                    var service = NetworkServices[id];
                    json = service.ToJson();
                }

                info.Code = 200;
                info.Sent = Encoding.UTF8.GetBytes(json).WriteAllToOutput(httpListenerContext);
            }
            catch (Exception e)
            {
                info.Code = 500;
                Logger.Log(e, nameof(ManagerService), nameof(Get));
                LatestException = e;
            }
        }
    }
}