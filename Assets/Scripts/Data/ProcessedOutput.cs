using System;
using Data.Enums;

namespace Data
{
    [Serializable]
    public class ProcessedOutput
    {
        public ProcessedMaterialType type;
        public int amount;
    }
}