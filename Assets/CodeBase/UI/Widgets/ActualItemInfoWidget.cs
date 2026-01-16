using PixelCrew.Components;
using PixelCrew.GameObjects.Creatures;
using PixelCrew.Model.Definitions;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Widgets
{
    public class ActualItemInfoWidget : MonoBehaviour
    {
        [SerializeField] [InventoryIdAttr] public string _inventoryItemId;
        [SerializeField] private Image _icon;
        [SerializeField] private Text _text;

        private InventoryComponent _invComp;
        
        private void Start() 
        {
            var hero = FindObjectOfType<Hero>();
            if (hero != null) _invComp = hero.GetComponent<InventoryComponent>();
            if (_invComp != null)
            {
                _invComp.InventoryData.onInventoryChanged += OnChangedInventory;
                SetItem(_inventoryItemId);
            }
        }

        public void SetItem(string inventoryItemId)
        {
            _inventoryItemId = inventoryItemId;

            var item = _invComp.InventoryData.GetItem(_inventoryItemId);
            var def = DefsFacade.I.Items.Get(_inventoryItemId);
            _icon.sprite = def.Icon;
            OnChangedInventory(_inventoryItemId, item?.Value ?? 0);
        }
 
        private void OnChangedInventory(string id, int value)
        {
            if (id != _inventoryItemId) return;

            _text.text = "x" + value;
        }

        private void OnDisable()
        {
            if (_invComp != null) _invComp.InventoryData.onInventoryChanged -= OnChangedInventory;
        }

        private void OnDestroy()
        {
            OnDisable();
        }
    }
}