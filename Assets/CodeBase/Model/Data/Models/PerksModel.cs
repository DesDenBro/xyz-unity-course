using System;
using System.Net.Mime;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Model.Definitions;
using PixelCrew.Utils.Disposables;

namespace PixelCrew.Model.Data
{
    public class PerksModel : IDisposable
    {
        private readonly PlayerData _data;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        public event Action OnChanged;
        public readonly StringProperty InterfaceSelection = new StringProperty();
        public string Used => _data.Perks.Used.Value;

        public bool IsDoubleJumpUnlocked => _data.Perks.IsUsed("double-jump");
        public bool IsSuperThrowUnlocked => _data.Perks.IsUsed("super-throw");

        public PerksModel(PlayerData data)
        {
            _data = data;
            InterfaceSelection.Value = DefsFacade.I.Perks.All[0].Id;

            _trash.Retain(_data.Perks.Used.Subscribe((x, y) => OnChanged?.Invoke()));
            _trash.Retain(InterfaceSelection.Subscribe((x, y) => OnChanged?.Invoke()));
        }

        public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
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

            OnChanged?.Invoke();
        }

        public void Use(string id)
        {
            _data.Perks.Used.Value = id;
        }

        public void Dispose()
        {
            _trash.Dispose();
        }

        public bool IsUsed(string id)
        {
            return _data.Perks.Used.Value == id;
        }

        public bool IsUnlocked(string id)
        {
            return _data.Perks.IsUnlocked(id);
        }

        internal bool CanBuy(string perkId)
        {
            var def = DefsFacade.I.Perks.Get(perkId);
            return _data.Inventory.IsEnough(def.Price);
        }
    }
}