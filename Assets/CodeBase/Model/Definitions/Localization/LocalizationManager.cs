using System;
using System.Collections;
using UnityEngine;
using PixelCrew.Model.Data.Properties;

namespace PixelCrew.Model.Definitions.Localization
{
    public class LocalizationManager : MonoBehaviour
    {
        public readonly static LocalizationManager I;

        private StringPersistentProperty _currentLocale = new StringPersistentProperty("en", "localization/current");

        static LocalizationManager()
        {
            I = new LocalizationManager();
        }

        public LocalizationManager()
        {
            Initialize();
        }

        private void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}