using System;
using System.Collections.Generic;
using System.Text;
using Database;
using HarmonyLib;
using STRINGS;

namespace JGeneral.Mods.ONI
{
    public static class BuildingPatcher
    {
        private const string PrefixBuilding = "STRINGS.BUILDINGS.PREFABS.";
        private const string PrefixPlant = "STRINGS.CREATURES.SPECIES.";
        private const string PrefixSeed = "STRINGS.CREATURES.SPECIES.SEEDS.";
        private const string PrefixFood = "STRINGS.ITEMS.FOOD.";

        private const string SufixName = ".NAME";
        private const string SufixDesc = ".DESC";
        private const string SufixRecipeDesc = ".RECIPEDESC";
        private const string SufixEffect = ".EFFECT";

        private static readonly StringBuilder Builder = new StringBuilder();

        public static ItemId AddBuildingName(this ItemId buildingId, string displayName)
        {
            Builder.Append(PrefixBuilding).Append(buildingId.Id.ToUpperInvariant()).Append(SufixName);
            Strings.Add(Builder.ToString(), UI.FormatAsLink(displayName, buildingId.Id));
            Builder.Clear();

            return buildingId;
        }

        public static ItemId AddBuildingDescription(this ItemId buildingId, string description)
        {
            Builder.Append(PrefixBuilding).Append(buildingId.Id.ToUpperInvariant()).Append(SufixDesc);
            Strings.Add(Builder.ToString(), description);
            Builder.Clear();

            return buildingId;
        }

        public static ItemId AddBuildingEffect(this ItemId buildingId, string description)
        {
            Builder.Append(PrefixBuilding).Append(buildingId.Id.ToUpperInvariant()).Append(SufixEffect);
            Strings.Add(Builder.ToString(), description);
            Builder.Clear();

            return buildingId;
        }

        public static ItemId AddToPlanScreen(this ItemId buildingId, CategoryType category,
            string addAfterBuildingId = null)
        {
            var index = TUNING.BUILDINGS.PLANORDER.FindIndex(x => x.category == category.ToString());

            if (index == -1)
            {
                return buildingId;
            }

            if (!(TUNING.BUILDINGS.PLANORDER[index].data is IList<string> planOrderList))
            {
                return buildingId;
            }
			
            var neighborIdx = planOrderList.IndexOf(addAfterBuildingId);

            if (neighborIdx != -1)
            {
                planOrderList.Insert(neighborIdx + 1, buildingId);
            }
            else
            {
                planOrderList.Add(buildingId);
            }
            
            return buildingId;
        }

        public static ItemId AddToTech(this ItemId buildingId, TechBase tech)
        {
            AddBuildingToTechnology(tech.ToString(), buildingId);

            return buildingId;
        }

        public static ItemId AddToTech(this ItemId buildingId, TechDlc tech)
        {
            AddBuildingToTechnology(tech.ToString(), buildingId);

            return buildingId;
        }

        private static void AddBuildingToTechnology(string tech, ItemId buildingId)
        {
            Db.Get().Techs.Get(tech).unlockedItemIDs.Add(buildingId);
        }
    }
}