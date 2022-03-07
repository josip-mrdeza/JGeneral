using System.Collections.Generic;
using System.IO;
using System.Reflection;
using JGeneral.IO.Database;

namespace JGeneral.IO.Updates
{
    /// <summary>
    /// Not subject to change, has to stay stable.
    /// </summary>
    internal class AppVersion
    {
        public int Key;
        public string Version;
        public string Name;

        public AppVersion()
        {
            var assembly = Assembly.GetEntryAssembly().GetName();
            Name = assembly.Name;
            Version = assembly.Version.ToString();
            Key = Name.GetHashCode();
            File.WriteAllText(UpdateClient.JsonLocation, new List<AppVersion>{this}.ToJson());
        }
        
        internal AppVersion(AssemblyName assemblyName)
        {
            Name = assemblyName.Name;
            Version = assemblyName.Version.ToString();
            Key = Name.GetHashCode();
            File.WriteAllText(UpdateClient.JsonLocation, new List<AppVersion>{this}.ToJson());
        }
    }
}