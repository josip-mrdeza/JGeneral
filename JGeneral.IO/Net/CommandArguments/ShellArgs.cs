using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JGeneral.IO.Net.CommandArguments
{
    public struct ShellArgs : IArgs
    {
        public string Args { get; private set; }

        public ShellArgs(string args)
        {
            Args = args;
        }

        public void CreateAndRun()
        {
            var random = new Random().Next();
            var path = $"{Environment.CurrentDirectory}/.temp/{random.ToString()}_temp.bat"; 
            Directory.CreateDirectory($"{Environment.CurrentDirectory}/.temp");
            File.WriteAllText(path, $"{(Args.Replace(@"\n", "\n"))}");
            ProcessStartInfo si = new ProcessStartInfo(path);
            si.CreateNoWindow = true;
            var proc = Process.Start(path);
            Task.Run(() =>
            {
                proc!.WaitForExit();
                Task.Delay(2000).Wait();
                File.Delete(path);
            });
        }
    }
}