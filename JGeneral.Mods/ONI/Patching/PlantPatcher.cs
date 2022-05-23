using System.Text;
using STRINGS;

namespace JGeneral.Mods.ONI
{
    public static class PlantPatcher
    {
        private const string PrefixBuilding = "STRINGS.BUILDINGS.PREFABS.";
        private const string PrefixPlant = "STRINGS.CREATURES.SPECIES.";
        private const string PrefixSeed = "STRINGS.CREATURES.SPECIES.SEEDS.";
        private const string PrefixFood = "STRINGS.ITEMS.FOOD.";

        private const string SufixName   = ".NAME";
        private const string SufixDesc   = ".DESC";
        private const string SufixRecipeDesc = ".RECIPEDESC";
        private const string SufixEffect = ".EFFECT";

        private static readonly StringBuilder Builder = new StringBuilder();
        
        public static ItemId AddPlantName(this ItemId plantId, string displayName)
        {
            Builder.Append(PrefixPlant).Append(plantId.Id.ToUpperInvariant()).Append(SufixName);
            Strings.Add(Builder.ToString(), UI.FormatAsLink(displayName, plantId.Id));
            Builder.Clear();

            return plantId;
        }

        public static ItemId AddPlantDescription(this ItemId plantId, string description)
        {
            Builder.Append(PrefixPlant).Append(plantId.Id.ToUpperInvariant()).Append(SufixDesc);
            Strings.Add(Builder.ToString(), description);
            Builder.Clear();
                
            return plantId;
        }
        
        public static ItemId AddPlantEffect(this ItemId plantId, string description)
        {
            Builder.Append(PrefixPlant).Append(plantId.Id.ToUpperInvariant()).Append(SufixDesc);
            Strings.Add(Builder.ToString(), description);
            Builder.Clear();

            return plantId;
        }
        
        public static ItemId AddSeedName(this ItemId plantId, string displayName)
        {
            Builder.Append(PrefixSeed).Append(plantId.Id.ToUpperInvariant()).Append(SufixName);
            Strings.Add(Builder.ToString(), UI.FormatAsLink(displayName, plantId.Id));
            Builder.Clear();

            return plantId;
        }

        public static ItemId AddSeedDescription(this ItemId plantId, string description)
        {
            Builder.Append(PrefixSeed).Append(plantId.Id.ToUpperInvariant()).Append(SufixDesc);
            Strings.Add(Builder.ToString(), description);
            Builder.Clear();
                
            return plantId;
        }
    }
}