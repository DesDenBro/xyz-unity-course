using UnityEngine;

namespace PixelCrew.Components
{
    public class InventoryComponent : MonoBehaviour
    {
        [SerializeField] private int _money;
        [SerializeField] private int _keys;

        public int MoneyCount => _money;
        public int KeysCount => _keys;

        public bool ChangeMoneyAmount(int count) => ChangeInventoryObjectCount(InventoryObjectType.Money, count);
        public bool ChangeKeyAmount(int count) => ChangeInventoryObjectCount(InventoryObjectType.Key, count);
        public bool ChangeInventoryObjectCount(InventoryObjectType type, int count)
        {
            if (count == 0) return true;

            switch (type)
            {
                case InventoryObjectType.Money:
                    if (!CheckMoneyCountToEvent(count))
                    {
                        Debug.Log("Не хватает денег");
                        return false;
                    }
                    _money += count;
                    Debug.Log((count > 0 ? "+" : string.Empty) + count + ", всего денег: " + _money);
                    break;

                case InventoryObjectType.Key:
                    if (!CheckKeyCountToEvent(count))
                    {
                        Debug.Log("Не хватает ключей");
                        return false;
                    }
                    _keys += count;
                    Debug.Log((count > 0 ? "+" : string.Empty) + count + ", всего ключей: " + _keys);
                    break;

                default:
                    break;
            }

            return true;
        }

        public bool CheckMoneyCountToEvent(int count) => CheckInventoryObjectCountToEvent(InventoryObjectType.Money, count);
        public bool CheckKeyCountToEvent(int count) => CheckInventoryObjectCountToEvent(InventoryObjectType.Key, count);
        public bool CheckInventoryObjectCountToEvent(InventoryObjectType type, int count)
        {
            if (count == 0) return true;

            switch (type)
            {
                case InventoryObjectType.Money:
                    return count > 0 || (count < 0 && (_money + count <= 0));

                case InventoryObjectType.Key:
                    return count > 0 || (count < 0 && (_keys + count <= 0));

                default: break;
            }

            return false;
        }
    }

    public enum InventoryObjectType : byte
    {
        Money = 0,
        Key = 1
    }
}
