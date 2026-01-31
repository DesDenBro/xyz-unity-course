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
        private readonly CompositeDisposable _trash = new CompositeDisposable();

        [SerializeField] private ProgressBarWidget _progressBarWidget;
        [SerializeField] private ActivePerkWidget _activePerkWidget;
        [SerializeField] private ActualItemInfoWidget _activeThrowableWidget;
        [SerializeField] private ActivePassivesWidget _activePassivesWidget;
        
        private HealthComponent _health;
        private PerksComponent _perks;
        private InventoryComponent _inv;

        private void Start()
        {
            var hero = FindObjectOfType<Hero>();
            if (hero != null) 
            {
                _health = hero.GetComponent<HealthComponent>();
                _perks = hero.GetComponent<PerksComponent>();
                _inv = hero.GetComponent<InventoryComponent>();
            }

            if (_health != null)
            {
                _health.OnHealthChanged += OnHealhChanged;
                OnHealhChanged(_health.Health, 0);
            }

            if (_perks != null)
            {
                _trash.Retain(_perks.PerksData.Used.Subscribe((x, y) => OnPerkChanged()));
                _trash.Retain(_perks.PerksData.ActivePassives.Subscribe((x, y) => OnPerkChanged()));
                OnPerkChanged();        
            }

            if (_inv != null)
            {
                _trash.Retain(_inv.InventoryData.SelectedThrowId.Subscribe((x, y) => OnSelectedThrowChanged()));
                OnSelectedThrowChanged();
            }
        }

        private void OnSelectedThrowChanged()
        {
            if (_activeThrowableWidget == null) return;
            _activeThrowableWidget.SetItem(_inv.InventoryData.SelectedThrowId.Value);
        }

        private void OnHealhChanged(int newValue, int oldValue)
        {
            var value = (float)newValue/_health.MaxHealth;
            _progressBarWidget.SetProgress(value);
        }
        
        private void OnPerkChanged()
        {
            if (_activePerkWidget != null) _activePerkWidget.UpdateView();
            if (_activePassivesWidget != null) _activePassivesWidget.UpdateView();
        }


        private void OnDestroy()
        {
            if (_health != null) _health.OnHealthChanged -= OnHealhChanged;;
           _trash.Dispose();
        }
    }
}