using System.Collections.Generic;
using System.Linq;
using Data.ScriptableObjects.Components;
using Data.ScriptableObjects.Factories;
using Data.ScriptableObjects.Orders;
using Data.ScriptableObjects.Resources;
using FairyGUI;
using Infrastructure.Signals;
using UI.Data;
using UI.Views;
using UnityEngine;
using Zenject;

namespace UI.Presenters
{
    public class RecipePresenter : IInitializable
    {
        private readonly RecipeView _recipeView;
        private readonly SignalBus _signalBus;

        //? Data
        private readonly ProcessingFactoryData[] _processingFactories;
        private readonly ComponentFactoryData[] _componentFactories;
        private readonly PCAssemblyOrdersData _orderPool;

        //? Icon Sources
        private readonly RawMaterialData[] _rawMaterials;
        private readonly ProcessedMaterialData[] _processedMaterials;
        private readonly ComponentData[] _components;

        private GList _gList;
        private List<RecipeDisplayData> _cachedData = new();


        public RecipePresenter(
            RecipeView recipeView,
            SignalBus signalBus,
            ProcessingFactoryData[] processingFactories,
            ComponentFactoryData[] componentFactories,
            PCAssemblyOrdersData orderPool,
            RawMaterialData[] rawMaterials,
            ProcessedMaterialData[] processedMaterials,
            ComponentData[] components)
        {
            _recipeView = recipeView;
            _signalBus = signalBus;
            _processingFactories = processingFactories;
            _componentFactories = componentFactories;
            _orderPool = orderPool;
            _rawMaterials = rawMaterials;
            _processedMaterials = processedMaterials;
            _components = components;
        }

        public void Initialize()
        {
            _recipeView.CreateUI();

            _recipeView.OnClose((() =>
            {
                _signalBus.Fire(new ButtonClick());
                _signalBus.Fire(new CloseMenuEvent());
            }));
            _gList = _recipeView.GetRecipeList();
            _gList.itemRenderer = RenderRecipeItem;
            _gList.SetVirtual();

            LoadAllRecipes();
        }

        private void LoadAllRecipes()
        {
            _cachedData.Clear();

            //? Processing Recipes
            foreach (var processingFactoryData in _processingFactories)
            {
                foreach (var recipe in processingFactoryData.recipes)
                {
                    var data = new RecipeDisplayData
                    {
                        Title = recipe.processedOutputs.type.ToString(),
                        OutputId = recipe.processedOutputs.type.ToString(),
                        OutputAmount = recipe.processedOutputs.amount,
                    };

                    if (recipe.inputs.rawInput != null)
                    {
                        foreach (var input in recipe.inputs.rawInput)
                        {
                            data.Ingredients.Add(new IngredientData
                            {
                                Id = input.type.ToString(),
                                Amount = input.amount
                            });
                        }
                    }

                    if (recipe.inputs.processedInput != null)
                    {
                        foreach (var input in recipe.inputs.processedInput)
                        {
                            data.Ingredients.Add(new IngredientData
                            {
                                Id = input.type.ToString(),
                                Amount = input.amount
                            });
                        }
                    }

                    _cachedData.Add(data);
                }
            }

            //? Component Recipes
            foreach (var componentFactoryData in _componentFactories)
            {
                foreach (var recipe in componentFactoryData.recipes)
                {
                    var data = new RecipeDisplayData
                    {
                        Title = recipe.componentOutputs[0].type.ToString(),
                        OutputId = recipe.componentOutputs[0].type.ToString(),
                        OutputAmount = recipe.componentOutputs[0].amount,
                    };
                    if (recipe.processedInputs != null)
                    {
                        foreach (var input in recipe.processedInputs)
                        {
                            data.Ingredients.Add(new IngredientData
                            {
                                Id = input.type.ToString(),
                                Amount = input.amount
                            });
                        }
                    }

                    _cachedData.Add(data);
                }
            }

            _gList.numItems = _cachedData.Count;
            _gList.RefreshVirtualList();
        }

        private void RenderRecipeItem(int index, GObject obj)
        {
            GComponent item = obj.asCom;
            var data = _cachedData[index];

            item.GetChild("Title").text = data.Title;

            GComponent outputItem = item.GetChild("OutputItem").asCom;

            outputItem.GetChild("Count").text = data.OutputAmount.ToString();

            GLoader outputLoader = outputItem.GetChild("Icon").asLoader;
            if (outputLoader != null)
            {
                Sprite sprite = GetSprite(data.OutputId);
                outputLoader.SetSize(60, 60);
                outputLoader.fill = FillType.ScaleFree;
                outputLoader.texture = sprite != null ? new NTexture(sprite) : null;
            }

            GList ingredientList = item.GetChild("IngredientList").asList;
            ingredientList.RemoveChildrenToPool();

            foreach (var ingredientData in data.Ingredients)
            {
                GComponent ingObj = ingredientList.AddItemFromPool().asCom;
                ingObj.GetChild("Count").text = ingredientData.Amount.ToString();

                GLoader ingLoader = ingObj.GetChild("Icon").asLoader;
                if (ingLoader != null)
                {
                    Sprite sprite = GetSprite(ingredientData.Id);
                    ingLoader.SetSize(80, 80);
                    ingLoader.fill = FillType.ScaleFree;
                    ingLoader.texture = sprite != null ? new NTexture(sprite) : null;
                }
            }
        }

        private Sprite GetSprite(string enumValue)
        {
            var raw = _rawMaterials.FirstOrDefault(m => m.type.ToString() == enumValue);
            if (raw != null) return raw.icon;

            var processed = _processedMaterials.FirstOrDefault(m => m.type.ToString() == enumValue);
            if (processed != null) return processed.icon;

            var component = _components.FirstOrDefault(m => m.id == enumValue);
            if (component != null) return component.icon;

            return null;
        }
    }
}