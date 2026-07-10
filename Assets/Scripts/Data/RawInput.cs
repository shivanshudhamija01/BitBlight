using System;
using Data.Enums;

namespace Data
{
    [Serializable]
    public class RawInput
    {
        public RawMaterialType type;
        public int amount;
    }
}