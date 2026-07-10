using Data.Enums;
using UnityEngine;

namespace Data.ScriptableObjects.Resources
{
    [CreateAssetMenu(menuName = "Data/Processed Material")]
    public class ProcessedMaterialData : ScriptableObject
    {
        [Header("Identity")]
        public ProcessedMaterialType type;

        public string displayName;
        public Sprite icon;
    }
}