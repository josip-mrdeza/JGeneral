using System;
using System.Runtime.InteropServices;

namespace JGeneral
{
    public static class Platform
    {
        public static OperatingSystem Current
        {
            get
            {
                switch (Environment.OSVersion.Platform)
                {
                    case PlatformID.Win32NT:
                        return OperatingSystem.Windows;
                    case PlatformID.Unix:
                        return OperatingSystem.Linux;
                    case PlatformID.MacOSX:
                        return OperatingSystem.MAC;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public static string CurrentDirectory => AppDomain.CurrentDomain.BaseDirectory.Replace('\\', '/');
        
    }
}