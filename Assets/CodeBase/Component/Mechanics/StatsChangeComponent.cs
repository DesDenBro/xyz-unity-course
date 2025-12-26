using PixelCrew.Common;
using PixelCrew.Model;
using UnityEngine;

namespace PixelCrew.Components
{
    // компонент используется для объектов, изменяющих состояние героя (шипы, тотемы, двери с замками и т.д.)
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
            inventory.ChangeInventoryItemCount(InventoryItemName.Money, amount);
        }

        public void KeysChange(GameObject target)
        {
            if (_thingSpecification == null) return;

            var inventory = target.GetComponent<InventoryComponent>();
            if (inventory == null) return;

            var amount = _thingSpecification.KeysAmount;
            inventory.ChangeInventoryItemCount(InventoryItemName.Key, amount);
        }
    }
}
