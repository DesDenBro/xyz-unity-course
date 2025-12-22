using System;
using PixelCrew.Model.Data;

namespace PixelCrew.Model
{
    [Serializable]
    public class PlayerData
    {
        public int MaxHealth;
        public int Health;

        public InventoryData Inventory;
        public PerksData Perks;
        public StatsData Stats;
    }
}
