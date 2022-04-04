namespace JGeneral.IO.Interop.Catia.Window
{
    public static class WindowHelper
    {
        public static CatiaWindow FromWindow(this INFITF.Window window)
        {
            return new CatiaWindow(window);
        }
    }
}