using System;
using PixelCrew.Components;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Model.Definitions;
using PixelCrew.Utils.Disposables;

namespace PixelCrew.Model.Data
{
    public class PerksModel : IDisposable
    {
        private readonly PerksComponent _perksComp;
        private readonly InventoryComponent _invComp;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        public event Action OnChanged;
        public readonly StringProperty InterfaceSelection = new StringProperty();
        public string Used => _perksComp.PerksData.Used.Value;

        public bool IsDoubleJumpUnlocked => _perksComp.PerksData.IsUsed("double-jump");
        public bool IsSuperThrowUnlocked => _perksComp.PerksData.IsUsed("super-throw");
        public bool IsStunUnlocked => _perksComp.PerksData.IsUsed("stun");

        public PerksModel(PerksComponent pc, InventoryComponent ic)
        {
            _perksComp = pc;
            _invComp = ic;
            InterfaceSelection.Value = DefsFacade.I.Perks.All[0].Id;

            _trash.Retain(_perksComp.PerksData.Used.Subscribe((x, y) => OnChanged?.Invoke()));
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
            var isEnough = _invComp.InventoryData.IsEnough(def.Price);

            if (isEnough)
            {
                _invComp.InventoryData.Remove(def.Price.ItemId, def.Price.Count);
                _perksComp.PerksData.AddPerk(id);
            }

            OnChanged?.Invoke();
        }

        public void Use(string id)
        {
            _perksComp.PerksData.Used.Value = id;
        }

        public void Dispose()
        {
            _trash.Dispose();
        }

        public bool IsUsed(string id)
        {
            return _perksComp.PerksData.IsUsed(id);
        }

        public bool IsUnlocked(string id)
        {
            return _perksComp.PerksData.IsUnlocked(id);
        }

        public bool IsPassive(string id)
        {
            return _perksComp.PerksData.IsPassive(id);
        }

        internal bool CanBuy(string perkId)
        {
            var def = DefsFacade.I.Perks.Get(perkId);
            return _invComp.InventoryData.IsEnough(def.Price);
        }
    }
}