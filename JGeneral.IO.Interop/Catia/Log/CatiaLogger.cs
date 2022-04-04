namespace JGeneral.IO.Interop.Catia.Log
{
    public static class CatiaLogger
    {
        /// <summary>
        /// Sets the status bar message in the lower left corner.
        /// </summary>
        /// <param name="message">The message to print.</param>
        public static void Log(string message)
        {
            Catia.Instance.ActivePrinter.Application.set_StatusBar(message);
        }
    }
}