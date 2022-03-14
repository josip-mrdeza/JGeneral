using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using JGeneral.Conveyors.Wrappers;
using JGeneral.IO.Database;

namespace JGeneral.Conveyors.Logger
{
    public static class LogReceiver
    {
        public static ConsoleColor Default;

        public static string Name;

        //private static readonly Conveyor _consoleConveyor = Conveyor.CreateJConveyor("jlogreceiver", "joki_jlogsender");
        private static HttpListener _consoleConveyor = new HttpListener();

        public static bool HasConnected;
        
        public static void StartListening()
        {
            Default = Console.ForegroundColor;
            _consoleConveyor.Prefixes.Add($"http://localhost:{Conveyor.LocalPort.ToString()}/");
            _consoleConveyor.Start();
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        var context = _consoleConveyor.GetContext();
                        if (!HasConnected)
                        {
                            var id = context.Request.UserHostName;
                            Console.Write("Server connected to ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(id + "!\n");
                            Console.WriteLine();
                            Console.ForegroundColor = Default;
                            HasConnected = true;
                            Name = id;
                        }

                        if (context.Request.HttpMethod == "POST")
                        {
                            var bytes = new byte[context.Request.ContentLength64];
                            context.Request.InputStream.Read(bytes, 0, bytes.Length);
                            var s = Encoding.ASCII.GetString(bytes);
                            bool flag2 = s == "cls";
                            if (flag2)
                            {
                                Console.Clear();
                                Console.Write("Server connected to ");
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.Write(Name + "!\n");
                                Console.WriteLine();
                                Console.ForegroundColor = Default;
                                HasConnected = true;
                            }

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write($"[{DateTime.Now.ToLongTimeString()}]");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(" -> ");
                            Console.ForegroundColor = Default;
                            Console.Write(s + "\n");
                            context.Request.InputStream.Close();
                            context.Response.OutputStream.Close();
                        }
                    }
                    catch
                    {
                    }
                }
            });
            Console.Read();
        }
    }
}