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

        private HealthComponent _heroHealth;

        private void Start()
        {
            var hero = FindObjectOfType<Hero>();
            if (hero != null) _heroHealth = hero.GetComponent<HealthComponent>();
            if (_heroHealth != null)
            {
                _heroHealth.OnHealthChanged += OnHealhChanged;
                OnHealhChanged(_heroHealth.Health, 0);
            }
        }

        private void OnHealhChanged(int newValue, int oldValue)
        {
            var value = (float)newValue/_heroHealth.MaxHealth;
            _progressBarWidget.SetProgress(value);
        }

        private void OnDestroy()
        {
            _heroHealth.OnHealthChanged -= OnHealhChanged;
        }
    }
}