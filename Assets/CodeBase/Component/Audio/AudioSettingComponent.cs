﻿using PixelCrew.Model.Data;
using PixelCrew.Model.Data.Properties;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSettingComponent : MonoBehaviour
    {
        [SerializeField] private SoundSetting _mode;

        private AudioSource _audioSource;
        private FloatPersistentProperty _model;
        
        void Start()
        {
            _audioSource = GetComponent<AudioSource>();

            _model = FindProperty();
            _model.OnChanged += OnSoundSettingChanged;

            OnSoundSettingChanged(_model.Value, _model.Value);
        }

        private void OnSoundSettingChanged(float newValue, float oldValue)
        {
            _audioSource.volume = newValue;
        }

        private FloatPersistentProperty FindProperty()
        {
            switch (_mode)
            {
                case SoundSetting.Music: return GameSettings.I.MusicProp;
                case SoundSetting.SFX: return GameSettings.I.SFXProp;
            }

            throw new System.Exception("Undefined mode");
        }

        private void OnDestroy()
        {
            _model.OnChanged -= OnSoundSettingChanged;
        }
    }
}
