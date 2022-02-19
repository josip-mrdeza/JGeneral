using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JGeneral.IO
{
    public static class KeepMeAlive
    {
        private static List<ProcessStartInfo> Programs { get; set; }

        public static void Keep(Process process)
        {
            process.StartInfo.FileName = process.ProcessName.EndsWith(".exe") ? process.ProcessName : process.ProcessName + ".exe";
            process.StartInfo.CreateNoWindow = true;
            Programs.Add(process.StartInfo); 
        }

        public static void Keep(string processname)
        {
            var process = Process.GetProcessesByName(processname).FirstOrDefault();
            process.StartInfo.FileName = process.ProcessName.EndsWith(".exe") ? process.ProcessName : process.ProcessName + ".exe";
            process.StartInfo.CreateNoWindow = true;
            Programs.Add(process.StartInfo); 
        }
        
        public static void Start()
        {
            Programs ??= new List<ProcessStartInfo>();
            ThreadPool.QueueUserWorkItem(cb =>
            {
                while (true)
                {
                    try
                    {
                        var running = Process.GetProcesses().Select(x => x.ProcessName).ToList();
                        foreach (var program in Programs)
                        {
                            string fn = program.FileName.Replace(".exe", "");
                            if (!running.Exists(x => x == fn))
                            {
                                Console.WriteLine($"Noticed program: {program.FileName} was closed, starting again...");
                                Process.Start(program);
                            }
                        }

                        running = null;
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine($"Failed: {e.Message}!");
                    }
                    finally
                    {
                        Task.Delay(2500).Wait();
                    }
                }
            });
        }
    }
}