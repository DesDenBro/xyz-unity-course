using System;
using UnityEngine;
using PixelCrew.Components;

namespace PixelCrew.Model.Definitions.Repositories
{
    [CreateAssetMenu(menuName = "Defs/Repository/Perks", fileName = "Perks")]
    public class PerkRepository : DefRepository<PerkDef> { }

    [Serializable]
    public struct PerkDef : IHaveId
    {
        [SerializeField] public string _id;
        [SerializeField] public Sprite _icon;
        [SerializeField] public string _info;
        [SerializeField] public ItemWithCount _price;
        [SerializeField] public bool _isCooldownExist;
        
        public string Id => _id;
        public Sprite Icon => _icon;
        public string Info => _info;
        public ItemWithCount Price => _price;
        public bool IsCooldownExist => _isCooldownExist;

        public bool IsVoid => string.IsNullOrWhiteSpace(_id);
    }

    [Serializable]
    public struct ItemWithCount
    {
        [InventoryIdAttr] [SerializeField] private string _itemId;
        [SerializeField] private int _count;

        public string ItemId => _itemId;
        public int Count => _count;
    }
}