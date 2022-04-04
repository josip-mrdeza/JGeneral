using System;
using System.Linq;
using JGeneral.IO.Net.V2.Services;

namespace JGeneral.IO.Net.V2
{
    public static class RouterExtensions
    {
        public static NetworkRouter AddService(this NetworkRouter router, Type networkServiceDerivedType)
        {
            var networkService = Activator.CreateInstance(networkServiceDerivedType) as NetworkService;
            router.Services.Add(networkService.Id, networkService);
            return router;
        }
        [Obsolete]
        public static NetworkRouter RemoveService(this NetworkRouter router, string id)
        {
            router.Services.Remove(id);
            return router;
        }
        
        [Obsolete]
        public static NetworkRouter RemoveService(this NetworkRouter router, Type serviceTypeId)
        {
            return router;
        }
        [Obsolete]
        public static NetworkRouter ReplaceService(this NetworkRouter router, Type oldServiceType, Type newServiceType)
        {
            return router;
        }
    }
}