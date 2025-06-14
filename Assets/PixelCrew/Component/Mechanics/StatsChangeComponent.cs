
using PixelCrew.Common;
using UnityEngine;

namespace PixelCrew.Components
{
    public class StatsChangeComponent : MonoBehaviour
    {
        private ThingSpecification _thingSpecification;

        public void Awake()
        {
            _thingSpecification = GetComponent<ThingSpecification>();
        }

        public void HealthChange(GameObject target)
        {
            if (_thingSpecification == null) return;

            var damage = _thingSpecification.Damage;
            var recover = _thingSpecification.Recovery;

            var healtComponent = target?.GetComponent<HealthComponent>();
            if (healtComponent == null) return;

            if (damage > 0) healtComponent.ApplyDamage(damage);
            if (recover > 0) healtComponent.RecoverHealth(recover);
        }

        public void MoneyChange(GameObject target)
        {
            if (_thingSpecification == null) return;

            var amount = _thingSpecification.Amount;

            var hero = target.GetComponent<Hero>();
            if (hero == null) return;

            hero.AddMoney(amount);
        }
    }
}
