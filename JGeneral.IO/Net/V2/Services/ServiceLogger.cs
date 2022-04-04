using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using JGeneral.IO.Database;
using JGeneral.IO.Net.V2.Services.Helpers;

namespace JGeneral.IO.Net.V2.Services
{
    public class ServiceLogger : NetworkService
    {
        public Dictionary<string, List<string>> Logs = new Dictionary<string, List<string>>();
        public ServiceLogger()
        {
            Id = "logger";
            var services = RouterReference.Services;
            foreach (var service in services)
            {
                Logs.Add(service.Key, new List<string>() {});
            }
        }

        protected override void Get(HttpListenerContext httpListenerContext, string[] resources, ref ContextInfo info)
        {
            info.Sent = Logs.ToJson().WriteStringToOutput(httpListenerContext);
        }
    }
}