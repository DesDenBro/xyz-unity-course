using System;
using UnityEngine;
using PixelCrew.Model.Definitions.Repositories;
using PixelCrew.Components;

namespace PixelCrew.Model.Definitions
{
    [Serializable]     
    public struct StatDef : IHaveId
    { 
        [SerializeField] private string _name;
        [SerializeField] private StatId _statId;
        [SerializeField] private Sprite _icon;
        [SerializeField] private StatLevel[] _levels;
        [SerializeField] private string _localizationKey;

        public string Name => _name;
        public string Id => _statId.ToString();
        public StatId StatId => _statId;
        public Sprite Icon => _icon;
        public StatLevel[] Levels => _levels;
        public string LocalizationKey => _localizationKey;
    }

    [Serializable]
    public struct StatLevel
    {
        [SerializeField] private float _value;
        [SerializeField] private ItemWithCount _price;

        public float Value => _value;
        public ItemWithCount Price => _price;
    }

    public enum StatId
    {
        Health,
        Speed,
        RangeDamage,
        Damage
    }
}