using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using JGeneral.IO.Logging;
using JGeneral.IO.Net.V2.Services.Helpers;

namespace JGeneral.IO.Net.V2.Services
{
    public class NetworkService
    {
        public string Id;
        public double TotalReceivedMb;
        public double TotalSentMb;
        public double TotalTransferredMb;
        public long SuccessfulRequests;
        public long FailedRequests;
        public bool IsOnline;   
        public ShortException LatestException;
        protected ConsoleLogger Logger = NetworkRouter.Logger;
        protected NetworkRouter RouterReference = NetworkRouter.Instance;

        public NetworkService()
        {
            TurnOn();
        }

        public void TurnOff()
        {
            IsOnline = false;
        }

        public void TurnOn()
        {
            IsOnline = true;
        }
        
        protected virtual void Get(HttpListenerContext httpListenerContext, string[] resources, ref ContextInfo info)
        {
            info.Code = (int)HttpStatusCode.Forbidden;
        }
        protected virtual void Post(HttpListenerContext httpListenerContext, string[] resources, ref ContextInfo info)
        {
            info.Code = (int)HttpStatusCode.Forbidden;
        }
        protected virtual void Put(HttpListenerContext httpListenerContext, string[] resources, ref ContextInfo info)
        {
            info.Code = (int)HttpStatusCode.Forbidden;
        }
        
        internal void _Get(HttpListenerContext context, string[] resources, ref ContextInfo info)
        {
            HandleRequest(context, resources, ref info, HandlerMethod.Get);
        }
        internal void _Post(HttpListenerContext context, string[] resources, ref ContextInfo info)
        {
            HandleRequest(context, resources, ref info, HandlerMethod.Post);
        }
        internal void _Put(HttpListenerContext context, string[] resources, ref ContextInfo info)
        {
            HandleRequest(context, resources, ref info, HandlerMethod.Put);
        }
        private void HandleRequest(HttpListenerContext context, string[] resources, ref ContextInfo info, HandlerMethod method)
        {
            if (!IsOnline)
            {
                info.Code = 503;
            }

            try
            {
                switch (method)
                {
                    case HandlerMethod.Get:
                    {
                        Get(context, resources, ref info);

                        break;
                    }
                    case HandlerMethod.Post:
                    {
                        Post(context, resources, ref info);

                        break;
                    }
                    case HandlerMethod.Put:
                    {
                        Put(context, resources, ref info);

                        break;
                    }
                    case HandlerMethod.Delete:
                    {
                        info.Code = 503;

                        break;
                    }
                    default:
                    {
                        info.Code = 503;

                        break;
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.Log(exception, this.GetType().Name, method.ToString());
                FailedRequests++;
                info.Code = 500;
            }
            finally
            {
                IncrementInfos(info);
            }
        }

        private void IncrementInfos(ContextInfo info)
        {
            var receivedMb = info.Received / 1_000_000d;
            var sentMb = info.Sent / 1_000_000d;
            TotalReceivedMb += receivedMb;
            TotalSentMb += sentMb;
            TotalTransferredMb += (receivedMb + sentMb);

            if (info.Code is 500)
            {
                FailedRequests++;
            }
            else
            {
                SuccessfulRequests++;
            }
        }
    }
}