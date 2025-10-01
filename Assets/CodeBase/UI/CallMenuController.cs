using PixelCrew.Components;
using PixelCrew.Model;
using UnityEngine;

namespace PixelCrew.UI
{
    public class CallMenuController : MonoBehaviour
    {
        private Cooldown menuCallCooldown = new Cooldown(1);
        
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
            if (!menuCallCooldown.IsReady) return;
            menuCallCooldown.Reset();

            MainMenuWindow mainMenuWindow = gameObject.GetComponentInChildren<MainMenuWindow>();
            if (mainMenuWindow != null)
            {
                mainMenuWindow.Close();
                return;
            }

            var mainMenuWindowGO = Resources.Load<GameObject>("UI/MainMenuWindow");
            var canvas = FindObjectOfType<Canvas>();
            Instantiate(mainMenuWindowGO, canvas.transform);
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