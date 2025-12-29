using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.Utils.Disposables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Inventory
{
    public class InventoryItemWidget : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _selection;
        [SerializeField] private Text _value;
        [SerializeField] private GameObject _lock;

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private int _index;
        private ItemDef _itemDef;

        private void Start()
        {
            var session = GameSessionSearch.Get(FindObjectsOfType<GameSession>);
            _trash.Retain(session.QuickInventory.SelectedIndex.SubscribeAndInvoke(OnIndexChanged));
        }

        private void OnIndexChanged(int newValue, int oldValue)
        {
            _selection.SetActive(_index == newValue);
        }

        public void SetData(InventoryDataItem item, int index)
        {
            _index = index;
            _itemDef = DefsFacade.I.Items.Get(item.Id);
            _icon.sprite = _itemDef.Icon;
            _value.text = !_itemDef.IsStackOnlyOne ? ("x" + item.Value.ToString()) : string.Empty;
        }

        public bool IsInTag(ItemTag tag)
        {
            return !_itemDef.IsVoid && _itemDef.HasTag(tag);
        }

        public void SetLockState(bool state)
        {
            if (_lock == null) return;
            _lock.SetActive(state);
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}