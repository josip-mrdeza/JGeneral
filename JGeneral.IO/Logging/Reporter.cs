using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace JGeneral.IO.Logging
{
    public static class Reporter
    {
        private const string RemoteAddress = "https://atriplex.loophole.site/";
        private const string LocalAddress = "http://192.168.1.200/";
        private static string Name;
        private static HttpClient _client;
        internal static ConsoleLogger _logger;
        
        public static async void Report(string message)
        {
            try
            {
                if (_client is null)
                {
                    _client = new HttpClient
                    {
                        BaseAddress = new Uri(LocalAddress),
                        Timeout = new TimeSpan(0, 0, 5, 0)
                    };
                }

                if (!string.IsNullOrEmpty(Name))
                {
                    await _client.PostAsync($"report/{Name}", new StringContent(message));
                }
                else
                {
                    await _client.PostAsync($"report/UnknownUser", new StringContent(message));
                }
            }
            catch (Exception exception)
            {
                if (_logger is null)
                {
                    return;
                }
                _logger.Log(exception, nameof(Reporter), nameof(Report));
            }
        }

    }
}