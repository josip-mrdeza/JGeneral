namespace JGeneral.IO.Interop.Catia.Dialog
{
    public static class ButtonMapHelper
    {
        public static short AsShort(this ButtonMap map)
        {
            return (short) map;
        }

        public static short AsShort(this ButtonResultMap map)
        {
            return (short) map;
        }

        public static ButtonMap AsButtonMap(this short num)
        {
            return (ButtonMap) num;
        }

        public static ButtonResultMap AsButtonResultMap(this short num)
        {
            return (ButtonResultMap) num;
        }
    }
}