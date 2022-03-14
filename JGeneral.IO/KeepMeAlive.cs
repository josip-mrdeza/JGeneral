using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JGeneral.IO
{
    public static class KeepMeAlive
    {
        private static List<(string name, Process process)> Programs { get; set; }
        //private static List<Action<string, string>> ProgramFiles { get; set; }

        private static readonly Func<string, string, Process> _runner = (file, args) =>
        {
            var startInfo = new ProcessStartInfo(file, args);
            startInfo.UseShellExecute = true;
            startInfo.CreateNoWindow = false;
            var process = Process.Start(startInfo);
            Console.WriteLine($"Started file: {file}!");
            return process;
        };
        
        private static readonly Action<string, string> _keepFn = (filename, args) =>
        {
            var fn = filename.Replace(".exe", "");
            var processes = Process.GetProcesses();
            var process = processes.FirstOrDefault(x => x.MainModule.FileName.Contains(fn) || x.MainWindowTitle.Contains(fn) || x.MainWindowTitle.Contains(fn));
            process.Exited += (_, _) =>
            {
                StartAndKeepRunning(process.StartInfo.FileName, new []{args});
            };
        };

        public static void Keep(this Process process, string path)
        {
            process.StartInfo.FileName = process.ProcessName.EndsWith(".exe") ? process.ProcessName : process.ProcessName + ".exe";
            Programs?.Add((path, process));
            Console.WriteLine($"Keeping process {process.StartInfo.FileName}!");
        }

        public static Process StartAndKeepRunning(string file, params string[] args)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < args.Length; i++)
            {
                builder.Append(args[i]);
                builder.Append(" ");
            }
            var proc = _runner(file, builder.ToString());
            proc.Keep(file);
            return proc;
        }
        public static void StartService(int updateInterval = 1000)
        {
            Programs ??= new List<(string name, Process process)>();
            ThreadPool.QueueUserWorkItem(cb =>
            {
                while (true)
                {
                    try
                    {
                        var exited = Programs.Where(x => x.process.HasExited).ToArray();
                        Programs.RemoveAll(x => x.process.HasExited);
                        for (int i = 0; i < exited.Length; i++)
                        {
                            Programs.Add((exited[i].name, Process.Start(exited[i].name, exited[i].process.StartInfo.Arguments)));
                        }
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine($"Failed in keeping: {e.Message}!");
                    }
                    finally
                    {
                        Task.Delay(updateInterval).Wait();
                    }
                }
            });
        }
    }
}