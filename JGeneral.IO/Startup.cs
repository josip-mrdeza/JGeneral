using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace JGeneral.IO
{
    public static class Startup
    {
        private static string _lastAddedPath;
        public static void AddAsStartupApp(string overrideName = null)
        {
            var ass = Assembly.GetEntryAssembly(); 
            var module = ass.GetModules().FirstOrDefault().Name;
            overrideName ??= module;
            var prevPath = AppDomain.CurrentDomain.BaseDirectory.Replace('\\', '/') + module;
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + $"\\{overrideName.Replace(".exe", string.Empty)}.bat";
            File.WriteAllText(path, $"@echo off\nstart {prevPath}");
            _lastAddedPath = path;
        }

        public static void RemoveAsStartupApp()
        {
            if (_lastAddedPath is null)
            {
                var module = Assembly.GetCallingAssembly().GetModules().FirstOrDefault().Name; 
                var path = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + $"\\_{module.Replace(".exe", string.Empty)}.bat";
                File.Delete(path);
            }
            else
            {
                File.Delete(_lastAddedPath);
            }
        }

        public static void Restart(int in_time_ms)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory.Replace('\\', '/');
            var restarter = Directory.EnumerateFiles(path).FirstOrDefault(x => x.Contains("Restarter"));
            if (restarter != default)
            {
                Task.Delay(in_time_ms).Wait();
                Process.Start(restarter, $"{Process.GetCurrentProcess().ProcessName} 1");
                return;
            }
            throw new Exception("Cannot restart app with no Restarter.");
        }
    }
}