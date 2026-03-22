using PixelCrew.Common;
using PixelCrew.Components;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.GameObjects
{
    public class BigSword : MonoBehaviour
    {
        [SerializeField] private bool _isActive;

        private Animator _animator;
        private ThingSpecification _thingspec;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _thingspec = GetComponent<ThingSpecification>();
        }

        public void InitAttack()
        {
            if (_isActive) return;

            _animator.SetKeyVal(AnimationKeys.Hero.Perks.TriggerBigSwordAttack);
        }

        public void ToggleActive()
        {
            _isActive = !_isActive;
        }

        public void DealDamage(GameObject go)
        {
            if (!_isActive) return;

            var hp = go.GetComponent<HealthComponent>();
            if (hp == null) return;

            hp.ApplyDamage(_thingspec.DamagePoints);
        }
    }
}