using System;
using PixelCrew.Model.Data;

namespace PixelCrew.Model
{
    [Serializable]
    public class PlayerData
    {
        public int Health;
        public bool IsCandleActive;

        public InventoryData Inventory;
        public PerksData Perks;
        public StatsData Stats;
    }
}
