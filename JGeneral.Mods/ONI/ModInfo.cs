namespace JGeneral.Mods.ONI
{
    public struct ModInfo
    {
        public SupportedContent Supported { get; set; }
        public string MinimumSupportedBuild { get; set; }
        public string Version { get; set; }
        public byte APIVersion { get; set; }
    }
}