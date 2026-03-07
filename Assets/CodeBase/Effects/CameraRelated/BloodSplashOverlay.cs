using System;
using System.Collections;
using System.Collections.Generic;
using PixelCrew.Components;
using PixelCrew.GameObjects;
using PixelCrew.GameObjects.Creatures;
using PixelCrew.Model;
using PixelCrew.Utils;
using PixelCrew.Utils.Disposables;
using UnityEngine;

namespace PixelCrew.Effects
{
    [RequireComponent(typeof(Animator))]
    public class BloodSplashOverlay : MonoBehaviour
    {
        [SerializeField] private Transform _overlay;

        private Animator _animator;
        private Vector3 _overScale;
        private HealthComponent _healthComp;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _overScale = _overlay.localScale;

            var hero = FindObjectOfType<Hero>();
            _healthComp = hero.GetComponent<HealthComponent>();
            _healthComp.OnHealthChanged += OnHPChanged;

            OnHPChanged(_healthComp.Health,0);
        }

        private void OnHPChanged(int newValue, int _)
        {
            var hpNormalized = (float)newValue / _healthComp.MaxHealth;
            _animator.SetKeyVal(AnimationKeys.Hero.HPValue, hpNormalized);

            var overlayModifier = Mathf.Max(hpNormalized - 0.3f, 0f);
            _overlay.localScale = Vector3.one + _overScale * overlayModifier;
        }

        private void OnDestroy()
        {
            _healthComp.OnHealthChanged -= OnHPChanged;
        }
    } 
}
