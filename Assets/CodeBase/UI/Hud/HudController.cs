using System.Collections;
using System.Collections.Generic;
using PixelCrew.Components;
using PixelCrew.GameObjects.Creatures;
using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.UI.Widgets;
using UnityEngine;

namespace PixelCrew.UI.Hud
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private ProgressBarWidget _progressBarWidget;
        [SerializeField] private HealthComponent _health;

        private void Start()
        {
            if (_health == null)
            {
                var hero = FindObjectOfType<Hero>();
                if (hero != null) _health = hero.GetComponent<HealthComponent>();
            }
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
            if (_health == null) return;
            _health.OnHealthChanged -= OnHealhChanged;
        }
    }
}