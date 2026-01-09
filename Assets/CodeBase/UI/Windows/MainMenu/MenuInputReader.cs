using UnityEngine;
using UnityEngine.InputSystem;
using PixelCrew.UI.Contollers;

namespace PixelCrew.UI
{
    public class MenuInputReader : MonoBehaviour
    {
        [SerializeField] private CallMenuController _callMenuController;
        [SerializeField] private CallInventoryController _callInventoryController;

        public void PressCallMenu(InputAction.CallbackContext context)
        {
            _callMenuController.Call(true);
        }

        public void PressCallInventory(InputAction.CallbackContext context)
        {
            _callInventoryController.Call(false);
        }
    }
}