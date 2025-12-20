using UnityEngine;
using PixelCrew.UI.Perks;
using PixelCrew.UI.Widgets;
using PixelCrew.Components;
using PixelCrew.GameObjects.Creatures;
using PixelCrew.Utils.Disposables;

namespace PixelCrew.UI.Hud
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private ProgressBarWidget _progressBarWidget;
        [SerializeField] private ActivePerkWidget _activePerkWidget;
        [SerializeField] private HealthComponent _health; // может быть заранее задан у мобов

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        
        private PerksComponent _perks;

        private void Start()
        {
            var hero = FindObjectOfType<Hero>();
            if (hero != null) 
            {
                if (_health == null) _health = hero.GetComponent<HealthComponent>();
                if (_perks == null) _perks = hero.GetComponent<PerksComponent>();
            }

            if (_health != null)
            {
                _health.OnHealthChanged += OnHealhChanged;
                OnHealhChanged(_health.Health, 0);
            }

            if (_perks != null)
            {
                _trash.Retain(_perks.PerksData.Used.Subscribe((x, y) => OnPerkChanged()));
                OnPerkChanged();        
            }
        }

        private void OnHealhChanged(int newValue, int oldValue)
        {
            var value = (float)newValue/_health.MaxHealth;
            _progressBarWidget.SetProgress(value);
        }
        
        private void OnPerkChanged()
        {
            if (_activePerkWidget == null) return;
            _activePerkWidget.UpdateView();
        }


        private void OnDestroy()
        {
            if (_health != null)  _health.OnHealthChanged -= OnHealhChanged;;
           _trash.Dispose();
        }
    }
}