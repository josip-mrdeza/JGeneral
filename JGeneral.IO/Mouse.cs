using System.Runtime.InteropServices;
using PInvoke;

namespace JGeneral.IO
{
    public static class Mouse
    {
        public static ScreenPoint GetPosition()
        {
            GetCursorPos(out POINT point);

            return new ScreenPoint(point.x, point.y);
        }

        public static (int x, int y) GetCursorPosition()
        {
            var pos = GetPosition();
            
            return new (pos.X,pos.Y);
        }
        
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);
    }
}