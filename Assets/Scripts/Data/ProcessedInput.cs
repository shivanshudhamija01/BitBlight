using System;
using Data.Enums;

namespace Data
{
    [Serializable]
    public class ProcessedInput
    {
        public ProcessedMaterialType type;
        public int amount;
    }
}