using System;
using System.Reflection;
using JGeneral.IO.Reflection;

namespace JGeneral.IO.Net.CommandArguments
{
    public struct ReflectionScript : IArgs
    {
        public readonly StaticExecutor Executor;
        public ReflectionScript(string typeName, string memberId, string isPublic)
        {
            Type type = Type.GetType(typeName);

            Executor = isPublic.ToLower() switch
            {
                "true"  => new StaticExecutor(type, memberId),
                "false" => new StaticExecutor(type, memberId, BindingFlags.NonPublic | BindingFlags.Static),
                _       => throw new ArgumentOutOfRangeException()
            };
        }

        public void Run(params object[] args) => Executor.Run(args);
    }
}