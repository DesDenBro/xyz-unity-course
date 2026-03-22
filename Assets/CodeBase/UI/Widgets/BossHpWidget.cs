using PixelCrew.Components;
using PixelCrew.Utils;
using PixelCrew.Utils.Disposables;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PixelCrew.UI.Widgets
{
    public class BossHpWidget : MonoBehaviour
    {
        [SerializeField] private HealthComponent _health;
        [SerializeField] private ProgressBarWidget _hpBar;
        [SerializeField] private CanvasGroup _canvasGroup;

        private float _maxHealth;

        private void Start()
        {
            _maxHealth = _health.MaxHealth;
            _health.OnHealthChanged += OnHpChanged;
        }

        public void ShowUI()
        {
            this.LerpAnimated(0, 1, 1, SetAlpha);
        }
        public void HideUI()
        {
            this.LerpAnimated(1, 0, 1, SetAlpha);
        }
        private void SetAlpha(float alpha)
        {
            _canvasGroup.alpha = alpha;
        }


        private void OnHpChanged(int hp, int _)
        {
            _hpBar.SetProgress(hp / _maxHealth);
        }

        private void OnDestroy()
        {
            _health.OnHealthChanged -= OnHpChanged;
        }
    }
}