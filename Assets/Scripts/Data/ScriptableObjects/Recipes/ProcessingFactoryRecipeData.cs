using System;
using System.Collections.Generic;
using Data.Enums;
using UnityEngine;

namespace Data.ScriptableObjects.Recipes
{
    [CreateAssetMenu(menuName = "Data/Processing Factory Recipe")]
    public class ProcessingFactoryRecipeData : ScriptableObject
    {
        [Header("Inputs (Raw Materials)")]
        public RecipeInputs inputs = new();


        [Header("Outputs (Processed Materials)")]
        public ProcessedOutput processedOutputs = new();

        [Header("Production")]
        public float productionTime = 5f;
    }

    [Serializable]
    public class RecipeInputs
    {
        public RawInput[] rawInput;
        public ProcessedInput[] processedInput;
    }
}