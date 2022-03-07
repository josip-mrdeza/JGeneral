using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using JGeneral.IO.Database;

namespace JGeneral.IO.Updates
{
    public class UpdateClient
    {
        private static List<UpdateClient> _activeClients = new List<UpdateClient>();
        private static string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        internal static string JsonLocation = BaseDirectory + ".jversion";
        internal static string ServerUrl = File.ReadAllText(BaseDirectory + ".jserver");
        private AppVersion Version;
        private HttpClient _client;
        public UpdateClient()
        {
            Init(Assembly.GetCallingAssembly().GetName());
            _client = new HttpClient();
            //KeepMeAlive.Keep("CJGT.exe");
        }

        public static async Task<bool> TryUpdateAll()
        {
            try
            {
                for (var i = 0; i < _activeClients.Count; i++)
                {
                    await _activeClients[i].SaveLatest();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        private void Init(AssemblyName assemblyName)
        {
            if (!File.Exists(JsonLocation))
            {
                Version = new AppVersion();
                return;
            }

            Version = File.ReadAllText(JsonLocation).FromJsonText<List<AppVersion>>().FirstOrDefault(x => x.Name == assemblyName.Name) ?? new AppVersion(assemblyName);
        }
        /// <returns>Whether the version is valid or not.</returns>
        private async Task<bool> IsValid()
        {
            var remoteVersion = await _client.GetStringAsync($"{ServerUrl}/Version/{Version.Key}");
            return Version.Version == remoteVersion;
        }

        private async Task SaveLatest()
        {
            var path = BaseDirectory + $"Cache";
            var filePath = path      + $"/{Version.Name}.update";
            var data = await _client.GetByteArrayAsync($"{ServerUrl}/Data/{Version.Key}");
            Directory.CreateDirectory(path);
            var fs = File.Create(filePath);
            await fs.WriteAsync(data, 0, data.Length);
            fs.Close();
        }
        
    }
}