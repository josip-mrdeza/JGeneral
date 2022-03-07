using System;
using System.Runtime.InteropServices;

namespace JGeneral.IO
{
    public static class ProcessModifiers
    {
        [DllImport("Kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("User32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int cmdShow);

        private static IntPtr _windowPtr
        {
            get => GetConsoleWindow();
        }
        public static void Hide()
        {
            ShowWindow(_windowPtr, 0);
        } 
        
        public static void Show()
        {
            ShowWindow(_windowPtr, 1);
        } 
    }
}