using Data.ScriptableObjects.Recipes;
using UnityEngine;

namespace Data.ScriptableObjects.Factories
{
    [CreateAssetMenu(
        fileName = "FactoryData",
        menuName = "Data/Processing Factory")]
    public class ProcessingFactoryData : ScriptableObject
    {
        [Header("Identity")]
        public string id;

        public string displayName;

        [Header("Recipe")]
        public ProcessingFactoryRecipeData[] recipes;
    }
}