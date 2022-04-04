using System.Text;

namespace JGeneral.IO.Interop.Catia.Command
{
    public static class CatiaCommandHelper
    {
        public static string FromCommandToString(this CatiaCommand command)
        {
            StringBuilder b = new StringBuilder(command.ToString());
            b.Replace("___", "/").Replace("__", "-").Replace("_", " ");
            return b.ToString();
        }

        public static void Run(this CatiaCommand command)
        {
            Catia.Instance.StartCommand(command.FromCommandToString());
        }
    }
}