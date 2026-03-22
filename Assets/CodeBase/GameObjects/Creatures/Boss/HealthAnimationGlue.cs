using PixelCrew.Components;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.GameObjects.Creatures.Boss
{
    public class HealthAnimationGlue : MonoBehaviour
    {
        [SerializeField] private HealthComponent _healthComp;
        [SerializeField] private Animator _animator;

        private void Awake()
        {
            _healthComp.OnHealthChanged += OnHPChanged;

            OnHPChanged(_healthComp.Health, 0);
        }

        private void OnHPChanged(int hp, int _)
        {
            _animator.SetKeyVal(AnimationKeys.Boss.HPValue, (float)hp);
        }

        private void OnDestroy()
        {
            _healthComp.OnHealthChanged -= OnHPChanged;
        }
    }
}
