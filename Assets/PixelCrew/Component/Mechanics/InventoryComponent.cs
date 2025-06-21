using UnityEngine;

namespace PixelCrew.Components
{
    public class InventoryComponent : MonoBehaviour
    {
        [SerializeField] private int _money;

        public int MoneyCount => _money;

        public void ChangeMoneyAmount(int val)
        {
            if (val == 0) return;

            _money += val;
            Debug.Log("+" + val + ", всего денег: " + _money);
        }
    }
}
