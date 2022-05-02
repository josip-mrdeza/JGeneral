using System;
using System.Diagnostics;
using System.Reflection;
using BenchmarkDotNet.Attributes;
using JGeneral.IO.Reflection;
using JGeneral.IO.Reflection.V2;

namespace JGeneralCoreBenchmarks.IO
{
    [MemoryDiagnoser]
    public class ReflectionBenchmark
    {
        public string Number = "Bitch";

        private FieldModifier<string, ReflectionBenchmark> FieldModifier_Generic;
        
        private FieldModifier<object, ReflectionBenchmark> FieldModifier_NonGeneric;

        public ReflectionBenchmark()
        {
            FieldModifier_Generic = new FieldModifier<string, ReflectionBenchmark>(this, "Number");
            FieldModifier_NonGeneric = new FieldModifier<object, ReflectionBenchmark>(this, "Number");
        }
        
        [Benchmark]
        public void GetField_Generic()
        {
            try
            {
                FieldModifier_Generic.Get();
            }
            catch
            {
                //
            }
        }
        
        [Benchmark]
        public void SetField_Generic()
        {
            try
            {
                FieldModifier_Generic.Set("Bitch was set 1");
            }
            catch
            {
                //
            }
        }

        private static readonly FieldInfo Info = typeof(JGeneralCoreBenchmarks.IO.ReflectionBenchmark).GetField("Number", BindingFlags.Instance | BindingFlags.Public);
        
        [Benchmark]
        public void GetField_Core()
        {
            _ = Info.GetValue(this);
        }
        
        [Benchmark]
        public void GetField_NonGeneric()
        {
            try
            {
                FieldModifier_NonGeneric.Get();
            }
            catch
            {
                //
            }
        }
        
        [Benchmark]
        public void SetField_NonGeneric()
        {
            try
            {
                FieldModifier_NonGeneric.Set("Bitch was set 2");
            }
            catch
            {
                //
            }
        }
        
        [Benchmark]
        public void GetField_Normal()
        {
            _ = Number;
        }
        
        [Benchmark]
        public void SetField_Normal()
        {
            Number = "Bitch was set 3";
        }
        
        
        

    }
}