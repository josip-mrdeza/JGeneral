using JGeneral.IO.Net.V2.Services;

namespace JGeneral.IO.Net.V2
{
    public static class NetworkBuilder
    {
        public static RouterBuilder CreateEmptyRouterBuilder()
        {
            return new RouterBuilder();
        }
    }
}