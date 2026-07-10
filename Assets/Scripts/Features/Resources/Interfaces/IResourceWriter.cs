using System;

namespace Features.Resources.Interfaces
{
    public interface IResourceWriter
    {
        public void Add<T>(T type, int amount) where T : Enum;
        public bool Remove<T>(T type, int amount) where T : Enum;
    }
}