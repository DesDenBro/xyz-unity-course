using System;
using PixelCrew.Components;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Repositories;
using PixelCrew.Utils.Disposables;

namespace PixelCrew.Model.Data
{
    public class StatsModel : IDisposable
    {
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private readonly StatsComponent _statsComp;
        private readonly InventoryComponent _invComp;
        private ObservableProperty<StatId> _interfaceSelectedStat = new ObservableProperty<StatId>();

        public ObservableProperty<StatId> InterfaceSelectedStat => _interfaceSelectedStat;
        public event Action OnChanged;
        public event Action<StatId> OnUpgraded;
        
        public StatsModel(StatsComponent sc, InventoryComponent ic)
        {
            _statsComp = sc;
            _invComp = ic;
            _trash.Retain(_interfaceSelectedStat.Subscribe((x, y) => OnChanged?.Invoke()));
        }

        public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }

        public void LevelUp(StatId id)
        {
            var price = GetPriceToUpgrade(id, out bool isEnough);
            if (!isEnough) return;

            _invComp.InventoryData.Remove(price.ItemId, price.Count);
            _statsComp.StatsData.LevelUp(id);

            OnUpgraded?.Invoke(id);
            OnChanged?.Invoke();
        }

        public StatLevel GetLevelDef(StatId id, int level = -1)
        {
            if (level == -1) level = GetCurrentLevel(id);
            var def = DefsFacade.I.Stats.GetStat(id);
            return def.Levels.Length > level ? def.Levels[level] : default;
        }
        public float GetValue(StatId id, int level = -1) => GetLevelDef(id, level).Value;

        public int GetCurrentLevel(StatId id)
        {
            return _statsComp.StatsData.GetLevel(id);
        }

        public bool CanUpgrade(StatId id)
        {
            GetPriceToUpgrade(id, out bool isEnough);
            return isEnough;
        }

        private ItemWithCount GetPriceToUpgrade(StatId id, out bool isEnough)
        {
            isEnough = false;

            var def = DefsFacade.I.Stats.GetStat(id);
            var nextLevel = GetCurrentLevel(id) + 1;
            if (def.Levels.Length <= nextLevel) return default;
            
            var price = def.Levels[nextLevel].Price;
            isEnough = _invComp.InventoryData.IsEnough(price);
            return price;
        }

        public void Dispose()
        {
            _trash.Dispose();
        }
    }
}