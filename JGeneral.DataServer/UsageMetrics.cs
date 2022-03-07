using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using JGeneral.IO.Database;

namespace JGeneral.DataServer
{
    public struct UsageMetrics
    {
        public string CpuUsage { get; set; }
        public string GCTotalMemory { get; set; }
        public string GpuUsage { get; set; }

        public static PerformanceCounter CpuCounter;
        public static PerformanceCounter RamCounter;

        public static UsageMetrics GetNewData()
        {
            CpuCounter ??= new PerformanceCounter("Processor", "% Processor Time", "_Total", true);
            RamCounter ??= new PerformanceCounter("Memory", "Available MBytes", true);
            var um =  new UsageMetrics()
            {
                CpuUsage = Math.Round(CpuCounter.NextValue(), 2)                     + " %",
                GCTotalMemory = Math.Round(GC.GetTotalMemory(false) / 1_000_000f, 2) + " MB",
                GpuUsage = 0                                                         + " %"
            };
            GotNewUsageMetric?.Invoke(um);
            return um;
        }

        public override string ToString() => this.ToJson();

        public static event Action<UsageMetrics> GotNewUsageMetric;
    }
}