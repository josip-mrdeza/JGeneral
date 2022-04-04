using System;
using JGeneral.IO.Net.V2.Services;

namespace JGeneral.IO.Net.V2
{
    public class RouterBuilder
    {
        public NetworkRouter Router = new NetworkRouter();

        public RouterBuilder AddService(Type networkService)
        {
            if (!(Activator.CreateInstance(networkService) is NetworkService serviceInstance))
            {
                throw new Exception($"Activation of {networkService.Name}'s instance failed, perhaps the type does not derive from Type 'NetworkService'.");
            }
            Router.Services.Add(serviceInstance.Id, serviceInstance);
            OnAddedService?.Invoke(serviceInstance);
            return this;
        }

        public event Action<NetworkService> OnAddedService;
    }
}