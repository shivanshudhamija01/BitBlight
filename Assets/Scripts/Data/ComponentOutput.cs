using System;
using Data.Enums;

namespace Data
{
    [Serializable]
    public class ComponentOutput
    {
        public ComponentType type;
        public int amount;
    }
}