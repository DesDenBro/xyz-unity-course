using System;
using PixelCrew.Components;
using PixelCrew.Model.Definitions;
using PixelCrew.Utils.Disposables;

namespace PixelCrew.Model.Data
{
    public class StatsModel : IDisposable
    {
        private readonly StatsComponent _statsComp;

        public event Action OnChanged;
        
        public StatsModel(StatsComponent sc)
        {
            _statsComp = sc;
        }

        public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }

        public void LevelUp(StatId id)
        {
            
        }

        public float GetValue(StatId id)
        {
            return 0f;
        }

        public int GetLevel(StatId id)
        {
            return _statsComp.StatsData.GetLevel(id);
        }


        public void Dispose()
        {
            
        }
    }
}