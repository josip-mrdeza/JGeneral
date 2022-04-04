namespace JGeneral.IO.Interop.Catia.Dialog
{
    public class CatiaMessageBox
    {
        public CatiaMessageBox(string title, string contents, ButtonMap buttonMapping)
        {
            Title = title;
            Contents = contents;
            ButtonMapping = buttonMapping;
        }

        public ButtonResultMap Show()
        {
            return Catia.Instance.MsgBox(Contents, ButtonMapping.AsShort(), Title, string.Empty, 0).AsButtonResultMap();
        }

        private string Title;
        private string Contents;
        private ButtonMap ButtonMapping;
    }
}