using PixelCrew.Model.Definitions.Localization;
using PixelCrew.UI.Widgets;
using System;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PixelCrew.UI.Localization
{
    public class LocaleItemWidget : MonoBehaviour, IItemRenderer<LocalInfo>
    {
        [SerializeField] private Text _text;
        [SerializeField] private GameObject _selector;
        [SerializeField] private SelectLocale _onSelected;

        private LocalInfo _data;
        
        private void Start()
        {
            LocalizationManager.I.OnLocaleChanged += UpdateSelection;
        }

        public void SetData(LocalInfo localeKey, int index)
        {
            _data = localeKey;
            UpdateSelection();
            _text.text = localeKey.LocaleId.ToUpper();
        }

        private void UpdateSelection()
        {
            var isSelected = LocalizationManager.I.LocaleKey == _data.LocaleId;
            _selector.SetActive(isSelected);
        }

        public void OnSelected()
        {
            _onSelected?.Invoke(_data.LocaleId);
        }

        private void OnDestroy()
        {
            LocalizationManager.I.OnLocaleChanged -= UpdateSelection;
        }
    }

    [Serializable]
    public class SelectLocale : UnityEvent<string> { }

    public class LocalInfo
    {
        public string LocaleId;
    }
}