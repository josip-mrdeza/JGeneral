using HarmonyLib;
using JGeneral.Mods.ONI;

public static class Patches
{
    [HarmonyPatch(typeof(Db), "Initialize")]
    public static class Fix
    {
        public const string ID = "SLUTJ";
        public const string DisplayName = "Slut Juicer";
        public static void Postfix()
        {
            BuildingPatcher.AddToTech(ID, TechBase.ImprovedCombustion);
        }
    }

    [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
    public static class Fix2
    {
        public static void Prefix()
        {
            BuildingPatcher.AddToPlanScreen(Fix.ID, CategoryType.Power).AddBuildingName(Fix.DisplayName).AddBuildingDescription("Empty Description").AddBuildingEffect("Empty Effect"); 
        }
    }
}