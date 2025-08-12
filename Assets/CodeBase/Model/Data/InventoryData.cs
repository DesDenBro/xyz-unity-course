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

            var item = GetItem(id);
            if (item == null)
            {
                item = new InventoryDataItem(id, value, itemDef);
                _inventory.Add(item);
            }
            else
            {
                item.Value += value;
            }

            onInventoryChanged?.Invoke(id, Count(id));
        }

        public void Remove(string id, int value)
        {
            if (string.IsNullOrWhiteSpace(id) || value == 0) return;

            var itemDef = DefsFacade.I.Items.Get(id);
            if (itemDef.IsVoid) return;

            var item = GetItem(id);
            if (item == null) return;

            item.Value -= value;

            if (item.Value <= 0)
            {
                _inventory.Remove(item);
            }

            onInventoryChanged?.Invoke(id, Count(id));
        }

        public InventoryDataItem GetItem(string id)
        {
            return _inventory.FirstOrDefault(x => x.Id == id);
        }

        public int Count(string id) 
        {
            return _inventory.Where(x => x.Id == id).Sum(x => x.Value);
        }

        public InventoryData Clone()
        {
            var json = JsonUtility.ToJson(this);
            return JsonUtility.FromJson<InventoryData>(json);
        }
    }

    [Serializable]
    public class InventoryDataItem
    {
        [InventoryIdAttr] public string Id;
        public int Value;
        public GameObject Prefab;

        public InventoryDataItem(string id, int value, ItemDef itemDef) 
        {
            Id = id;
            Value = value;
            Prefab = itemDef.Prefab;
        }
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
