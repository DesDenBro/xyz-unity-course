using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PixelCrew.Model.Definitions.Localization
{
    [CreateAssetMenu(menuName = "Defs/LocaleDef", fileName = "LocaleDef")]
    public class LocaleDef : ScriptableObject
    {
        [SerializeField] private string _fileName;
        [SerializeField] private List<LocaleItem> _localeItems;

        public Dictionary<string, string> GetData()
        {
            return _localeItems.ToDictionary(x => x.Key, x => x.Value);
        }

        [ContextMenu("Update locale")]
        public async Task UpdateLocaleAsync()
        {
            var handle = Addressables.LoadAssetAsync<TextAsset>(_fileName);
            TextAsset asset = await handle.Task;
            OnDataLoaded(asset);
            Addressables.Release(asset);
        }

        private void OnDataLoaded(TextAsset asset)
        {
            if (asset == null || string.IsNullOrWhiteSpace(asset.text))
            {
                Debug.Log("Asset" + _fileName + "is empty");
                return;
            }

            var rows = asset.text.Split('\n');
            _localeItems.Clear();
            foreach (var row in rows)
            {
                if (string.IsNullOrWhiteSpace(row)) continue;
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
                var parts = row.Split(';');
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