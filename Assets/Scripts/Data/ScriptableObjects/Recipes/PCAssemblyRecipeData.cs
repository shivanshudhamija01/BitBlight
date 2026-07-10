using UnityEngine;

namespace Data.ScriptableObjects.Recipes
{
    [CreateAssetMenu(
        fileName = "PCRecipeData",
        menuName = "Data/PC Recipe")]
    public class PCAssemblyRecipeData : ScriptableObject
    {
        public string pcId;
        public string displayName;

        public ComponentInput[] componentInputs;

        [Header("Assembly")]
        public float assemblyTime = 6f;
    }
}