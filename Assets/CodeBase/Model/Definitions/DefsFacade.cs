using UnityEngine;

namespace PixelCrew.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/DefsFacade", fileName = "DefsFacade")]
    public class DefsFacade : ScriptableObject
    {
        [SerializeField] private InventoryItemsDef _items;
        [SerializeField] private ThrowableItemsDef _throwableItems;

        private static DefsFacade _instance;
        private static DefsFacade LoadDefs()
        {
            _instance = Resources.Load<DefsFacade>("DefsFacade");
            return _instance;
        }
        public static DefsFacade I => _instance == null ? LoadDefs() : _instance;

        public InventoryItemsDef Items => _items;
        public ThrowableItemsDef ThrowableItems => _throwableItems;
    }
}
