using PixelCrew.Components;
using PixelCrew.Utils;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.UI
{
    public class MainMenuWindow : AnimatedWindow
    {
        private Action _closeAction;

        public void OnShowSettings()
        {
            UIWindowUtils.InitWindow("UI/SettingsMenuWindow");
        }

        public void OnStartGame()
        {
            _closeAction = () => 
            { 
                SceneManager.LoadScene("Level1"); 
            };
            Close();
        }

        public void OnRestartLevel()
        {
            _closeAction = () =>
            {
                var restartLevel = FindObjectOfType<RestartLevelComponent>();
                if (restartLevel != null) restartLevel.Restart();
            };
            Close();
        }

        public void OnLanguages()
        {
            UIWindowUtils.InitWindow("UI/LocalizationWindow");
        }

        public void OnExitGame()
        {
            _closeAction = () => 
            {
                Application.Quit();

#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            };
            Close();
        }

        public override void OnCloseAnimationComplete()
        {
            base.OnCloseAnimationComplete();
            _closeAction?.Invoke();
        }
    }
}
