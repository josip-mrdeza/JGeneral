using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using JGeneral.IO.Logging;
using JGeneral.IO.Net.V2.Services;

namespace JGeneral.IO.Net.V2
{
    public sealed class NetworkRouter
    {
        private HttpListener Server = new HttpListener();
        public Dictionary<string, NetworkService> Services = new Dictionary<string, NetworkService>();
        internal static ConsoleLogger Logger = new ConsoleLogger();
        internal static NetworkRouter Instance;
        internal NetworkRouter()
        {
            try
            {
                Logger ??= new ConsoleLogger();
                string prefix = "http://+:80/";
                Server.Prefixes.Add(prefix);
                Logger.Log($"Successfully added prefix: {prefix}!", ILogger.LogType.Info);
            }
            catch (Exception e)
            {
                Logger.Log(e);
            }
            finally
            {
                Instance = this;
            }
        }

        public void Start()
        {
            try
            {
                Server.Start();
                while (true)
                {
                    ResolveContext();
                }
            }
            catch (Exception e)
            {
                Logger.Log(e, "NetworkRouter", "Start");
            }
        }

        private void ResolveContext()
        {
            var context = Server.GetContext();
            Task.Factory.StartNew(() =>
            {
                String UriSegment = null;
                try
                {
                    UriSegment = context.GetUriSegments().FirstOrDefault();
                    if (UriSegment == "favicon.ico")
                    {
                        File.ReadAllBytes(@"C:\Users\Jzf\Downloads\goat.ico").WriteAllToOutput(context);
                        context.Response.Close();
                        return;
                    }
                    var service = Services[UriSegment];
                    context.Response.StatusCode = (context.Request.HttpMethod switch
                    {
                        "GET"  => service._Get(context),
                        "POST" => service._Post(context),
                        "PUT"  => service._Put(context),
                        _      => (code: 403, received: 0, sent: 0)
                    }).code;
                    context.Response.Close();
                }
                catch (KeyNotFoundException keyNotFoundException)
                {
                    Logger.Log(keyNotFoundException, nameof(NetworkRouter), nameof(ResolveContext), UriSegment, true);
                    context.Response.StatusCode = 404;
                    context.Response.Close();
                }
                catch (Exception e)
                {
                    Logger.Log(e, nameof(NetworkRouter), nameof(ResolveContext), null, true);
                    context.Response.StatusCode = 500;
                    context.Response.Close();
                }
            });
        }
    }
}