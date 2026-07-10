using FairyGUI;
using UnityEngine;

namespace UI
{
    [RequireComponent(typeof(UIPanel))]
    public class BuildingProgressBar : MonoBehaviour
    {
        private UIPanel _panel;
        private GProgressBar _bar;

        [Range(0, 1)]
        public float progress = 1f;

        void Start()
        {
            _panel = GetComponent<UIPanel>();

            GComponent ui = _panel.ui as GComponent;

            if (ui == null)
            {
                Debug.LogError("UI not loaded in UIPanel.");
                return;
            }

            _bar = ui.GetChild("Bar")?.asProgress;

            if (_bar == null)
            {
                Debug.LogError("ProgressBar child not found.");
            }
        }

        void LateUpdate()
        {
            if (_bar == null) return;

            _bar.value = progress * _bar.max;

            if (Camera.main != null)
                transform.forward = Camera.main.transform.forward;
        }
    }
}