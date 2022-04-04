using System;

namespace JGeneral.Mods.ONI
{
    public enum SupportedContent
    {
        Vanilla,
        Dlc1,
        All
    }

    internal static class SupportedContentExtensions
    {
        internal static string AsUniqueId(this SupportedContent supportedContent)
        {
            return (supportedContent switch
            {
                SupportedContent.Vanilla => "VANILLA_ID",
                SupportedContent.Dlc1    => "EXPANSION1_ID",
                SupportedContent.All     => "ALL",
                _ => "ALL"
            });
        }
    }
}