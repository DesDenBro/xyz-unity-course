using PixelCrew.Components;
using PixelCrew.Model.Data.Properties;
using PixelCrew.Utils.Disposables;
using System;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    public class QuickInventoryModel
    {
        private readonly InventoryComponent _invComp;

        public InventoryDataItem[] Inventory { get; private set; }

        public readonly IntProperty SelectedIndex = new IntProperty();
        public Action OnChanged;
        public InventoryDataItem SelectedItem => Inventory.Length > 0 ? Inventory[SelectedIndex.Value] : null;

        public QuickInventoryModel(InventoryComponent invComp)
        {
            _invComp = invComp;
            Inventory = _invComp.InventoryData.GetAll(Definitions.ItemTag.Usable);
            _invComp.InventoryData.onInventoryChanged += OnChangedInventory;
        }

        public IDisposable Subscribe(Action call)
        {
            OnChanged += call;
            return new ActionDisposable(() => OnChanged -= call);
        }

        private void OnChangedInventory(string id, int value)
        {
            if (string.IsNullOrWhiteSpace(id)) return;

            Inventory = _invComp.InventoryData.GetAll(Definitions.ItemTag.Usable);
            var indexFound = Array.FindIndex(Inventory, x => x.Id == id);
            if (indexFound != -1 || value == 0)
            {
                OnChanged?.Invoke();
                SelectedIndex.Value = Mathf.Clamp(SelectedIndex.Value, 0, Inventory.Length - 1);
            }
        }

        internal void SetNextItem()
        {
            SelectedIndex.Value = (int)Mathf.Repeat(SelectedIndex.Value + 1, Inventory.Length);
        }

        public void OnDestroy()
        {
            _invComp.InventoryData.onInventoryChanged -= OnChangedInventory;
        }
    }
}