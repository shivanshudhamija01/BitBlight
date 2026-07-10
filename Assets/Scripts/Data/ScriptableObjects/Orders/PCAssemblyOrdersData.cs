using Data.ScriptableObjects.Recipes;
using UnityEngine;

namespace Data.ScriptableObjects.Orders
{
    [CreateAssetMenu(menuName = "Data/PC Assembly Factory")]
    public class PCAssemblyOrdersData : ScriptableObject
    {
        public string id;
        public string displayName;

        public PCAssemblyRecipeData[] recipes;
    }
}