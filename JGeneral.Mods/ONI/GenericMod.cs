using System.IO;
using System.Linq;
using System.Text;
using JGeneral.Mods.ONI.Exceptions;

namespace JGeneral.Mods.ONI
{
    public class GenericMod : IMod
    {
        public string Title { get; }
        public string Description { get; }
        public string StaticID { get; }
        public ModInfo Info { get; set; }
        
        public void WriteInfos(string directory = null)
        {
            StringBuilder builder = new StringBuilder();
            if (directory is null)
            {
                builder.Append("Mods/").Append(StaticID);
                directory = builder.ToString();
                builder.Clear();
            }

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            var modYaml =     BuildYaml(builder);
            var modYamlPath = BuildYamlPath(directory, builder);
            WriteYaml(modYamlPath, modYaml);
            
            var modInfoYaml = BuildInfoYaml(builder);
            var modInfoPath = BuildInfoYamlPath(directory, builder);
            
            WriteInfoYaml(modInfoPath, modInfoYaml);
        }

        public void WriteInfoYaml(string modInfoPath, string modInfoYaml)
        {
            File.WriteAllText(modInfoPath, modInfoYaml);
        }

        public void WriteYaml(string modYamlPath, string modYaml)
        {
            File.WriteAllText(modYamlPath, modYaml);
        }

        public string BuildInfoYamlPath(string directory, StringBuilder builder)
        {
            builder.Append(directory).Append("/mod_info.yaml");
            string modInfoPath = builder.ToString();
            builder.Clear();

            return modInfoPath;
        }

        public string BuildInfoYaml(StringBuilder builder)
        {
            builder.Append("supportedContent: ").Append(Info.Supported).Append('\n');
            builder.Append("minimumSupportedBuild: ").Append(Info.MinimumSupportedBuild).Append('\n');
            builder.Append("version: ").Append(Info.Version).Append('\n');
            builder.Append("APIVersion: ").Append(Info.APIVersion);
            string modInfoYaml = builder.ToString();
            builder.Clear();

            return modInfoYaml;
        }

        public string BuildYamlPath(string directory, StringBuilder builder)
        {
            builder.Append(directory).Append("/mod.yaml");
            string modYamlPath = builder.ToString();
            builder.Clear();

            return modYamlPath;
        }

        public string BuildYaml(StringBuilder builder)
        {
            builder.Append("title: ").Append(Title).Append('\n');
            builder.Append("description: ").Append('\'').Append(Description).Append('\'').Append('\n');
            builder.Append("staticID: ").Append(StaticID);
            string modYaml = builder.ToString();
            builder.Clear();

            return modYaml;
        }

        public void Build()
        {
            WriteInfos();
        }

        private GenericMod(string title, string description, string staticId, ModInfo info)
        {
            Title = title;
            Description = description;
            StaticID = staticId;
            Info = info;
        }
        
        public static GenericMod Make(string title, string description, string staticId, ModInfo info)
        {
            staticId = staticId.ToUpper();
            if (info.Version.Count(x => x == '.') != 3)
            {
                throw new InvalidVersionStringException();
            }
            return new GenericMod(title, description, staticId, info);
        }
        
        public static GenericMod Make(string title, string description, string staticId, SupportedContent supportedContent,
            SupportedBuild minimumSupBuild, string version, APIVersion apiVersion)
        {
            return Make(title, description, staticId, new ModInfo(supportedContent, minimumSupBuild, version, apiVersion));
        }
    }
}