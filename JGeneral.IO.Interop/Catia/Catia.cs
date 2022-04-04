using System.Runtime.InteropServices;

namespace JGeneral.IO.Interop.Catia
{
    public static class Catia
    {
        public static readonly INFITF.Application Instance = (INFITF.Application)Marshal.GetActiveObject("Catia.Application");
    }
}