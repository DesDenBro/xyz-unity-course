using System;
using System.Linq;
using UnityEngine;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/ThrowableItemsDef", fileName = "ThrowableItemsDef")]
    public class ThrowableItemsDef : ScriptableObject
    {
        [SerializeField] private ThrowableDef[] _items;

        public ThrowableDef Get(string id)
        {
            return _items?.FirstOrDefault(x => x.Id == id) ?? default;
        }
    }

    [Serializable]
    public struct ThrowableDef
    {
        [InventoryIdAttr][SerializeField] private string _id;
        [SerializeField] GameObject _projectile;
        [SerializeField] string _actionName;

        public string Id => _id;
        public GameObject Projectile => _projectile;
        public string ActionName => _actionName;
    }
}
