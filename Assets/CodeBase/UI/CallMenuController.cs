using PixelCrew.Model;
using UnityEngine;

namespace PixelCrew.UI
{
    public class CallMenuController : MonoBehaviour
    {
        private GameObject _mainMenuWindow;
        
        private void Awake()
        {
            if (IsControllerExist())
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(this);
                CallMenu();
            }
        }

        public void CallMenu()
        {
            if (_mainMenuWindow == null)
            {
                _mainMenuWindow = Resources.Load<GameObject>("UI/MainMenuWindow");
            }

            var canvas = FindObjectOfType<Canvas>();
            Instantiate(_mainMenuWindow, canvas.transform);
        }

        private bool IsControllerExist()
        {
            var cmcs = FindObjectsOfType<CallMenuController>();
            foreach (var cmc in cmcs)
            {
                if (cmc != this) return true;
            }
            return false;
        }
    }
}