using System;
using UnityEngine;

namespace PixelCrew.Model.Data.Properties
{
    [Serializable]
    public class FloatPersistentProperty : PrefPersistentProperty<float>
    {
        public FloatPersistentProperty(float defVal, string key) : base(defVal, key)
        {
            Init();
        }

        protected override float Read(float defaultValue)
        {
            return PlayerPrefs.GetFloat(Key, defaultValue);
        }

        protected override void Write(float value)
        {
            PlayerPrefs.SetFloat(Key, value);
            PlayerPrefs.Save();
        }
    }
}
