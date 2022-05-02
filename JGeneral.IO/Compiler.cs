using System.CodeDom.Compiler;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CSharp;

namespace JGeneral.IO
{
    public static class Compiler
    {
        public static string Compile(string sourceDirectoryPath, string dependencyDirectoryPath, string libraryName, string outputDirectory = null, bool isExe = false)
        {
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            ICodeCompiler icc = codeProvider.CreateCompiler();
            var files = Directory.GetFiles(sourceDirectoryPath);
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateInMemory = false;
            parameters.GenerateExecutable = isExe;
            parameters.OutputAssembly = libraryName;
            parameters.ReferencedAssemblies.AddRange(Directory.GetFiles(dependencyDirectoryPath).Where(x => x.EndsWith(".dll")).ToArray());
            CompilerResults results = icc.CompileAssemblyFromFileBatch(parameters, files);

            return results.PathToAssembly;
        }

        public static void Run(this string path)
        {
            var ass = Assembly.ReflectionOnlyLoadFrom(path);
            ass.Modules.FirstOrDefault()!.GetMethod("Main").Invoke(null, new object[]{new string[]{}});
        }

        public static string CompileFromText(string text, string dll, bool exe = false)
        {
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            ICodeCompiler icc = codeProvider.CreateCompiler();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateExecutable = exe;
            parameters.OutputAssembly = dll;
            CompilerResults results = icc.CompileAssemblyFromSource(parameters, text);
            return results.PathToAssembly;
        }
    }
}