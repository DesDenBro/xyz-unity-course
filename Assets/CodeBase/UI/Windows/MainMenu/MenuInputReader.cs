using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.UI
{
    public class MenuInputReader : MonoBehaviour
    {
        [SerializeField] private CallMenuController _callMenuController;

        public void PressCallMenu(InputAction.CallbackContext context)
        {
            _callMenuController.CallMenu();
        }
    }
}