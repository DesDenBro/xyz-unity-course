using UnityEngine;

namespace PixelCrew.Components
{
    public class InventoryComponent : MonoBehaviour
    {
        [SerializeField] private int _money;
        [SerializeField] private int _keys;
        [SerializeField] private int _throws;

        public int MoneyCount => _money;
        public int KeysCount => _keys;
        public int ThrowsCount => _throws;


        public void SetMoney(int money) => _money = money;
        public void SetKeys(int keys) => _keys = keys;
        public void SetThrows(int throws) => _throws = throws;


        public bool ChangeMoneyAmount(int count) => ChangeInventoryObjectCount(InventoryObjectType.Money, count);
        public bool ChangeKeyAmount(int count) => ChangeInventoryObjectCount(InventoryObjectType.Key, count);
        public bool ChangeThrowsAmount(int count) => ChangeInventoryObjectCount(InventoryObjectType.Throws, count);
        public bool ChangeInventoryObjectCount(InventoryObjectType type, int count)
        {
            if (count == 0) return true;

            switch (type)
            {
                case InventoryObjectType.Money:
                    if (!CheckMoneyCountToEvent(count)) return false;
                    _money += count;
                    LogChangeCount("денег", count, _money);
                    break;

                case InventoryObjectType.Key:
                    if (!CheckKeyCountToEvent(count)) return false;
                    _keys += count;
                    LogChangeCount("ключей", count, _keys);
                    break;

                case InventoryObjectType.Throws:
                    if (!CheckThrowsCountToEvent(count)) return false;
                    _throws += count;
                    LogChangeCount("метательных снарядов", count, _throws);
                    break;

                default:
                    break;
            }

            return true;
        }
        private void LogChangeCount(string typeName, int count, int summary) => Debug.Log((count > 0 ? "+" : string.Empty) + count + ", всего " + typeName + ": " + summary);

        public bool CheckMoneyCountToEvent(int count)
        {
            var res = CheckInventoryObjectCountToEvent(InventoryObjectType.Money, count);
            if (!res) Debug.Log("Не хватает денег");
            return res;
        }
        public bool CheckKeyCountToEvent(int count)
        {
            var res = CheckInventoryObjectCountToEvent(InventoryObjectType.Key, count);
            if (!res) Debug.Log("Не хватает ключей");
            return res;
        }
        public bool CheckThrowsCountToEvent(int count)
        {
            var res = CheckInventoryObjectCountToEvent(InventoryObjectType.Throws, count);
            if (!res) Debug.Log("Не хватает метательных снарядов");
            return res;
        }
        public bool CheckInventoryObjectCountToEvent(InventoryObjectType type, int count)
        {
            if (count == 0) return true;

            switch (type)
            {
                case InventoryObjectType.Money:
                    return count > 0 || (count < 0 && (_money + count >= 0));

                case InventoryObjectType.Key:
                    return count > 0 || (count < 0 && (_keys + count >= 0));

                case InventoryObjectType.Throws:
                    return count > 0 || (count < 0 && (_throws + count >= 0));

                default: break;
            }

            return false;
        }
    }

    public enum InventoryObjectType : byte
    {
        Money = 0,
        Key = 1,
        Throws = 2
    }
}
