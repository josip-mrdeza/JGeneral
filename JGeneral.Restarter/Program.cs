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
            var processName = args[0].Replace(".exe", "");
            var time = int.Parse(args[1]);

            Task.Delay(time).Wait();
            var process = Process.GetProcessesByName(processName).FirstOrDefault();
            if (process != null)
            {
                var startArgs = process.StartInfo;
                startArgs.FileName = processName + ".exe";
                process.Kill();
                Task.Delay(time).Wait();
                Process.Start(startArgs);
            }
        }
    }
}