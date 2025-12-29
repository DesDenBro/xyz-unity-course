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
        [SerializeField] private int _stackLimitSize;
        [SerializeField] private ItemTag[] _tags;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _descriptionLocaleKey;

        public string Id => _id;
        public bool IsStackOnlyOne => HasTag(ItemTag.Stackable) && StackLimitSize == 1;
        public bool IsStackLimit => HasTag(ItemTag.Stackable) && StackLimitSize > 0;
        public int StackLimitSize => _stackLimitSize;
        public GameObject Prefab => _prefab;
        public Sprite Icon => _icon;
        public string DescriptionLocaleKey => _descriptionLocaleKey;

        public bool IsVoid => string.IsNullOrWhiteSpace(Id);

        public bool HasTag(ItemTag tag)
        {
            return _tags?.Contains(tag) ?? false;
        }
    }
}
