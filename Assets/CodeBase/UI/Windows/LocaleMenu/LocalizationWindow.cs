using System.Collections.Generic;
using UnityEngine;
using PixelCrew.UI.Localization;
using PixelCrew.UI.Widgets;
using PixelCrew.Model.Definitions.Localization;

namespace PixelCrew.UI
{
    public class LocalizationWindow : AnimatedWindow
    {
        [SerializeField] private Transform _container;
        [SerializeField] private LocaleItemWidget _perfab;

        private DataGroup<LocalInfo, LocaleItemWidget> _dataGroup;

        private string[] _supportedLocales = new[] { "en", "ru" }; 

        protected override void Start()
        {
            base.Start();

            _dataGroup = new DataGroup<LocalInfo, LocaleItemWidget>(_perfab, _container);
            _dataGroup.SetData(ComposeData());
        }

        private List<LocalInfo> ComposeData()
        {
            var data = new List<LocalInfo>();
            foreach (var locale in _supportedLocales)
            {
                data.Add(new LocalInfo { LocaleId = locale });
            }
            return data;
        }

        public void OnSelected(string selectedLocale)
        {
            LocalizationManager.I.SetLocale(selectedLocale);
        }
    }
}