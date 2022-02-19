using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace JGeneral.IO.Unity
{
    public class UnityProcess : Process
    {
        public UnityProcess(string appId, IntPtr panelHandlePointer) : base()
        {
            StartInfo.FileName = Environment.CurrentDirectory + $"/{appId}";
            StartInfo.Arguments = $"-parentHWND {panelHandlePointer.ToInt32().ToString()} {Environment.CommandLine}";
            StartInfo.UseShellExecute = true;
            StartInfo.CreateNoWindow = true;
        }
        internal delegate int WindowEnumProc(IntPtr hwnd, IntPtr lparam);
        
        [DllImport("User32.dll")]
        public static extern bool MoveWindow(IntPtr handle, int x, int y, int width, int height, bool redraw);
        [DllImport("user32.dll")]
        internal static extern bool EnumChildWindows(IntPtr hwnd, WindowEnumProc func, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
        public void ActivateUnityWindow()
        {
            SendMessage(UnityWindow, WM_Activate, WA_Active, IntPtr.Zero);
        }

        public void DeactivateUnityWindow()
        {
            SendMessage(UnityWindow, WM_Activate, WA_Inactive, IntPtr.Zero);
        }

        public int WindowEnum(IntPtr hwnd, IntPtr lparam)
        {
            UnityWindow = hwnd;
            ActivateUnityWindow();
            return 0;
        }
        
        public IntPtr UnityWindow = IntPtr.Zero;
        public const int WM_Activate = 0x0006;
        public readonly IntPtr WA_Active = new IntPtr(1);
        public readonly IntPtr WA_Inactive = new IntPtr(0);
        public IntPtr PanelHandler;
        public string AppId;
        public static UnityProcess Generate(string appId, IntPtr panelHandlePointer)
        {
            UnityProcess process = new UnityProcess(appId, panelHandlePointer);
            process.AppId = appId;
            process.PanelHandler = panelHandlePointer;
            return process;
        }
    }
}