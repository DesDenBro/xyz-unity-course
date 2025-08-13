using PixelCrew.Model;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PixelCrew.Common
{
    public class ThingSpecification : MonoBehaviour
    {
        [SerializeField] private List<InventoryDataItem> items;
        [SerializeField] private int _damagePoints;
        [SerializeField] private int _healthPoints;

        public int CostAmount => items.Where(x => x.Id == InventoryItemName.Money).Sum(x => x.Value);
        public int KeysAmount => items.Where(x => x.Id == InventoryItemName.Key).Sum(x => x.Value);

        public int DamagePoints => _damagePoints;
        public int HealthPoints => _healthPoints;
    }
}