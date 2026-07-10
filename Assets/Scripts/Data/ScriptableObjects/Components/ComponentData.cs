using UnityEngine;

namespace Data.ScriptableObjects.Components
{
    [CreateAssetMenu(
        fileName = "ComponentData",
        menuName = "Data/Component")]
    public class ComponentData : ScriptableObject
    {
        public string id;
        public string displayName;
        public Sprite icon;

        [TextArea]
        public string description;
    }
}