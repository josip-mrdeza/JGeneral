namespace JGeneral.IO.Threading
{
    public enum SyncThreadMode
    {
        /// <summary>
        /// Represents the usage of the thread that was used to invoke the currently executing method.
        /// </summary>
        Current,
        /// <summary>
        /// Represents the usage of the main process's thread.
        /// </summary>
        Main
    }
}