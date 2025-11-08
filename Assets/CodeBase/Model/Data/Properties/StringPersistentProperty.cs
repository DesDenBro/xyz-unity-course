using UnityEngine;

namespace PixelCrew.Model.Data.Properties
{
    public class StringPersistentProperty : PrefPersistentProperty<string>
    {
        public StringPersistentProperty(string desfaultValue, string key) : base(desfaultValue, key) 
        {
            Init();
        }

        protected override void Write(string value)
        {
            PlayerPrefs.SetString(Key, value);
        }

        protected override string Read(string defaultValue)
        {
            return PlayerPrefs.GetString(Key, defaultValue);
        }
    }
}