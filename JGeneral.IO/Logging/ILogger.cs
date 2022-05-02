using System;

namespace JGeneral.IO.Logging
{
    public interface ILogger
    {
        enum LogType
        {
            Debug,
            Error,
            Info,
            Network
        }

        enum LogInfoAction
        {
            Accessed,
            Created,
            Deleted,
            Modified
        }

        public void Log(in object o, ILogger.LogType type = ILogger.LogType.Debug,
            ConsoleColor infoColor = ConsoleColor.DarkGreen, bool reporterLog = false);
    }
}