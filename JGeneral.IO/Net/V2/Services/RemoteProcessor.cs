using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using JGeneral.IO.Database;
using JGeneral.IO.Net.V2.Services.Helpers;

namespace JGeneral.IO.Net.V2.Services
{
    public sealed class RemoteProcessor : NetworkService
    {
        public RemoteProcessor()
        {
            Id = "rp";
        }

        protected override void Post(HttpListenerContext httpListenerContext, string[] resources, ref ContextInfo info)
        {
            object o = null;
            try
            {
                info.Code = 200;
            }
            catch (NullReferenceException nre)
            {
                info.Sent += nre.Message.WriteStringToOutput(httpListenerContext);
                info.Sent += ("Possible object source of error -> " + nre.Source).WriteStringToOutput(httpListenerContext);
                info.Code = 500;
            }
            catch (Exception e)
            {
                info.Sent += e.Message.WriteStringToOutput(httpListenerContext);
                info.Code = 500;
                Logger.Log(e);
            }
        }

        private readonly IFormatter BinaryFormatter = new BinaryFormatter();
    }
}