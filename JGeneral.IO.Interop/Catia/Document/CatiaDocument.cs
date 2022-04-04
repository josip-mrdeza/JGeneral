using INFITF;
using JGeneral.IO.Interop.Catia.Window;

namespace JGeneral.IO.Interop.Catia.Document
{
    public sealed class CatiaDocument
    {
        private INFITF.Document _comObject;

        internal CatiaDocument(INFITF.Document document)
        {
            _comObject = document;
        }
        public Application Application
        {
            get => _comObject.Application;
        }
        public Cameras Cameras
        {
            get => _comObject.Cameras;
        }
        public CATBaseDispatch Parent
        {
            get => _comObject.Parent;
        }
        public string Path
        {
            get => _comObject.Path;
        }
        public bool Saved
        {
            get => _comObject.Saved;
        }
        public Selection Selection
        {
            get => _comObject.Selection;
        }

        public string FullName
        {
            get => _comObject.FullName;
        }

        public string Name
        {
            get => _comObject.get_Name();
            set => _comObject.set_Name(value);
        }
        public bool ReadOnly
        {
            get => _comObject.ReadOnly;
        }

        public bool SeeHiddenElements
        {
            get => _comObject.SeeHiddenElements;
        }

        public void Activate()
        {
            _comObject.Activate();
        }

        public void Close()
        {
            _comObject.Close();
        }

        public void Destructor()
        {
            _comObject.Destructor();
        }
        public void Save()
        {
            _comObject.Save();
        }

        public void SaveAs(string fileName)
        {
            _comObject.SaveAs(fileName);
        }

        public void CreateFilter(string filterName, string filterDefinition)
        {
            _comObject.CreateFilter(filterName, filterDefinition);
        }

        public void RemoveFilter(string filterName)
        {
            _comObject.RemoveFilter(filterName);
        }

        public void SetFilter(string filterName)
        {
            _comObject.set_CurrentFilter(filterName);
        }

        public void SetLayer(string layerName)
        {
            _comObject.set_CurrentLayer(layerName);
        }
        
        public void ExportData(string fileName, string format)
        {
            _comObject.ExportData(fileName, format);
        }
        /// <summary>
        /// No idea what this does.
        /// </summary>
        public void GetImpl()
        {
            _comObject.GetImpl();
        }

        public void SetImpl()
        {
            _comObject.SetImpl();
        }

        public CATBaseDispatch GetItem(string name)
        {
            return _comObject.GetItem(name);
        }

        public Workbench GetWorkbench(string workbenchName)
        {
            return _comObject.GetWorkbench(workbenchName);
        }

        public CatiaWindow NewWindow()
        {
            return _comObject.NewWindow().FromWindow();
        }

        public Reference CreateReference(string label)
        {
            return _comObject.CreateReferenceFromName(label);
        }
        
    }
}