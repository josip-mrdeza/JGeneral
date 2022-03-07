using System.Diagnostics;
using Newtonsoft.Json;

namespace JGeneral.IO.Net.CommandArguments
{
    public struct ProgramEndArgs : IArgs
    {
        public string ProgramName { get; set; }

        public ProgramEndArgs(string programName)
        {
            ProgramName = programName;
        }

        public void End()
        {
            Process.GetProcessesByName(ProgramName)[0].Kill();
        }
    }
}