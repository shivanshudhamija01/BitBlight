using Data.ScriptableObjects.Resources;
using UnityEngine;

namespace Data.ScriptableObjects.Tasks
{
    [CreateAssetMenu(
        fileName = "TaskData",
        menuName = "Data/Task")]
    public class TaskData : ScriptableObject
    {
        [Header("Identity")]
        public string id;

        public string displayName;

        [Header("Timing")]
        public float duration;

        [Header("Output (Randomized Type)")]
        public RawMaterialData[] possibleOutputs;

        public int producedAmount = 1;
    }
}