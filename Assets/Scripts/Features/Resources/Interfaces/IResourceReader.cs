using System;

namespace Features.Resources.Interfaces
{
    public interface IResourceReader
    {
        public bool Has<T>(T type, int amount) where T : Enum;
        public int GetAmount<T>(T type, int amount) where T : Enum;
    }
}