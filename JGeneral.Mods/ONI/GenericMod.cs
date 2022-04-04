using System.IO;
using System.Text;

namespace JGeneral.Mods.ONI
{
    public class GenericMod : IMod
    {
        public string Title { get; }
        public string Description { get; }
        public string StaticID { get; }
        public ModInfo Info { get; }
        
        public void WriteInfos(string directory = null)
        {
            StringBuilder builder = new StringBuilder();
            if (directory is null)
            {
                builder.Append("/Mods/").Append(StaticID);
                directory = builder.ToString();
                builder.Clear();
            }

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            builder.Append("title: ").Append(Title).Append('\n');
            builder.Append("description: ").Append('\'').Append(Description).Append('\'').Append('\n');
            builder.Append("staticID: ").Append(StaticID);
            string modYaml = builder.ToString();
            builder.Clear();
            builder.Append(directory).Append("/mod.yaml");
            string modYamlPath = builder.ToString();
            builder.Clear();
            builder.Append("supportedContent: ").Append(Info.Supported.AsUniqueId());
            builder.Append("minimumSupportedBuild: ").Append(Info.MinimumSupportedBuild);
            builder.Append("version: ").Append(Info.Version);
            builder.Append("APIVersion: ").Append(Info.APIVersion);
            string modInfoYaml = builder.ToString();
            builder.Clear();
            builder.Append(directory).Append("/mod_info.yaml");
            string modInfoPath = builder.ToString();
            builder.Clear();
            File.WriteAllText(modYamlPath, modYaml);
            File.WriteAllText(modInfoPath, modInfoYaml);
        }

        public void Build()
        {
            WriteInfos();
        }
    }
}