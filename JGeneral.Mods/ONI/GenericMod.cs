using System;
using System.IO;
using System.Linq;
using System.Text;
using JGeneral.IO;
using JGeneral.Mods.ONI.Exceptions;

namespace JGeneral.Mods.ONI
{
    public class GenericMod : IMod
    {
        public string Title { get; }
        public string Description { get; }
        public string StaticID { get; }
        public ModInfo Info { get; }
        public ModType Type { get; }
        
        public void WriteInfos(string directory = null)
        {
            StringBuilder builder = new StringBuilder();
            if (directory is null)
            {
                builder.Append(StaticID);
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

        public void Build(string outputDirectory)
        {
            WriteInfos(outputDirectory);
            CreateSourceTemplate();
            BuildSource(outputDirectory);
        }

        public void BuildSource(string outputDirectory)
        {
            StringBuilder builder = new();
            builder.Append(StaticID).Append("_Source");
            var sourceDirectory = builder.ToString();
            Directory.CreateDirectory(sourceDirectory);
            builder.Clear();

            
            builder.Append(Title).Replace(" ", string.Empty).Append(".dll");
            var path = Compiler.Compile(sourceDirectory, Environment.CurrentDirectory, builder.ToString(), "", false);
            var data = File.ReadAllBytes(path);
            builder.Clear();
            builder.Append(outputDirectory).Append('/').Append(path);
            File.WriteAllBytes(builder.ToString(), data);
            File.Delete(path);
            builder.Clear();
        }

        public void CreateSourceTemplate()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StaticID).Append("_Source");
            var directory = builder.ToString();
            Directory.CreateDirectory(directory);
            builder.Clear();

                switch (Type)
            {
                case ModType.Building:
                {
                    builder.Append(directory).Append("/Config.cs");
                    File.WriteAllText(builder.ToString(), File.ReadAllText("ONI/Source/Config.cs"));
                    builder.Clear();
                    break;
                }
                case ModType.Patch:
                {
                    builder.Append(directory).Append("/Patches.cs");
                    File.WriteAllText(builder.ToString(), File.ReadAllText("ONI/Source/Patches.cs"));
                    builder.Clear(); 
                    break;
                }
                case ModType.Element:
                {
                    break;
                }
                case ModType.Mixed:
                {
                    builder.Append(directory).Append("/Config.cs");
                    File.WriteAllText(builder.ToString(), File.ReadAllText("ONI/Source/Config.cs"));
                    builder.Clear();
                    builder.Append(directory).Append("/Patches.cs");
                    File.WriteAllText(builder.ToString(), File.ReadAllText("ONI/Source/Patches.cs"));
                    builder.Clear();
                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        private GenericMod(string title, string description, string staticId, ModType type, ModInfo info)
        {
            Title = title;
            Description = description;
            StaticID = staticId;
            Info = info;
            Type = type;
        }
        
        public static GenericMod Make(string title, string description, ModType type, ModInfo info)
        {
            var staticId = title.Replace(" ", string.Empty).ToUpperInvariant();
            if (info.Version.Count(x => x == '.') != 3)
            {
                throw new InvalidVersionStringException();
            }
            return new GenericMod(title, description, staticId, type, info);
        }
        
        public static GenericMod Make(string title, string description, ModType type, SupportedContent supportedContent,
            SupportedBuild minimumSupBuild, string version, APIVersion apiVersion)
        {
            return Make(title, description, type, new ModInfo(supportedContent, minimumSupBuild, version, apiVersion));
        }
    }
}