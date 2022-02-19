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
        public static void AddAsStartupApp(string overrideName = null)
        {
            var ass = Assembly.GetCallingAssembly(); 
            var module = ass.GetModules().FirstOrDefault().Name;
            overrideName ??= module;
            var prevPath = AppDomain.CurrentDomain.BaseDirectory.Replace('\\', '/') + module;
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + $"\\{overrideName.Replace(".exe", string.Empty)}.bat";
            File.WriteAllText(path, $"@echo off\nstart {prevPath}");
        }

        public static void RemoveAsStartupApp()
        {
            var module = Assembly.GetCallingAssembly().GetModules().FirstOrDefault().Name; 
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + $"\\_{module.Replace(".exe", string.Empty)}.bat";
            File.Delete(path);
        }

        public static void Restart(int in_time_ms)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory.Replace('\\', '/');
            var restarter = Directory.EnumerateFiles(path).FirstOrDefault(x => x.Contains("Restarter"));
            if (restarter != default)
            {
                Process.Start(restarter, $"{Process.GetCurrentProcess().ProcessName} {in_time_ms.ToString()}");
                return;
            }
            throw new Exception("Cannot restart app with no Restarter.");
        }
    }
}