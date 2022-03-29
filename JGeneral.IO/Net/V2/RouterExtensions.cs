using JGeneral.IO.Net.V2.Services;

namespace JGeneral.IO.Net.V2
{
    public static class RouterExtensions
    {
        public static NetworkRouter AddService(this NetworkRouter router, NetworkService networkService)
        {
            router.Services.Add(networkService.Id, networkService);
            
            return router;
        }

        public static NetworkRouter RemoveService(this NetworkRouter router, string id)
        {
            router.Services.Remove(id);
            return router;
        }

        public static NetworkRouter ReplaceService(this NetworkRouter router, string id, NetworkService networkService)
        {
            router.RemoveService(id).AddService(networkService);

            return router;
        }
    }
}