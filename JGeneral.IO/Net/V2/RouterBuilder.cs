using System;
using System.Collections.Generic;
using JGeneral.IO.Net.V2.Services;

namespace JGeneral.IO.Net.V2
{
    public class RouterBuilder
    {
        public NetworkRouter Router = new NetworkRouter();
        
        public RouterBuilder AddService(NetworkService service)
        {
            Router.Services.Add(service.Id, service);
            return this;
        }
        
    }
}