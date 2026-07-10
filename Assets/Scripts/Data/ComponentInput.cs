using System;
using Data.Enums;

namespace Data
{
    [Serializable]
    public class ComponentInput
    {
        public ComponentType type;
        public int amount;
    }
}