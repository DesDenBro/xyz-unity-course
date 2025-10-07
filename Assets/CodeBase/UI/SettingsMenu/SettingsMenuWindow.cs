using PixelCrew.Model.Data;
using PixelCrew.UI.Widgets;
using UnityEngine;

namespace PixelCrew.UI
{
    public class SettingsMenuWindow : AnimatedWindow
    {
        [SerializeField] private AudioSettingsWidget _musicASW;
        [SerializeField] private AudioSettingsWidget _sfxASW;

        protected override void Start()
        {
            base.Start();

            _musicASW.SetModel(GameSettings.I.MusicProp);
            _sfxASW.SetModel(GameSettings.I.SFXProp);
        }        
    }
}
