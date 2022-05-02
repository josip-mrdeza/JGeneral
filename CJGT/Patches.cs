using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using HarmonyLib;
using UnityEngine;

namespace CJGT
{
    public static class Patches
    {
        public static WebClient client = new WebClient();
        [HarmonyPatch(typeof(CookingStation))]
        [HarmonyPatch("SpawnOrderProduct")]
        public class TropicalPacuPatch
        {
            public static void Postfix(List<GameObject> __result)
            {
                var gbName = __result.FirstOrDefault().PrefabID().Name;
                client.UploadStringAsync(new Uri("http://localhost:1407/"), $"A '{gbName}' has been fabricated in the Cooking Station.");
            }
        }
    }
}