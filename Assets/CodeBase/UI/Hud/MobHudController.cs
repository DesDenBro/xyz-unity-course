using UnityEngine;
using PixelCrew.UI.Widgets;
using PixelCrew.Components;

namespace PixelCrew.UI.Hud
{
    public class MobHudController : MonoBehaviour
    {
        [SerializeField] private ProgressBarWidget _progressBarWidget;
        [SerializeField] private HealthComponent _health;

        private void Start()
        {
            if (_health != null)
            {
                _health.OnHealthChanged += OnHealhChanged;
                OnHealhChanged(_health.Health, 0);
            }
        }

        private void OnHealhChanged(int newValue, int oldValue)
        {
            var value = (float)newValue/_health.MaxHealth;
            _progressBarWidget.SetProgress(value);
        }

        private void OnDestroy()
        {
            if (_health != null)
            { 
                _health.OnHealthChanged -= OnHealhChanged;
            }
        }
    }
}