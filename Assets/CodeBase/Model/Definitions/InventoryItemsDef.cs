using System;
using System.Linq;
using UnityEngine;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/InventoryItems", fileName = "InventoryItems")]
    public class InventoryItemsDef : ScriptableObject
    {
        [SerializeField] private ItemDef[] _items;

        public ItemDef Get(string id)
        {
            return _items?.FirstOrDefault(x => x.Id == id) ?? default;
        }
    }

    [Serializable]
    public struct ItemDef
    {
        [SerializeField] private string _id;

        public string Id => _id;

        public bool IsVoid => string.IsNullOrWhiteSpace(Id);
    }
}
