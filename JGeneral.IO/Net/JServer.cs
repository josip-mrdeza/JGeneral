using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
#pragma warning disable 1998

namespace JGeneral.IO.Net
{
    public class JServer
    {
        protected JServer()
        {
            Listener = new HttpListener();
            Listener.Prefixes.Add("http://+:8090/");
        }
        protected void Start(CancellationToken token)
        {
            Listener.Start();
            Task.Run(async () =>
            {
                while (true)
                {
                    var context = await Listener.GetContextAsync();
                    try
                    {
                        switch (context.Request.HttpMethod)
                        {
                            case "GET":
                            {
                                await HttpGet(context);
                                OnCompleteGet?.Invoke(context.Request.RawUrl);
                                OnComplete_Full?.Invoke(context.Request.RawUrl, "GET", (context.Request.ContentLength64, context.Response.ContentLength64));
                                context.Request.InputStream.Close();
                                break;
                            }
                            case "POST":
                            {
                                await HttpPost(context, context.Request.ContentLength64);
                                OnCompletePost?.Invoke(context.Request.RawUrl);
                                OnComplete_Full?.Invoke(context.Request.RawUrl, "POST", (context.Request.ContentLength64, context.Response.ContentLength64));
                                context.Request.InputStream.Close(); 
                                context.Response.OutputStream.Close();
                                break;
                            }
                            case "PUT":
                            {
                                await HttpPut(context, context.Request.ContentLength64);
                                OnCompletePut?.Invoke(context.Request.RawUrl);
                                OnComplete_Full?.Invoke(context.Request.RawUrl, "PUT", (context.Request.ContentLength64, context.Response.ContentLength64));
                                context.Request.InputStream.Close();
                                context.Response.OutputStream.Close();
                                break;
                            }
                            case "PATCH":
                            {
                                await HttpPatch(context, context.Request.ContentLength64);
                                OnCompletePut?.Invoke(context.Request.RawUrl);
                                OnComplete_Full?.Invoke(context.Request.RawUrl, "PATCH", (context.Request.ContentLength64, context.Response.ContentLength64));
                                context.Request.InputStream.Close();
                                context.Response.OutputStream.Close();
                                break;
                            }
                            default:
                            {
                                context.Response.StatusCode = 204;
                                OnComplete_Full?.Invoke(context.Request.RawUrl, "unknown", (context.Request.ContentLength64, context.Response.ContentLength64));
                                context.Request.InputStream.Close();
                                context.Response.OutputStream.Close();
                                break;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        context.Response.StatusCode = 500;
                        context.Request.InputStream.Close();
                        context.Response.OutputStream.Close();
                        switch (context.Request.HttpMethod)
                        {
                            case "GET":
                            {
                                OnFailedGet?.Invoke(e);  
                                break;
                            }
                            case "POST":
                            {
                                OnFailedPost?.Invoke(e);
                                break;
                            }
                            case "PUT":
                            {
                                OnFailedPut?.Invoke(e);
                                break;
                            }
                        }
                    }
                }
            }, token);
        }

        protected virtual async Task HttpGet(HttpListenerContext listenerContext)
        {
            OnNonOverridenMethodInvocation?.Invoke(nameof(HttpGet));
            throw new NotImplementedException("Child class must override method 'HttpGet()!");
        }
        protected virtual async Task HttpPost(HttpListenerContext listenerContext, long length)
        {
            OnNonOverridenMethodInvocation?.Invoke(nameof(HttpPost));
            throw new NotImplementedException("Child class must override method 'HttpPost()!");
        }
        protected virtual async Task HttpPut(HttpListenerContext listenerContext, long length)
        {
            OnNonOverridenMethodInvocation?.Invoke(nameof(HttpPut));
            throw new NotImplementedException("Child class must override method 'HttpPut()!");
        }
        protected virtual async Task HttpPatch(HttpListenerContext listenerContext, long length)
        {
            OnNonOverridenMethodInvocation?.Invoke(nameof(HttpPatch));
            throw new NotImplementedException("Child class must override method 'HttpPatch()!");
        }

        private readonly HttpListener Listener;
        public event Action<string> OnCompleteGet;
        public event Action<string, string, (long input, long output)> OnComplete_Full;
        public event Action<Exception> OnFailedGet;
        public event Action<string> OnCompletePost;
        public event Action<Exception> OnFailedPost;
        public event Action<string> OnCompletePut;
        public event Action<Exception> OnFailedPut;
        public event Action<string> OnNonOverridenMethodInvocation;
    }
}