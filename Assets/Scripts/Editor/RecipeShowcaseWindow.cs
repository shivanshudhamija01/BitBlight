using Data.ScriptableObjects.Factories;
using Data.ScriptableObjects.Orders;
using Data.ScriptableObjects.Recipes;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class RecipeShowcaseWindow : EditorWindow
    {
        private Vector2 _scrollPos;

        [MenuItem("Bitblight/Production Overview")]
        public static void ShowWindow()
        {
            GetWindow<RecipeShowcaseWindow>("Production Overview");
        }

        private void OnGUI()
        {
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            EditorGUILayout.LabelField("Bitblight Production Chain", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            // 1. Raw Materials to Processed Materials
            DrawSectionHeader("1. Processing Factory (Ores -> Ingots)");
            var processingFactories = Resources.FindObjectsOfTypeAll<ProcessingFactoryData>();
            foreach (var factory in processingFactories)
            {
                foreach (var recipe in factory.recipes)
                {
                    DrawProcessingRecipe(recipe);
                }
            }

            // 2. Processed Materials to Components
            DrawSectionHeader("2. Component Factory (Ingots -> Parts)");
            var componentFactories = Resources.FindObjectsOfTypeAll<ComponentFactoryData>();
            foreach (var factory in componentFactories)
            {
                foreach (var recipe in factory.recipes)
                {
                    DrawComponentRecipe(recipe);
                }
            }

            // 3. Components to PC Assembly
            DrawSectionHeader("3. PC Assembly (Parts -> Orders)");
            var orderPools = Resources.FindObjectsOfTypeAll<PCAssemblyOrdersData>();
            foreach (var pool in orderPools)
            {
                foreach (var recipe in pool.recipes)
                {
                    DrawAssemblyRecipe(recipe);
                }
            }

            EditorGUILayout.EndScrollView();
        }

        private void DrawSectionHeader(string label)
        {
            GUI.backgroundColor = Color.cyan;
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField(label, EditorStyles.whiteBoldLabel);
            EditorGUILayout.EndVertical();
            GUI.backgroundColor = Color.white;
        }

        private void DrawProcessingRecipe(ProcessingFactoryRecipeData recipe)
        {
            EditorGUILayout.BeginVertical("helpbox");
            EditorGUILayout.LabelField($"Output: {recipe.processedOutputs.type} x{recipe.processedOutputs.amount}",
                EditorStyles.boldLabel);
            if (recipe.inputs.rawInput != null)
            {
                foreach (var input in recipe.inputs.rawInput)
                    EditorGUILayout.LabelField($"  - Requires Raw: {input.type} x{input.amount}");
            }

            EditorGUILayout.LabelField($"  Time: {recipe.productionTime}s");
            EditorGUILayout.EndVertical();
        }

        private void DrawComponentRecipe(ComponentFactoryRecipeData recipe)
        {
            EditorGUILayout.BeginVertical("helpbox");
            foreach (var output in recipe.componentOutputs)
                EditorGUILayout.LabelField($"Output Component: {output.type} x{output.amount}", EditorStyles.boldLabel);

            foreach (var input in recipe.processedInputs)
                EditorGUILayout.LabelField($"  - Requires Ingot: {input.type} x{input.amount}");

            EditorGUILayout.EndVertical();
        }

        private void DrawAssemblyRecipe(PCAssemblyRecipeData recipe)
        {
            EditorGUILayout.BeginVertical("helpbox");
            EditorGUILayout.LabelField($"Build: {recipe.displayName} ({recipe.pcId})", EditorStyles.boldLabel);
            foreach (var input in recipe.componentInputs)
                EditorGUILayout.LabelField($"  - Requires: {input.type} x{input.amount}");
            EditorGUILayout.EndVertical();
        }
    }
}