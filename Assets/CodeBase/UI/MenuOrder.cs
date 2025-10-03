using UnityEngine;

namespace PixelCrew.UI
{
    public class MenuOrder : MonoBehaviour
    {
        [SerializeField] private int _orderPostion;

        public int OrderPostion => _orderPostion;
    }
}