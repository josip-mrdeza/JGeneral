using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;

namespace JGeneral.IO.Net.CommandArguments
{
    [JsonObject]
    public struct ProgramStartArgs : IArgs
    {
        public string PathToExe { get; private set; }
        public string Args { get; private set; }

        public ProgramStartArgs(string pathToExe, params string[] args)
        {
            PathToExe = pathToExe;
            string s = string.Empty;
            for (int i = 0; i < args.Length; i++)
            {
                s += args[i];
            }
            Args = s;
        }
        [JsonConstructor]
        public ProgramStartArgs(string pathToExe, string args)
        {
            PathToExe = pathToExe;
            Args = args;
        }

        public void CreateAndRun()
        {
            var random = new Random().Next();
            var path = $"{Environment.CurrentDirectory}/.temp/{random.ToString()}_temp.bat"; 
            Directory.CreateDirectory($"{Environment.CurrentDirectory}/.temp");
            File.WriteAllText(path, $"@echo off\nstart {PathToExe} {Args}");
            var pcs = Process.Start(path);
            pcs.Exited += (sender, args) =>
            {
                File.Delete(path);
            };
        }
    }
}