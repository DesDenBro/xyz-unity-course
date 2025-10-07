using PixelCrew.Model.Data.Properties;
using System;
using UnityEngine;

namespace PixelCrew.Model.Data
{
    [CreateAssetMenu(menuName="Data/GameSettings", fileName="GameSettings")]
    public class GameSettings : ScriptableObject
    {
        [SerializeField] private FloatPersistentProperty _musicProp;
        [SerializeField] private FloatPersistentProperty _sfxProp;

        public FloatPersistentProperty MusicProp => _musicProp;
        public FloatPersistentProperty SFXProp => _sfxProp; 

        private static GameSettings _instance;
        public static GameSettings I => _instance ?? (_instance = LoadGameSettings());

        private static GameSettings LoadGameSettings()
        {
            return Resources.Load<GameSettings>("GameSettings");
        }

        private void OnEnable()
        {
            _musicProp = new FloatPersistentProperty(1, SoundSetting.Music.ToString());
            _sfxProp = new FloatPersistentProperty(1, SoundSetting.SFX.ToString());
        }

        private void OnValidate()
        {
            MusicProp.Validate();
            SFXProp.Validate();
        }
    }

    public enum SoundSetting
    {
        Music,
        SFX
    }
}