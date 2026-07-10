using System;
using Data.Enums;
using UnityEngine;

namespace Data.ScriptableObjects.Recipes
{
    [CreateAssetMenu(menuName = "Data/Component Factory Recipe")]
    public class ComponentFactoryRecipeData : ScriptableObject
    {
        [Header("Inputs (Processed Materials)")]
        public ProcessedInput[] processedInputs;

        [Header("Outputs (Components)")]
        public ComponentOutput[] componentOutputs;

        [Header("Production")]
        public float productionTime = 8f;
    }
}