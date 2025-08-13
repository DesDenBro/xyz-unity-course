using PixelCrew.Model.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PixelCrew.Model
{
    [Serializable]
    public class InventoryData
    {
        [SerializeField] private List<InventoryDataItem> _inventory = new List<InventoryDataItem>();

        public delegate void OnInventoryChanged(string id, int value);

        public OnInventoryChanged onInventoryChanged;

        public void Add(string id, int value)
        {
            if (string.IsNullOrWhiteSpace(id) || value <= 0) return;

            var itemDef = DefsFacade.I.Items.Get(id);
            if (itemDef.IsVoid) return;
            
            while (value > 0)
            {
                var countToAdd = !itemDef.IsStackLimit || value <= itemDef.StackLimitSize ? value : itemDef.StackLimitSize;

                var items = GetItems(id);
                if (itemDef.IsStackOnlyOne && items.Count(x => x.IsFullStack) > 0) break;

                var firstNotFullStackItem = items.FirstOrDefault(x => !x.IsFullStack);
                if (firstNotFullStackItem == null)
                {
                    _inventory.Add(new InventoryDataItem(id, countToAdd, itemDef));
                }
                else
                {
                    if (itemDef.IsStackLimit)
                    {
                        countToAdd = firstNotFullStackItem.Value + countToAdd > itemDef.StackLimitSize ? itemDef.StackLimitSize - firstNotFullStackItem.Value : countToAdd;
                    }
                    firstNotFullStackItem.Value += countToAdd;
                }

                value -= countToAdd;
            }

            onInventoryChanged?.Invoke(id, Count(id));
        }

        public void Remove(string id, int value)
        {
            if (string.IsNullOrWhiteSpace(id) || value == 0) return;
            if (value < 0) value = -value; // - стоит, чтобы перебить знак минуса на уменьшение

            var itemDef = DefsFacade.I.Items.Get(id);
            if (itemDef.IsVoid) return;

            var items = GetItems(id);
            foreach (var invItem in items)
            {
                if (value <= 0) break;

                var countToRemove = value;
                if (invItem.Value < countToRemove) countToRemove = invItem.Value;

                invItem.Value -= countToRemove;
                if (invItem.Value <= 0)
                {
                    _inventory.Remove(invItem);
                }

                value -= countToRemove;
            }

            onInventoryChanged?.Invoke(id, Count(id));
        }

        public InventoryDataItem GetItem(string id)
        {
            return _inventory.FirstOrDefault(x => x.Id == id);
        }
        public IReadOnlyCollection<InventoryDataItem> GetItems(string id)
        {
            return _inventory.Where(x => x.Id == id).ToList();
        }

        public int Count(string id) 
        {
            return _inventory.Where(x => x.Id == id).Sum(x => x.Value);
        }

        public InventoryData Clone()
        {
            var json = JsonUtility.ToJson(this);
            var clone = JsonUtility.FromJson<InventoryData>(json);
            foreach (var item in clone._inventory)
            {
                item.SetLinks(DefsFacade.I.Items.Get(item.Id));
            }
            return clone;
        }
    }

    [Serializable]
    public class InventoryDataItem
    {
        [SerializeField] private ItemDef _itemDef;

        [InventoryIdAttr] public string Id;
        public int Value;

        public GameObject Prefab => _itemDef.Prefab;
        public bool IsStackLimit => _itemDef.IsStackLimit;
        public int StackLimitSize => _itemDef.StackLimitSize;
        public bool IsFullStack => IsStackLimit && Value == StackLimitSize;


        public InventoryDataItem(string id, int value, ItemDef itemDef) 
        {
            Id = id;
            Value = value;

            SetLinks(itemDef);
        }

        public void SetLinks(ItemDef itemDef) => _itemDef = itemDef;
    }

    public static class InventoryItemName
    {
        public const string Money = "Money";
        public const string Key = "Key";
        public const string Throw = "Throw";
        public const string Sword = "Sword";
        public const string HealhPotion = "HealhPotion";
    }
}
