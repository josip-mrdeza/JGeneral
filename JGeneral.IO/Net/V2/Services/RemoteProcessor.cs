using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using JGeneral.IO.Database;

namespace JGeneral.IO.Net.V2.Services
{
    public sealed class RemoteProcessor : NetworkService
    {
        public RemoteProcessor()
        {
            Id = "rp";
        }

        protected override (int code, int received, int sent) Post(HttpListenerContext httpListenerContext)
        {
            int code = 0;
            int received = 0;
            int sent = 0;
            object o = null;
            try
            {
                code = 200;
            }
            catch (NullReferenceException nre)
            {
                sent += nre.Message.WriteStringToOutput(httpListenerContext);
                sent += ("Possible object source of error -> " + nre.Source).WriteStringToOutput(httpListenerContext);
                code = 500;
            }
            catch (Exception e)
            {
                sent += e.Message.WriteStringToOutput(httpListenerContext);
                code = 500;
            }
            return (code, received, sent);
        }

        private readonly IFormatter BinaryFormatter = new BinaryFormatter();
    }
}