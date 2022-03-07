using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;

namespace JGeneral.IO.Net.CommandArguments
{
    public struct ProgramNamesArgs : IArgs
    {
        public Process[] GetNames() => Process.GetProcesses();
    }
}