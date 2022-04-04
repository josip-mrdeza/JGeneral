using System.Runtime.InteropServices;

namespace JGeneral.IO.Interop.Catia
{
    internal static class Catia
    {
        internal static readonly INFITF.Application Instance = (INFITF.Application)Marshal.GetActiveObject("Catia.Application");
    }
}