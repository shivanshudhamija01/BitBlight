using UnityEngine;
using Interfaces;

namespace UI
{
    public class ProgressBarLinker : MonoBehaviour
    {
        [SerializeField]
        private GameObject providerObject;

        [SerializeField]
        private BuildingProgressBar progressBar;

        private IProgressProvider _provider;

        private void Awake()
        {
            if (providerObject != null)
                _provider = providerObject.GetComponent<IProgressProvider>();
        }

        private void Update()
        {
            if (_provider != null && progressBar != null)
            {
                progressBar.progress = _provider.CurrentProgress;
            }
        }
    }
}