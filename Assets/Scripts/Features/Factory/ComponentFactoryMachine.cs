using System.Collections;
using Data.Enums;
using Data.ScriptableObjects.Factories;
using Data.ScriptableObjects.Recipes;
using Features.Interaction;
using Features.Resources;
using Features.Resources.Interfaces;
using Infrastructure.Signals;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Features.Factory
{
    public class ComponentFactoryMachine : MonoBehaviour, IInteractable, IProgressProvider
    {
        [SerializeField]
        private ComponentFactoryData factoryData;

        private IResourceWriter _resourceWriter;
        private IResourceReader _resourceReader;
        private SignalBus _signalBus;
        private bool _isProcessing;
        private float _currentProgress;

        public float CurrentProgress => _currentProgress;

        [Inject]
        private void Construct(
            IResourceWriter resourceWriter,
            IResourceReader resourceReader,
            SignalBus signalBus)
        {
            _resourceWriter = resourceWriter;
            _resourceReader = resourceReader;
            _signalBus = signalBus;
        }

        public void OnPlayerEnter()
        {
            TryStartProduction();
        }

        public void OnPlayerExit()
        {
            // old behavior preserved
        }

        public bool IsActive
        {
            get { return _isProcessing; }
        }

        public StationType StationType
        {
            get { return StationType.Factory; }
        }

        private void TryStartProduction()
        {
            if (_isProcessing)
                return;

            var recipe = FindRecipeToProduce();
            if (recipe == null)
                return;

            ConsumeInputs(recipe);
            StartCoroutine(Produce(recipe));
        }

        private ComponentFactoryRecipeData FindRecipeToProduce()
        {
            foreach (var recipe in factoryData.recipes)
            {
                if (CanProduce(recipe))
                    return recipe;
            }

            return null;
        }

        private IEnumerator Produce(ComponentFactoryRecipeData recipe)
        {
            _isProcessing = true;

            _signalBus.Fire(new FactoryStateChangedSignal(
                factoryData.id,
                FactoryState.Processing));

            // yield return new WaitForSeconds(recipe.productionTime);

            float timer = 0;
            float duration = recipe.productionTime;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                _currentProgress = Mathf.Clamp01(timer / duration);
                yield return null;
            }

            ProduceOutputs(recipe);

            _signalBus.Fire(new FactoryStateChangedSignal(
                factoryData.id,
                FactoryState.Idle));

            _isProcessing = false;

            TryStartProduction(); //? try again in case more materials exist
        }

        #region LOGIC

        private bool CanProduce(ComponentFactoryRecipeData recipe)
        {
            if (recipe == null)
                return false;

            var inputs = recipe.processedInputs;

            if (inputs != null)
            {
                foreach (var input in inputs)
                {
                    if (!_resourceReader.Has(input.type, input.amount))
                        return false;
                }
            }

            return true;
        }

        private void ConsumeInputs(ComponentFactoryRecipeData recipe)
        {
            var inputs = recipe.processedInputs;

            if (inputs != null)
            {
                foreach (var input in inputs)
                    _resourceWriter.Remove(input.type, input.amount);
            }
        }


        private void ProduceOutputs(ComponentFactoryRecipeData recipe)
        {
            var outputs = recipe.componentOutputs;
            if (outputs == null)
                return;
            foreach (var output in outputs)
            {
                _resourceWriter.Add(output.type, output.amount);
                _signalBus.Fire(new ComponentProducedSignal(
                    output.type,
                    output.amount));
            }
        }
    }

    #endregion
}