using System;
using UnityEngine;

namespace PixelCrew.Model
{
    [Serializable]
    public class PlayerData
    {
        public int MaxHealth;
        public int Health;

        public InventoryData Inventory;
    }
}
