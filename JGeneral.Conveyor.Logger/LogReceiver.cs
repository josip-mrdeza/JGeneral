using System;
using JGeneral.Conveyors.Wrappers;

namespace JGeneral.Conveyors.Logger
{
    public static class LogReceiver
    {
        public static ConsoleColor Default;

        public static string Name;

        private static readonly Conveyor _consoleConveyor = Conveyor.CreateJConveyor("jlogreceiver", "joki_jlogsender");

        public static bool HasConnected;
        
        public static void StartListening()
        {
            Default = Console.ForegroundColor;
            _consoleConveyor.OnServerReceived += delegate(ConveyorObject o)
            {
                string line = o.Data as string;
                bool flag = line != null;
                if (flag)
                {
                    bool flag2 = line == "cls";
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
                    Console.Write("[" + DateTime.Now.ToLongTimeString() + "]");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(" -> ");
                    Console.ForegroundColor = Default;
                    Console.Write(line + "\n");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("[" + DateTime.Now.ToLongTimeString() + "]");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(" -> (UnknownDataType) -> ");
                    Console.ForegroundColor = Default;
                    Console.Write("[JSON-Format]\n");
                }
            };
            _consoleConveyor.OnServerConnect += delegate(string clientId)
            {
                bool flag = !HasConnected;
                if (flag)
                {
                    Console.Write("Server connected to ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(clientId + "!\n");
                    Console.WriteLine();
                    Console.ForegroundColor = Default;
                    HasConnected = true;
                    Name = clientId;
                }
            };
            Console.Read();
        }
    }
}