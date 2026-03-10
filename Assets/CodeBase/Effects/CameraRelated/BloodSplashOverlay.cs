using PixelCrew.Components;
using PixelCrew.GameObjects;
using PixelCrew.GameObjects.Creatures;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.UI;

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

            var frameImage = _overlay.GetComponent<Image>();
            frameImage.color = new Color(frameImage.color.r, frameImage.color.g, frameImage.color.b, 0.7f);

            var hero = FindObjectOfType<Hero>();
            _healthComp = hero.GetComponent<HealthComponent>();
            _healthComp.OnHealthChanged += OnHPChanged;

            OnHPChanged(_healthComp.Health, 0);
        }

        private void OnHPChanged(int newValue, int _)
        {
            var hpNormalized = (float)newValue / _healthComp.MaxHealth;
            _animator.SetKeyVal(AnimationKeys.Hero.HPValue, hpNormalized);

            var overlayModifier = Mathf.Max(hpNormalized - 0.1f, 0.03f);
            _overlay.localScale = Vector3.one + _overScale * overlayModifier;
        }

        private void OnDestroy()
        {
            _healthComp.OnHealthChanged -= OnHPChanged;
        }
    } 
}
