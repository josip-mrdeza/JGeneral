using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using JGeneral.IO.Logging;

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
        private protected ConsoleLogger Logger = NetworkRouter.Logger;
        /// <summary>
        /// These handlers may be overriden by classes deriving from <see cref="NetworkService"/>. 
        /// </summary>
        /// <param name="handler">The function to be executed on new request of type <see cref="HandlerMethod"/>.</param>
        /// <param name="httpMethod">The request type.</param>
        public void AddHandler(Func<HttpListenerContext, (int code, int received, int sent)> handler, HandlerMethod httpMethod)
        {
            switch (httpMethod)
            {
                case HandlerMethod.Get:
                {
                    VirtualHandlers.Add(HandlerMethod.Get, handler);
                    CachedGet = handler;
                    break;
                }          
                case HandlerMethod.Post:
                {
                    VirtualHandlers.Add(HandlerMethod.Post, handler);
                    CachedPost = handler;
                    break;
                }                
                case HandlerMethod.Put:
                {
                    VirtualHandlers.Add(HandlerMethod.Put, handler);
                    CachedPut = handler;
                    break;
                }
                default:
                {
                    Logger.Log(new Exception($"Adding handlers to Http Method '{httpMethod.ToString()}' isn't allowed for NetworkService with id '{Id}'!"), 
                        Id.ToUpper() + "_(Unknown)", nameof(AddHandler));
                    break;
                }
            }
        }

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
        
        protected virtual (int code, int received, int sent) Get(HttpListenerContext httpListenerContext)
        {
            if (VirtualHandlers.ContainsKey(HandlerMethod.Get))
            {
                CachedGet ??= VirtualHandlers[HandlerMethod.Get];

                return CachedGet.Invoke(httpListenerContext);
            }
            return ((int)HttpStatusCode.Forbidden, 0, 0);
        }
        protected virtual (int code, int received, int sent) Post(HttpListenerContext httpListenerContext)
        {
            if (VirtualHandlers.ContainsKey(HandlerMethod.Post))
            {
                CachedPost ??= VirtualHandlers[HandlerMethod.Post];

                return CachedPost.Invoke(httpListenerContext);
            }
            return ((int)HttpStatusCode.Forbidden, 0, 0);
        }
        protected virtual (int code, int received, int sent) Put(HttpListenerContext httpListenerContext)
        {
            if (VirtualHandlers.ContainsKey(HandlerMethod.Put))
            {
                CachedPut ??= VirtualHandlers[HandlerMethod.Put];

                return CachedPut.Invoke(httpListenerContext);
            }
            return ((int)HttpStatusCode.Forbidden, 0, 0);
        }
        
        internal (int code, int received, int sent) _Get(HttpListenerContext context)
        {
            if (!IsOnline)
            {
                return (503, 0, 0);
            }
            
            try
            {
                var result = Get(context);
                
                var receivedMb = result.received / 1_000_000d;
                var sentMb = result.sent / 1_000_000d; 
                TotalReceivedMb += receivedMb;
                TotalSentMb += sentMb;
                TotalTransferredMb += (receivedMb + sentMb);

                TotalReceivedMb = Math.Round(TotalReceivedMb, 4);
                TotalSentMb = Math.Round(TotalSentMb, 4);
                TotalTransferredMb += (receivedMb + sentMb);
                TotalTransferredMb = Math.Round(TotalTransferredMb, 4);

                if (result.code is 500)
                {
                    FailedRequests++;
                }
                else
                {
                    SuccessfulRequests++;
                }
                return result;
            }
            catch (Exception e)
            {
                FailedRequests++;
                return ((int)HttpStatusCode.InternalServerError, 0, 0);
            }
        }
        internal (int code, int received, int sent) _Post(HttpListenerContext context)
        {
            if (!IsOnline)
            {
                return (503, 0, 0);
            }
            
            try
            {
                var result = Post(context);
                
                var receivedMb =result.received / 1_000_000d;
                var sentMb = result.sent / 1_000_000d; 
                TotalReceivedMb += receivedMb;
                TotalSentMb += sentMb;
                TotalTransferredMb += (receivedMb + sentMb);
                
                if (result.code is 500)
                {
                    FailedRequests++;
                }
                else
                {
                    SuccessfulRequests++;
                }
                return result;
            }
            catch (Exception e)
            {
                FailedRequests++;
                return ((int)HttpStatusCode.InternalServerError, 0, 0);
            }
        }
        internal (int code, int received, int sent) _Put(HttpListenerContext context)
        {
            if (!IsOnline)
            {
                return (503, 0, 0);
            }
            
            try
            {
                var result = Put(context);
                
                var receivedMb =result.received / 1_000_000d;
                var sentMb = result.sent / 1_000_000d; 
                TotalReceivedMb += receivedMb;
                TotalSentMb += sentMb;
                TotalTransferredMb += (receivedMb + sentMb);
                
                if (result.code is 500)
                {
                    FailedRequests++;
                }
                else
                {
                    SuccessfulRequests++;
                }
                return result;
            }
            catch (Exception e)
            {
                FailedRequests++;
                return ((int)HttpStatusCode.InternalServerError, 0, 0);
            }
        }

        private Dictionary<HandlerMethod, Func<HttpListenerContext, (int code, int received, int sent)>> VirtualHandlers = new ();
        private Func<HttpListenerContext, (int code, int received, int sent)> CachedGet;
        private Func<HttpListenerContext, (int code, int received, int sent)> CachedPost;
        private Func<HttpListenerContext, (int code, int received, int sent)> CachedPut;
    }
}