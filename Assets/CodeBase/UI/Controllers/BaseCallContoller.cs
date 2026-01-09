using PixelCrew.Components;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.UI.Contollers
{
    public abstract class BaseCallController<T> : MonoBehaviour where T : AnimatedWindow
    {
        protected bool IsMainMenuLevel => SceneManager.GetActiveScene().name == "MainMenu";
        protected abstract bool CallInMainMenu { get; }
        protected abstract string ResourcePath { get; }
        protected Cooldown _callCooldown = new Cooldown(1f);
        
        protected virtual void Awake()
        {
            if (IsControllerExist())
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(this);
                if (CallInMainMenu) Call(false);
            }
        }

        public virtual void Call(bool isEscBtn)
        {
            if (!_callCooldown.IsReady) return;
            _callCooldown.Reset();
            
            if (!IsMainMenuLevel)
            {
                var animWindows = gameObject.GetComponentsInChildren<AnimatedWindow>(); // пока думаем, что есть один
                if (animWindows.Length > 0) 
                {
                    if (isEscBtn)
                    {
                        animWindows.Last().Close();
                    }
                    return;
                }
            }

            var window = gameObject.GetComponentInChildren<T>();
            if (window == null)
            {
                Show();
            }
            else
            {
                Close();
            }
        }
        protected virtual void Show()
        {
            var windowGO = Resources.Load<GameObject>(ResourcePath);
            windowGO.GetComponent<AnimatedWindow>().SetButtonsVisible(IsMainMenuLevel);
            Canvas canvas = FindObjectsOfType<Canvas>().FirstOrDefault(x => x.tag == "MenuCanvas");
            Instantiate(windowGO, canvas.transform);
        }
        protected virtual void Close()
        {
            var menus = gameObject.GetComponentsInChildren<MenuOrder>().OrderByDescending(x => x.OrderPostion);
            var openedMenu = menus.FirstOrDefault();
            if (openedMenu == null) return;
            if (IsMainMenuLevel && openedMenu.OrderPostion == 0) return;

            openedMenu.gameObject.GetComponent<AnimatedWindow>().Close();
        }

        private bool IsControllerExist()
        {
            var cmcs = FindObjectsOfType<BaseCallController<T>>();
            foreach (var cmc in cmcs)
            {
                if (cmc != this) return true;
            }
            return false;
        }
    }
}