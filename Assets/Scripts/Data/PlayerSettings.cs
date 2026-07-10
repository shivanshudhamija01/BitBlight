using System;

namespace Data
{
    [Serializable]
    public class PlayerSettings
    {
        public float moveSpeed = 5f;
        public float fireRate = 0.5f;
        public int maxHealth = 100;
    }
}