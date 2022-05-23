namespace JGeneral.Mods.ONI
{
    public struct ModInfo
    {
        internal string Supported { get; set; }
        internal string MinimumSupportedBuild { get; set; }
        internal string Version { get; set; }
        internal byte APIVersion { get; set; }

        public ModInfo(SupportedContent supportedContent, SupportedBuild minimumSupportedBuild, string version, APIVersion apiVersion)
        {
            Supported = supportedContent.ToUniqueId();
            MinimumSupportedBuild = ((ulong)minimumSupportedBuild).ToString();
            Version = version;
            APIVersion = (byte)apiVersion;
        }
    }
}