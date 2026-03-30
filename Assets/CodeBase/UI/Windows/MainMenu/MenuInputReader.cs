using UnityEngine;
using UnityEngine.InputSystem;
using PixelCrew.UI.Contollers;
using UnityEngine.SceneManagement;

namespace PixelCrew.UI
{
    public class MenuInputReader : MonoBehaviour
    {
        [SerializeField] private CallMenuController _callMenuController;
        [SerializeField] private CallInventoryController _callInventoryController;
        protected bool IsMainMenuLevel => SceneManager.GetActiveScene().name == "MainMenu";

        public void PressCallMenu(InputAction.CallbackContext context)
        {
            _callMenuController.Call(true);
        }

        public void PressCallInventory(InputAction.CallbackContext context)
        {
            if (IsMainMenuLevel) return;
            _callInventoryController.Call(false);
        }
    }
}