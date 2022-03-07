using System;
using System.IO;
using Newtonsoft.Json;

namespace JGeneral.IO.Net.CommandArguments
{
    [JsonObject]
    public struct UpdateArgs : IArgs
    {
        [JsonRequired]
        public string DllId
        {
            get; 
            set;
        }
        [JsonRequired]
        public byte[] DllData { get; set; }
        
        [JsonConstructor]
        public UpdateArgs(string dllName, byte[] dllData)
        {
            DllId = dllName;
            DllData = dllData;
        }

        public void UpdateDll(bool isDll = true)
        {
            File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + $"Cache/{DllId}.{(isDll ? "update" : "binupdate")}", DllData);
        }
    }
}