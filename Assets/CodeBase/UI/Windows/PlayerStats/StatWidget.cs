using System;
using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Localization;
using PixelCrew.UI.Widgets;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI
{
    public class StatWidget : MonoBehaviour, IItemRenderer<StatDef>
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Text _name;
        [SerializeField] private Text _currentValue;
        [SerializeField] private Text _increaseValue;
        [SerializeField] private ProgressBarWidget _progress;
        [SerializeField] private GameObject _selector;

        private GameSession _session;
        private StatDef _data;

        private void Start()
        {
            _session = GameSessionSearch.Get(FindObjectsOfType<GameSession>);
            UpdateView();
        }

        public void SetData(StatDef data, int index)
        {
            _data = data;
            if (_session != null)
            {
                UpdateView();
            }
        }

        public void OnSelect()
        {
            _session.StatsModel.InterfaceSelectedStat.Value = _data.StatId;    
        }

        private void UpdateView()
        {
            _icon.sprite = _data.Icon;
            _name.text = LocalizationManager.I.Localize(_data.LocalizationKey);

            var statsModel = _session.StatsModel;
            var currentValue = statsModel.GetValue(_data.StatId);
            _currentValue.text = currentValue.ToString();

            var currentLevel = statsModel.GetCurrentLevel(_data.StatId);
            var nextLevel = currentLevel + 1;

            var increaseValue = statsModel.GetValue(_data.StatId, nextLevel) - currentValue;
            _increaseValue.text = "+" + increaseValue.ToString();
            _increaseValue.gameObject.SetActive(increaseValue > 0);

            var maxLevel = DefsFacade.I.Stats.GetStat(_data.StatId).Levels.Length - 1;
            var percent = currentLevel / (float)maxLevel;
            _progress.SetProgress(percent == 0 ? 0.05f : percent);

            _selector.SetActive(statsModel.InterfaceSelectedStat.Value == _data.StatId);
        }
    }
}