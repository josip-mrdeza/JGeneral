namespace JGeneral.Mods.ONI
{
    public interface IMod
    {
        public string Title { get; }
        public string Description { get;}
        public string StaticID { get;}
        public ModInfo Info { get; }

        public void WriteInfos(string directory);
    }
}