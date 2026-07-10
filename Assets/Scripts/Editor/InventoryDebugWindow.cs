using System;
using System.Collections.Generic;
using Features.Resources;
using Features.Resources.Interfaces;
using Interfaces;
using UnityEditor;
using UnityEngine;
using Zenject;

namespace Editor
{
    public class InventoryDebugWindow : EditorWindow
    {
        private IResourceWriter _inventory;
        private IDataPersistence _persistence; // Reference for saving

        [MenuItem("Bitblight/Inventory Debugger")]
        public static void ShowWindow()
        {
            GetWindow<InventoryDebugWindow>("Inventory Debugger");
        }

        private void OnGUI()
        {
            if (!Application.isPlaying)
            {
                EditorGUILayout.HelpBox("Please enter Play Mode to view the inventory.", MessageType.Info);
                return;
            }

            if (_inventory == null)
            {
                var sceneContext = FindObjectOfType<SceneContext>();
                if (sceneContext != null && sceneContext.Container != null)
                {
                    _inventory = sceneContext.Container.TryResolve<IResourceWriter>();
                    // Resolve the persistence service to allow manual saves from the window
                    _persistence = sceneContext.Container.TryResolve<IDataPersistence>();
                }
            }

            if (_inventory == null)
            {
                EditorGUILayout.HelpBox("ResourceInventory not found.", MessageType.Warning);
                return;
            }

            DrawControls();
            EditorGUILayout.Space();
            DrawInventorySection();
        }

        private void DrawControls()
        {
            EditorGUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Manual Save", GUILayout.Height(30)))
            {
                _persistence?.Save();
            }

            GUI.backgroundColor = Color.yellow;
            if (GUILayout.Button("Manual Load", GUILayout.Height(30)))
            {
                _persistence?.Load();
            }

            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndHorizontal();
        }

        private void DrawInventorySection()
        {
            EditorGUILayout.LabelField("Current Inventory Status", EditorStyles.boldLabel);

            // Access the private dictionary using reflection
            var field = typeof(IResourceWriter).GetField("_resources",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var resources = field?.GetValue(_inventory) as Dictionary<Type, Dictionary<Enum, int>>;

            if (resources == null || resources.Count == 0)
            {
                EditorGUILayout.HelpBox("Inventory is currently empty.", MessageType.Info);
                return;
            }

            foreach (var category in resources)
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField($"Category: {category.Key.Name}", EditorStyles.miniBoldLabel);

                foreach (var item in category.Value)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"{item.Key}", GUILayout.Width(150));
                    EditorGUILayout.LabelField($"{item.Value}", GUILayout.Width(50));

                    if (GUILayout.Button("+5", GUILayout.Width(40)))
                        _inventory.Add(item.Key, 5);

                    if (GUILayout.Button("-5", GUILayout.Width(40)))
                        _inventory.Remove(item.Key, 5);

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }
        }
    }
}