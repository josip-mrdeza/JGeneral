using System;

namespace JGeneral.IO.Unity
{
    public static class Embed
    {
        public static UnityProcess Unity;
        
        /// <summary>
        /// Embeds Unity inside WPF or other similar context.
        /// Generates the <see cref="Unity"/> static variable required to run <see cref="StartUnityEmbedded"/>.
        /// </summary>
        /// <returns>A pointer to the unity window.</returns>
        public static IntPtr EmbedUnity(string appId, IntPtr panelHandlePointer)
        {
            Unity = UnityProcess.Generate(appId, panelHandlePointer);
            return Unity.Handle;
        }
        /// <summary>
        /// Starts the previously embedded <see cref="Unity"/> inside the window specified in <see cref="EmbedUnity"/>.
        /// </summary>
        /// <returns>Whether the function succeeded or failed (<see cref="Boolean"/>).</returns>
        /// <exception cref="Exception">UnityProcess cannot be null while starting it.</exception>
        public static bool StartUnityEmbedded()
        {
            try
            {
                if (Unity is null)
                {
                    throw new Exception("UnityProcess cannot be null while starting it.");
                }

                Unity.Start();
                Unity.WaitForInputIdle();
                UnityProcess.EnumChildWindows(Unity.PanelHandler, Unity.WindowEnum, IntPtr.Zero);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            } 
        }
    }
}