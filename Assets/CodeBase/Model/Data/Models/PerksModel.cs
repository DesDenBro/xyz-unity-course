using System;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Model.Definitions;

namespace PixelCrew.Model.Data
{
    public class PerksModel : IDisposable
    {
        private readonly PlayerData _data;

        public readonly StringProperty InterfaceSelection = new StringProperty();

        public PerksModel(PlayerData data)
        {
            _data = data;
        }

        public void Unlock(string id)
        {
            var def = DefsFacade.I.Perks.Get(id);
            var isEnough = _data.Inventory.IsEnough(def.Price);

            if (isEnough)
            {
                _data.Inventory.Remove(def.Price.ItemId, def.Price.Count);
                _data.Perks.AddPerk(id);
            }
        }

        public void Use(string id)
        {
            _data.Perks.Used.Value = id;
        }

        public void Dispose()
        {
            
        }

        public bool IsUsed(string id)
        {
            return _data.Perks.Used.Value == id;
        }

        public bool IsUnlocked(string id)
        {
            return _data.Perks.IsUnlocked(id);
        }
    }
}