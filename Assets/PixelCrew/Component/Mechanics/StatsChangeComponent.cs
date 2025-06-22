
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

            var healtComponent = target?.GetComponent<HealthComponent>();
            if (healtComponent == null) return;

            var damagePoints = _thingSpecification.DamagePoints;
            if (damagePoints > 0) healtComponent.ApplyDamage(damagePoints);

            var healthPoints = _thingSpecification.HealthPoints;
            if (healthPoints > 0) healtComponent.RecoverHealth(healthPoints);
        }

        public void MoneyChange(GameObject target)
        {
            if (_thingSpecification == null) return;

            var inventory = target.GetComponent<InventoryComponent>();
            if (inventory == null) return;

            var amount = _thingSpecification.CostAmount;
            inventory.ChangeMoneyAmount(amount);
        }

        public void KeysChange(GameObject target)
        {
            if (_thingSpecification == null) return;

            var inventory = target.GetComponent<InventoryComponent>();
            if (inventory == null) return;

            inventory.ChangeKeyAmount(1);
        }
    }
}
