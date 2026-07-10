using System.Collections;
using Data.Enums;
using Data.ScriptableObjects.Orders;
using Data.ScriptableObjects.Recipes;
using Features.Interaction;
using Features.Resources;
using Features.Resources.Interfaces;
using Infrastructure.Signals;
using Interfaces;
using UnityEngine;
using Zenject;

namespace Features.Orders
{
    public class PCAssemblyMachine : MonoBehaviour, IInteractable, IProgressProvider
    {
        [SerializeField]
        private PCAssemblyOrdersData factoryData;

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
            TryStartAssembly();
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
            get { return StationType.PCAssembly; }
        }

        private void TryStartAssembly()
        {
            if (_isProcessing || factoryData == null)
                return;

            var recipe = FindRecipeToAssemble();
            if (recipe == null)
                return;

            ConsumeInputs(recipe);
            StartCoroutine(Assemble(recipe));
        }

        private PCAssemblyRecipeData FindRecipeToAssemble()
        {
            foreach (var recipe in factoryData.recipes)
            {
                if (CanAssemble(recipe))
                    return recipe;
            }

            return null;
        }

        private IEnumerator Assemble(PCAssemblyRecipeData recipe)
        {
            _isProcessing = true;

            _signalBus.Fire(new FactoryStateChangedSignal(
                factoryData.id,
                FactoryState.Processing));

            float timer = 0;
            float duration = recipe.assemblyTime;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                _currentProgress = Mathf.Clamp01(timer / duration);
                yield return null;
            }
            // yield return new WaitForSeconds(recipe.assemblyTime);

            _signalBus.Fire(new PCAssembledSignal(recipe.pcId));

            _signalBus.Fire(new FactoryStateChangedSignal(
                factoryData.id,
                FactoryState.Idle));

            _isProcessing = false;

            TryStartAssembly();
        }

        #region LOGIC

        private bool CanAssemble(PCAssemblyRecipeData recipe)
        {
            if (recipe == null || recipe.componentInputs == null)
                return false;

            foreach (var input in recipe.componentInputs)
            {
                if (!_resourceReader.Has(input.type, input.amount))
                    return false;
            }

            return true;
        }

        private void ConsumeInputs(PCAssemblyRecipeData recipe)
        {
            foreach (var input in recipe.componentInputs)
                _resourceWriter.Remove(input.type, input.amount);
        }

        #endregion
    }
}