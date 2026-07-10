using Data.ScriptableObjects.Recipes;

namespace Features.Orders
{
    public interface IOrderService
    {
        public void StartOrder(PCAssemblyRecipeData order, int amount = 1);
    }
}