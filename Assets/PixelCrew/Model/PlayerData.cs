using PixelCrew.GameObjects;
using System;
using UnityEngine;

namespace PixelCrew.Model
{
    [Serializable]
    public class PlayerData
    {
        public Vector3 Position;
        public int Coins;
        public int Keys;
        public int MaxHealth;
        public int Health;
        public bool IsArmed;
        public Weapon Weapon;
    }
}
