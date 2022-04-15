namespace JGeneral.Mods.ONI
{
    internal static class SupportedContentHelper
    {
        internal static string ToUniqueId(this SupportedContent supportedContent)
        {
            return (supportedContent switch
            {
                SupportedContent.Vanilla => "VANILLA_ID",
                SupportedContent.Dlc     => "EXPANSION1_ID",
                SupportedContent.All     => "ALL",
                _                        => "ALL"
            });
        }
    }
}