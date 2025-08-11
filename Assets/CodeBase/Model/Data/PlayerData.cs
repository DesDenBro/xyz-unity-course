using PixelCrew.GameObjects;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Model
{
    [Serializable]
    public class PlayerData
    {
        public Dictionary<string, Vector3> PositionOnLevel;
        public int Coins;
        public int Keys;
        public int Throws;
        public int MaxHealth;
        public int Health;
        public bool IsArmed;
        public Weapon Weapon;

        public InventoryData Inventory;
    }
}
