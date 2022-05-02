using System;
using System.Diagnostics;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using JGeneral.IO.Reflection;
using JGeneral.IO.Reflection.V2;

namespace JGeneralBenchmarks.IO
{
    [MemoryDiagnoser]
    public class ReflectionBenchmark
    {
        internal int Number = 420;
        internal string Number1 { get; set; } = "yes";

        private FieldModifier<int, ReflectionBenchmark> FieldModifier;
        private PropertyModifier<string, ReflectionBenchmark> PropertyModifier;

        public ReflectionBenchmark()
        {
            FieldModifier = new FieldModifier<int, ReflectionBenchmark>(this, "Number");
            
            PropertyModifier = new PropertyModifier<string, ReflectionBenchmark>(this, "Number1");
        }

        [Benchmark]
        public void GetField()
        {
            FieldModifier.Get();
        }

        [Benchmark]
        public void SetField()
        {
            FieldModifier.Set(420);
        }

        [Benchmark]
        public void GetProperty()
        {
            PropertyModifier.Get();
        }

        [Benchmark]
        public void SetProperty()
        {
            PropertyModifier.Set("420");
        }



    }
}