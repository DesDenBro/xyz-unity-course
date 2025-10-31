using PixelCrew.Model;
using UnityEngine;

namespace PixelCrew.Components
{
    public class InventoryComponent : MonoBehaviour
    {
        [SerializeField] private InventoryData _inventoryData;

        public InventoryData InventoryData => _inventoryData;

        public InventoryDataItem GetItem(string id)
        {
            return _inventoryData.GetItem(id);
        }

        public void SetInventory(InventoryData inv)
        {
            _inventoryData = inv;
        }

        public int Count(string inventoryId) => _inventoryData.Count(inventoryId);

        public bool ChangeInventoryItemCount(string itemName, int count)
        {
            if (count == 0) return true;
            if (!CheckItemCountToEvent(itemName, count)) return false;

            if (count > 0) _inventoryData.Add(itemName, count);
            else _inventoryData.Remove(itemName, count);

            Debug.Log((count > 0 ? "+" : string.Empty) + count + ", всего " + itemName + ": " + InventoryData.Count(itemName));
            return true;
        }

        public bool CheckItemCountToEvent(string itemName, int count)
        {
            if (string.IsNullOrEmpty(itemName)) return false;
            if (count == 0) return true;

            var res = count > 0 || (count < 0 && (InventoryData.Count(itemName) + count >= 0));
            if (!res) Debug.Log("Не хватает " + itemName);

            return res;
        }
    }
}
