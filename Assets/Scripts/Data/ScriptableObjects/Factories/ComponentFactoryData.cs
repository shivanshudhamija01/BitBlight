using Data.ScriptableObjects.Recipes;
using UnityEngine;

namespace Data.ScriptableObjects.Factories
{
    [CreateAssetMenu(
        fileName = "FactoryData",
        menuName = "Data/Component Factory")]
    public class ComponentFactoryData : ScriptableObject
    {
        [Header("Identity")]
        public string id;

        public string displayName;

        [Header("Recipe")]
        public ComponentFactoryRecipeData[] recipes;
    }
}