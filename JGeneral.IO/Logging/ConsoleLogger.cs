using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JGeneral.IO.Logging
{
    public class ConsoleLogger : ILogger
    {
        private static ConsoleColor DefaultConsoleColor;
        private static string NetworkAddress;
        public ConsoleLogger()
        {
            DefaultConsoleColor = Console.ForegroundColor;
            NetworkAddress = null;
            Reporter._logger = this;
        } 
        public ConsoleLogger(string networkAddress)
        {
            DefaultConsoleColor = Console.ForegroundColor;
            NetworkAddress = networkAddress;
            Reporter._logger = this;
        }
        
        public void Log(in object o, ILogger.LogType type = ILogger.LogType.Info, ConsoleColor infoColor = default)
        {
            StringBuilder builder = new StringBuilder();
            
            builder.Append('[');
            builder.Append(DateTime.Now.ToLongTimeString());
            builder.Append(']');
            builder.Append(' ');
            
            switch (type)
            {
                case ILogger.LogType.Debug:
                {
                    builder.Append("Debug: ");
                    if (o is null)
                    {
                        builder.Append("Object provided was null!");
                    }
                    else
                    {
                        builder.Append(o);
                    }
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    break;
                }
                case ILogger.LogType.Error:
                {
                    Log(o as Exception);
                    break;
                }
                case ILogger.LogType.Info:
                {
                    builder.Append("Info:  ");
                    if (o is IEnumerable arr and not string)
                    {
                        builder.Append(arr.Cast<object>().Count());
                        builder.Append(" items in collection");
                    }
                    else
                    {
                        builder.Append(o);
                    }
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                }
                case ILogger.LogType.Network:
                {
                    //Work in progress.
                    if (NetworkAddress is null)
                    {                           
                        Log(new Exception("Field 'NetworkAddress' in class ConsoleLogger is null but the method 'Log' tried accessing it."));
                        Console.ForegroundColor = DefaultConsoleColor;
                        builder.Clear();
                        return;
                    }
                    builder.Append("Logged to a network address: ");
                    builder.Append('"');
                    builder.Append(NetworkAddress);
                    builder.Append('"');
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                }
            }

            builder.Append('\n');
            var str = builder.ToString();
            var ss = str.Split('\n');
            Console.WriteLine(ss[0]);
            Console.ForegroundColor = infoColor == default ? DefaultConsoleColor : ConsoleColor.Red;
            for (int i = 1; i < ss.Length; i++)
            {
                Console.WriteLine(ss[i]);
            }
            Console.ForegroundColor = DefaultConsoleColor;
            Reporter.Report(str);
            builder.Clear();
        }
        public void Log(Exception caughtException, string type = "N/A", string method = "N/A", object cause = null, bool remoteLog = false)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append('[');
            builder.Append(DateTime.Now.ToLongTimeString());
            builder.Append(']');
            builder.Append(' ');
            
            builder.Append("Error: ");
            builder.Append('"');
            builder.Append(caughtException.Message);
            builder.Append('"');
            builder.Append(", in type ");
            builder.Append(type);
            builder.Append(" at site ");
            builder.Append('\'');
            builder.Append(method);
            builder.Append('\'');
            builder.Append('\n');
            builder.Append($"            -> Stored value: '{cause}'.");
            Console.ForegroundColor = ConsoleColor.DarkRed;
            
            builder.Append('\n');
            var str = builder.ToString();
            Console.WriteLine(str);
            Console.ForegroundColor = DefaultConsoleColor;
            if (remoteLog)
            {
                Reporter.Report(str);
            }
            builder.Clear();
        }
    }
}