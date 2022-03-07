using System.Diagnostics;
using System.Linq;

namespace JGeneral.IO.Net.CommandArguments
{
    public struct ProgramPIDArgs : IArgs
    {
        public (string, int)[] GetPIDs() => Process.GetProcesses().Select(x => (x.ProcessName, x.Id)).ToArray();
    }
}