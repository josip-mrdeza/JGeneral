using System.Text;
using STRINGS;

namespace JGeneral.Mods.ONI
{
    public static class FoodPatcher
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
        
        public static ItemId AddFoodName(ItemId foodId, string displayName)
        {
            Builder.Append(PrefixSeed).Append(foodId.Id.ToUpperInvariant()).Append(SufixName);
            Strings.Add(Builder.ToString(), UI.FormatAsLink(displayName, foodId.Id));
            Builder.Clear();
                
            return foodId;
        }

        public static ItemId AddFoodDescription(ItemId foodId, string description)
        {
            Builder.Append(PrefixFood).Append(foodId.Id.ToUpperInvariant()).Append(SufixDesc);
            Strings.Add(Builder.ToString(), description);
            Builder.Clear();
                
            return foodId;
        }
            
        public static ItemId AddFoodRecipeDescription(ItemId foodId, string recipeDescription)
        {
            Builder.Append(PrefixFood).Append(foodId.Id.ToUpperInvariant()).Append(SufixRecipeDesc);
            Strings.Add(Builder.ToString(), recipeDescription);
            Builder.Clear();

            return foodId;
        }
    }
}