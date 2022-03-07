using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JGeneral.Restarter
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string[] f1s = null;
            var processName = args[0].Replace(".exe", "");
            var process = Process.GetProcessesByName(processName).FirstOrDefault();
            var startArgs = process.StartInfo;
            startArgs.FileName = processName + ".exe";
            process.Kill();
            Task.Delay(1000).Wait();
            var path = AppDomain.CurrentDomain.BaseDirectory;
            {
                var files = Directory.GetFiles($"{path}Cache");
                f1s = files;
                for (int i = 0; i < files.Length; i++)
                {
                    try
                    {
                        var file = files[i].Replace('\\', '/');
                        Console.WriteLine("Processing: " + file);
                        if (file.Contains(".update"))
                        {
                            var localpath = $"{path}{file.Substring(file.LastIndexOf('/') + 1).Replace(".update", ".dll")}";
                            File.WriteAllBytes(localpath, File.ReadAllBytes(file)); 
                            Console.WriteLine($"Wrote file: {new FileInfo(file).Name} to path {localpath}");
                        }
                        else if (file.Contains(".binupdate"))
                        {                                         
                            var localpath = $"{path}{file.Substring(file.LastIndexOf('/') + 1).Replace(".binupdate", ".exe")}";
                            File.WriteAllBytes(localpath, File.ReadAllBytes(file));
                            //Console.WriteLine($"Wrote file: {file} to path {localpath}");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                //File.Delete(currentPath + "/.jversion");
            }
            Process.Start(startArgs);

            foreach (var cacheFile in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Cache"))
            {
                if (File.Exists(cacheFile))
                {
                    File.Delete(cacheFile); 
                }
            }
        }
    }
}