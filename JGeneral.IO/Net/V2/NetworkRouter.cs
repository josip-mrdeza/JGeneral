using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JGeneral.IO.Database;
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
                    ResolveContext(Server.GetContext());
                }
            }
            catch (Exception e)
            {
                Logger.Log(e, "NetworkRouter", "Start");
            }
        }

        public void ResolveContext(HttpListenerContext context)
        {
            Task.Factory.StartNew(() =>
            {
                String UriSegment = null;
                String[] Segments = null;
                try
                {
                    Segments = context.GetUriSegments();
                    UriSegment = Segments[0];
                    if (UriSegment == "favicon.ico")
                    {
                        //File.ReadAllBytes(@"C:\Users\Jzf\Downloads\goat.ico").WriteAllToOutput(context);
                        context.Response.Close();

                        return;
                    }

                    var service = Services[UriSegment];
                    var contextInfo = new ContextInfo(200, 0, 0);
                    switch (context.Request.HttpMethod)
                    {
                        case "GET":
                        {
                            service._Get(context, Segments, ref contextInfo);

                            break;
                        }
                        case "POST":
                        {
                            service._Post(context, Segments, ref contextInfo);

                            break;
                        }
                        case "PUT":
                        {
                            service._Put(context, Segments, ref contextInfo);

                            break;
                        }
                        default:
                        {
                            contextInfo.Code = 503;

                            break;
                        }
                    }

                    if (contextInfo.Code < 100 || contextInfo.Code > 999)
                    {
                        contextInfo.Code = 200;
                    }
                    context.Response.StatusCode = contextInfo.Code;
                    context.Response.Close();
                }
                catch (KeyNotFoundException keyNotFoundException)
                {
                    Logger.Log(keyNotFoundException, nameof(NetworkRouter), nameof(ResolveContext), null, true);
                    keyNotFoundException.ToJson().WriteStringToOutput(context);
                    context.Response.StatusCode = 404;
                    context.Response.Close();
                }
                catch (IndexOutOfRangeException indexOutOfRangeException)
                {
                    Logger.Log(indexOutOfRangeException, nameof(NetworkRouter), nameof(ResolveContext), null, true);
                    indexOutOfRangeException.ToJson().WriteStringToOutput(context);
                    context.Response.StatusCode = 500;
                    context.Response.Close();
                }
                catch (Exception exception)
                {
                    Logger.Log(exception, nameof(NetworkRouter), nameof(ResolveContext), null, true);
                    exception.ToJson().WriteStringToOutput(context);
                    context.Response.StatusCode = 500;
                    context.Response.Close();
                }
            });
        }
        
    }
}