using UnityEngine;
using Zenject;

namespace Features.Resources
{
    public class ApplicationPauseSaveBridge : MonoBehaviour
    {
        private ResourceSaveService _saveService;

        [Inject]
        public void Construct(ResourceSaveService saveService)
        {
            _saveService = saveService;
        }

        private void OnApplicationPause(bool paused)
        {
            if (paused)
            {
                Debug.Log("App paused - triggering save");
                _saveService.Save();
            }
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
            {
                Debug.Log("App lost focus - triggering save");
                _saveService.Save();
            }
        }
    }
}