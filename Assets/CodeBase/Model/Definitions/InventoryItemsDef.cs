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

#if UNITY_EDITOR
        public ItemDef[] ItemsForEditor => _items;
#endif


    }

    [Serializable]
    public struct ItemDef
    {
        [SerializeField] private string _id;
        [SerializeField] private bool _isStackLimit;
        [SerializeField] private int _stackLimitSize;
        [SerializeField] GameObject _prefab;

        public string Id => _id;
        public bool IsStackLimit => _isStackLimit;
        public int StackLimitSize => _stackLimitSize;
        public GameObject Prefab => _prefab;

        public bool IsVoid => string.IsNullOrWhiteSpace(Id);
    }
}
