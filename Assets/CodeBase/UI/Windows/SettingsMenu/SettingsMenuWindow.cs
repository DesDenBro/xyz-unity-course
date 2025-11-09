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

            if (_musicASW != null) _musicASW.SetModel(GameSettings.I.MusicProp);
            if (_sfxASW != null) _sfxASW.SetModel(GameSettings.I.SFXProp);
        }        
    }
}
