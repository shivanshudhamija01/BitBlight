using System.Collections.Generic;

namespace UI.Data
{
    public class RecipeDisplayData
    {
        public string Title;
        public string OutputId;
        public int OutputAmount;
        public List<IngredientData> Ingredients = new();
    }

    public struct IngredientData
    {
        public string Id;
        public int Amount;
    }
}