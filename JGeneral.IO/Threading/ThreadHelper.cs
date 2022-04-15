using System;
using System.Threading;
using JGeneral.IO.Logging;

namespace JGeneral.IO.Threading
{
    public static class ThreadHelper
    {
        private static readonly ConsoleLogger Logger = new ConsoleLogger();
        /// <summary>
        /// Creates a new background thread which starts when the <see cref="ThreadPool"/> has an available thread to give.
        /// </summary>
        /// <returns></returns>
        public static bool CreateDaemon(Action<object> function)
        {
            try
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(function));
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static SyncThread<TAction> CreateSyncDaemon<TAction>(TAction function) where TAction : MulticastDelegate
        {
            return new SyncThread<TAction>(null, Logger);
        }
        
    }
}