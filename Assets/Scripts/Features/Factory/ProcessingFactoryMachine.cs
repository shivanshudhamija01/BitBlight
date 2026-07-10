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
    public class ProcessingFactoryMachine : MonoBehaviour, IInteractable, IProgressProvider
    {
        [SerializeField]
        private ProcessingFactoryData factoryData;

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

        private ProcessingFactoryRecipeData FindRecipeToProduce()
        {
            foreach (var recipe in factoryData.recipes)
            {
                if (CanProduce(recipe))
                    return recipe;
            }

            return null;
        }

        private IEnumerator Produce(ProcessingFactoryRecipeData recipe)
        {
            _isProcessing = true;

            _signalBus.Fire(new FactoryStateChangedSignal(
                factoryData.id,
                FactoryState.Processing));

            float timer = 0;
            float duration = recipe.productionTime;

            // yield return new WaitForSeconds(recipe.productionTime);
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

        private bool CanProduce(ProcessingFactoryRecipeData recipe)
        {
            if (recipe == null || recipe.inputs == null)
                return false;

            var inputs = recipe.inputs;

            if (inputs.rawInput != null)
            {
                foreach (var input in inputs.rawInput)
                {
                    if (!_resourceReader.Has(input.type, input.amount))
                        return false;
                }
            }

            if (inputs.processedInput != null)
            {
                foreach (var input in inputs.processedInput)
                {
                    if (!_resourceReader.Has(input.type, input.amount))
                        return false;
                }
            }

            return true;
        }

        private void ConsumeInputs(ProcessingFactoryRecipeData recipe)
        {
            var inputs = recipe.inputs;

            if (inputs.rawInput != null)
            {
                foreach (var input in inputs.rawInput)
                    _resourceWriter.Remove(input.type, input.amount);
            }

            if (inputs.processedInput != null)
            {
                foreach (var input in inputs.processedInput)
                    _resourceWriter.Remove(input.type, input.amount);
            }
        }

        private void ProduceOutputs(ProcessingFactoryRecipeData recipe)
        {
            var output = recipe.processedOutputs;

            _resourceWriter.Add(output.type, output.amount);

            _signalBus.Fire(new ProcessedMaterialProducedSignal(
                output.type,
                output.amount));
            Debug.Log(output.type + ": " + output.amount);
        }
    }

    #endregion
}