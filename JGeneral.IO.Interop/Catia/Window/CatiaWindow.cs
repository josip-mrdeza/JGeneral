using INFITF;

namespace JGeneral.IO.Interop.Catia.Window
{
    public class CatiaWindow
    {
        private INFITF.Window _comObject;
        
        internal CatiaWindow(INFITF.Window comWindow)
        {
            _comObject = comWindow;
        }                           
        /// <summary>
        /// Gets the currently shown name of the object in Catia.
        /// </summary>
        public string Name
        {
            get => _comObject.get_Name();
            set => _comObject.set_Name(value);
        }
        /// <summary>
        /// Gets the currently shown name of window frame in Catia.
        /// </summary>
        public string Caption
        {
            get => _comObject.get_Caption();
            set => _comObject.set_Caption(value);
        }

        public int Height
        {
            get => _comObject.Height;
            set => _comObject.Height = value;
        }
        public int Width
        {
            get => _comObject.Width;
            set => _comObject.Width = value;
        }

        public int Left
        {
            get => _comObject.Left;
            set => _comObject.Left = value;
        }
        public int Top    
        {
            get => _comObject.Top;
            set => _comObject.Top = value;
        }
        /// <summary>
        /// The window's parent of type <see cref="CATBaseDispatch"/>.
        /// </summary>
        public CATBaseDispatch Parent
        {
            get => _comObject.Parent;
        }
        /// <summary>
        /// Changes the state of the window from disabled to enabled, or nothing in the case that the window is already enabled or active.
        /// </summary>
        public void Activate()
        {
            _comObject.Activate();
        }
        /// <summary>
        /// Closes the current window and disposes?? resources.
        /// </summary>
        public void Close()
        {
            _comObject.Close();
        }
        /// <summary>
        /// Unknown function.
        /// </summary>
        public void PrintToFile(string fileName)
        {
            _comObject.PrintToFile(fileName);
        }
    }
}