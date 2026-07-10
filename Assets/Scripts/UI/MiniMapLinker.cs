using UnityEngine;
using Zenject;
using FairyGUI;
using UI.Views;

namespace UI
{
    public class MiniMapLinker : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private RenderTexture minimapTexture;

        private HUDView _hudView;
        private NTexture _nTexture;

        [Inject]
        public void Construct(HUDView hudView)
        {
            _hudView = hudView;
        }

        private void Start()
        {
            if (minimapTexture != null)
            {
                _nTexture = new NTexture(minimapTexture);

                var loader = _hudView.GetMiniMapLoader();
                if (loader != null)
                {
                    loader.texture = _nTexture;
                    loader.fill = FillType.ScaleFree;
                }
                else
                {
                    Debug.LogWarning("MiniMap loader not found in HUDView");
                }
            }
            else
            {
                Debug.LogWarning("Minimap RenderTexture is not assigned in the inspector.");
            }
        }
    }
}