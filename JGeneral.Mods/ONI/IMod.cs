using System.Text;

namespace JGeneral.Mods.ONI
{
    public interface IMod
    {
        public string Title { get; }
        public string Description { get;}
        public string StaticID { get;}
        public ModInfo Info { get; }
        public ModType Type { get; }

        public void WriteInfos(string directory);
        public void WriteInfoYaml(string modInfoPath, string modInfoYaml);
        public void WriteYaml(string modYamlPath, string modYaml);
        public string BuildInfoYamlPath(string directory, StringBuilder builder);
        public string BuildInfoYaml(StringBuilder builder);
        public string BuildYamlPath(string directory, StringBuilder builder);
        public string BuildYaml(StringBuilder builder);
        public void Build(string outputDirectory);
        public void BuildSource(string outputDirectory);
    }
    
}