using System;
using System.Collections;
using Data.Enums;
using UnityEngine;
using Zenject;
using Data.ScriptableObjects.Tasks;
using Features.Interaction;
using Features.Resources;
using Features.Resources.Interfaces;
using Infrastructure.Signals;
using Interfaces;
using Random = UnityEngine.Random;

namespace Features.Tasks
{
    public class TaskStation : MonoBehaviour, IInteractable, IProgressProvider
    {
        [SerializeField]
        private TaskData taskData;

        private IResourceWriter _resourceWriter;
        private SignalBus _signalBus;

        private bool _isPlayerPresent;
        private Coroutine _taskRoutine;


        private float _currentProgress;
        public float CurrentProgress => _currentProgress;

        [Inject]
        public void Construct(
            IResourceWriter resourceWriter,
            SignalBus signalBus)
        {
            _resourceWriter = resourceWriter;
            _signalBus = signalBus;
        }

        public void OnPlayerEnter()
        {
            _isPlayerPresent = true;
            _signalBus.Fire(new TaskStationStateChangedSignal(true));
            if (_taskRoutine == null)
            {
                _taskRoutine = StartCoroutine(DoTask());
            }
        }

        public void OnPlayerExit()
        {
            _isPlayerPresent = false;
            _signalBus.Fire(new TaskStationStateChangedSignal(false));
            if (_taskRoutine != null)
            {
                StopCoroutine(_taskRoutine);
                _taskRoutine = null;
            }

            _currentProgress = 0f;
        }

        public bool IsActive
        {
            get { return _isPlayerPresent; }
        }

        public StationType StationType
        {
            get { return StationType.TaskStation; }
        }

        private IEnumerator DoTask()
        {
            while (_isPlayerPresent)
            {
                float timer = 0;
                float duration = taskData.duration;

                while (timer < duration)
                {
                    if (!_isPlayerPresent) yield break;
                    timer += Time.deltaTime;
                    _currentProgress = Mathf.Clamp01(timer / duration);
                    yield return null;
                }

                _currentProgress = 0f;

                if (taskData.possibleOutputs == null || taskData.possibleOutputs.Length == 0)
                {
                    Debug.LogWarning($"{name} has no possible outputs assigned.");
                    continue;
                }

                var outputs = taskData.possibleOutputs[
                    Random.Range(0, taskData.possibleOutputs.Length)];

                _resourceWriter.Add(outputs.type, taskData.producedAmount);
            }

            _taskRoutine = null;
        }
    }
}