using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;

namespace PixelCrew.Model.Definitions.Localization
{
    [CreateAssetMenu(menuName = "Defs/LocaleDef", fileName = "LocaleDef")]
    public class LocaleDef : ScriptableObject
    {
        [SerializeField] private List<LocaleItem> _localeItems;

        public Dictionary<string, string> GetData()
        {
            return _localeItems.ToDictionary(x => x.Key, x => x.Value);
        }

        [ContextMenu("Update locale")]
        public void UpdateLocale()
        { 
            
        }

        private void OnDataLoaded(AsyncOperation operation)
        {
            if (!operation.isDone) return;

            var rows = "".Split('\t');
            _localeItems.Clear();
            foreach (var row in rows)
            {
                if (TryParseLocaleItem(row, out LocaleItem parsedItem))
                {
                    _localeItems.Add(parsedItem);
                }
            }
        }

        private bool TryParseLocaleItem(string row, out LocaleItem parsedItem)
        {
            parsedItem = default;

            try
            {
                var parts = row.Split('\t');
                parsedItem = new LocaleItem { Key = parts[0], Value = parts[1] };
            }
            catch (Exception ex)
            {
                Debug.LogError($"Parse row \"{row}\" error: {ex.Message}");
                return false;
            }

            return true;
        }



        [Serializable]
        private struct LocaleItem
        {
            public string Key;
            public string Value;
        }
    }
}