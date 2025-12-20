using PixelCrew.Components;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.UI
{
    public class CallMenuController : MonoBehaviour
    {
        private Cooldown menuCallCooldown = new Cooldown(1f);

        public bool IsMainMenuLevel => SceneManager.GetActiveScene().name == "MainMenu";

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
            
            if (!IsMainMenuLevel)
            {
                var animWindows = gameObject.GetComponentsInChildren<AnimatedWindow>(); // пока думаем, что есть один
                if (animWindows.Length > 0) 
                {
                    animWindows.Last().Close();
                    return;
                }
            }

            MainMenuWindow mainMenuWindow = gameObject.GetComponentInChildren<MainMenuWindow>();
            if (mainMenuWindow == null)
            {
                ShowMenu();
            }
            else
            {
                CloseMenu();
            }
        }

        public void CloseMenu()
        {
            var menus = gameObject.GetComponentsInChildren<MenuOrder>().OrderByDescending(x => x.OrderPostion);
            var openedMenu = menus.FirstOrDefault();
            if (openedMenu == null) return;
            if (IsMainMenuLevel && openedMenu.OrderPostion == 0) return; // главное меню, в нем нельзя закрыть меню полностью

            openedMenu.gameObject.GetComponent<AnimatedWindow>().Close();
        }

        public void ShowMenu()
        {
            var mainMenuWindowGO = Resources.Load<GameObject>("UI/MainMenuWindow");
            mainMenuWindowGO.GetComponent<AnimatedWindow>().SetButtonsVisible(IsMainMenuLevel);
            var canvas = FindObjectsOfType<Canvas>().FirstOrDefault(x => x.tag == "MenuCanvas");
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