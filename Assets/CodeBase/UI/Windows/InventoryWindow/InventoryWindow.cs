using System;
using System.Collections.Generic;
using PixelCrew.Components;
using PixelCrew.GameObjects.Creatures;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Localization;
using PixelCrew.Utils;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Inventory
{
    public class InventoryWindow : AnimatedWindow
    {
        private const string __throwSelectionDescrKey = "inventory_btn_throwabledescr";
        private const string __useSelectionDescrKey = "inventory_btn_usedescr";
        private const string __seeItemDecrHintKey = "inventory_btn_seeitemdescrhint";

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        [SerializeField] private InventoryMode _currentMode = InventoryMode.Default;
        [SerializeField] private Transform _container;
        [SerializeField] private GameObject _rowPrefab;
        [SerializeField] private int _rowsCount = 3;
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private int _itemsInRowCount = 8;
        [SerializeField] private Text _description;
        [SerializeField] private Button _setThrowableBtn;
        [SerializeField] private Button _useBtn;

        private InventoryItemWidget _selectedItem;
        private InventoryComponent _invComp;
        private IReadOnlyCollection<InventoryItemWidget> _currentItems;

        private void Awake() 
        {
            var hero = FindObjectOfType<Hero>();
            if (hero != null)
            {
                _invComp = hero.GetComponent<InventoryComponent>();
                _invComp.InventoryData.onInventoryChanged += OnInventoryChange;
            }

            Rebuld();
            ActivateMode(InventoryMode.Default);

            _trash.Retain( _useBtn.onClick.Subscribe(() => { ActivateMode(InventoryMode.UseItem); }));
            _trash.Retain( _setThrowableBtn.onClick.Subscribe(() => { ActivateMode(InventoryMode.SelectThrowable); }));
        }

        private void ActivateMode(InventoryMode mode)
        {
            if (_selectedItem != null) _selectedItem.SetSelection(false);

            if (_currentMode == InventoryMode.Default && mode != InventoryMode.Default)
            {
                _currentMode = mode;
                switch (mode)
                {
                    case InventoryMode.SelectThrowable: 
                        _useBtn.interactable = false;
                        FilterItemByTag(ItemTag.Throwable);
                        SetDescription(__throwSelectionDescrKey, true);
                        break;
                    case InventoryMode.UseItem:
                        _setThrowableBtn.interactable = false;
                        FilterItemByTag(ItemTag.Usable);
                        SetDescription(__useSelectionDescrKey, true);
                        break;
                    default: 
                        Debug.LogWarning("No mode processing for inventory window found!");
                        ActivateMode(InventoryMode.Default);
                        return;
                }
            }
            else
            {
                _currentMode = InventoryMode.Default;
                SetDescription(__seeItemDecrHintKey, true);
                _setThrowableBtn.interactable = true;
                _useBtn.interactable = true;
                FilterItemByTag(null);
            }
        }

        private void FilterItemByTag(ItemTag? tag)
        {
            if (_currentItems == null || _currentItems.Count == 0) return;

            foreach (var widget in _currentItems)
            {
                if (widget == null) continue;
                if (tag == null) { widget.SetLockState(false); continue; }
                widget.SetLockState(!widget.IsInTag(tag.Value));
            }
        }

        public void SelectItem(InventoryItemWidget itemW)
        {
            var itemId = itemW.ItemId;
            var itemDef = DefsFacade.I.Items.Get(itemId);
            if (itemDef.IsVoid) return;

            if (_selectedItem != null) _selectedItem.SetSelection(false);

            switch (_currentMode)
            {
                case InventoryMode.SelectThrowable:
                    _invComp.InventoryData.ChangeThrowable(itemDef.Id);
                    break;
                case InventoryMode.UseItem:

                    break;
                case InventoryMode.Default:
                    _selectedItem = itemW;
                    _selectedItem.SetSelection(true);
                    SetDescription(itemDef.DescriptionLocaleKey);
                    return;
            }

            ActivateMode(InventoryMode.Default);
        }

        private void OnInventoryChange(string id, int val)
        { 
            Rebuld();
        }

        private void Rebuld()
        {
            _currentItems = Array.Empty<InventoryItemWidget>();
            UIWindowUtils.ClearContainer(_container.gameObject);

            if (_invComp == null)
            {
                Debug.LogWarning("Inventary component is not found");
                return;
            }

            var inventory = _invComp.InventoryData?.GetAll();
            if (inventory == null)
            {
                Debug.LogWarning("Inventary data is not found");
                return;
            }

            var widgets = new List<InventoryItemWidget>(_rowsCount * _itemsInRowCount);
            var listIndex = 0;
            for (int i = 0; i < _rowsCount; i++)
            {
                var row = Instantiate(_rowPrefab, _container);
                for (int j = 0; j < _itemsInRowCount; j++)
                {
                    var item = Instantiate(_itemPrefab, row.transform);
                    var widget = item.GetComponent<InventoryItemWidget>();
                    if (widget == null) continue;

                    widgets.Add(widget);
                    if (listIndex < inventory.Length)
                    {
                        widget.SetData(inventory[listIndex], listIndex);
                    }
                    
                    listIndex++;
                }
            }

            _currentItems = widgets;
        }

        private void SetDescription(string key, bool isTech = false)
        {
            _description.text = LocalizationManager.I.Localize(key);
            _description.color = new Color(_description.color.r, _description.color.g, _description.color.b, isTech ? 0.5f : 1f);
        }

        void OnDestroy()
        {
            if (_invComp != null) _invComp.InventoryData.onInventoryChanged -= OnInventoryChange;
            _trash.Dispose();
        }
    }

    public enum InventoryMode
    {
        Default,
        SelectThrowable,
        UseItem
    }
}