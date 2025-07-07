using PixelCrew.GameObjects;
using System;

namespace PixelCrew.Model
{
    [Serializable]
    public class PlayerData
    {
        public int Coins;
        public int Keys;
        public int MaxHealth;
        public int Health;
        public bool IsArmed;
        public Weapon Weapon;
    }
}
