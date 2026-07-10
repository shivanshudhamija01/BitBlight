using System.Collections.Generic;
using Data.Enums;
using UnityEngine;

namespace Data.ScriptableObjects.Resources
{
    [CreateAssetMenu(menuName = "Data/Raw Material")]
    public class RawMaterialData : ScriptableObject
    {
        [Header("Identity")]
        public RawMaterialType type;

        public string displayName;
        public Sprite icon;
    }
}