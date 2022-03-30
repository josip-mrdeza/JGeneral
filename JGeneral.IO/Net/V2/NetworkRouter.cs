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
using JGeneral.IO.Net.V2.Services.Helpers;

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
                    var segments = context.GetUriSegments();
                    UriSegment = segments[0];
                    if (UriSegment == "favicon.ico")
                    {
                        File.ReadAllBytes(@"C:\Users\Jzf\Downloads\goat.ico").WriteAllToOutput(context);
                        context.Response.Close();
                        return;
                    }
                    var service = Services[UriSegment];
                    var contextInfo = new ContextInfo();
                    switch (context.Request.HttpMethod)
                    {
                        case "GET":
                        {
                            service._Get(context, segments, ref contextInfo);
                            break;
                        }
                        case "POST":
                        {
                            service._Get(context, segments, ref contextInfo);
                            break;
                        }         
                        case "PUT":
                        {
                            service._Get(context, segments, ref contextInfo);
                            break;
                        }
                        default:
                        {
                            contextInfo.Code = 503;
                            break;
                        }
                    }

                    context.Response.StatusCode = contextInfo.Code;
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