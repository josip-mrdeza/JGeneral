namespace JGeneral.Mods.ONI
{
    public class ModData
    {
        public string Directory { get; }
        public IMod Mod { get; }
        public string Title { get => Mod.Title; }
        public string Description { get => Mod.Description; }
        public string StaticId { get => Mod.StaticID; }
        public ModInfo Info { get => Mod.Info; }
        public ModType Type { get => Mod.Type; }

        public ModData(IMod mod)
        {
            Mod = mod;
        }
        
    }
}