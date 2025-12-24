using System;
using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI
{
    public class PlayerStatsWindow : AnimatedWindow
    {
        private readonly CompositeDisposable _trash = new CompositeDisposable();

        [SerializeField] private Transform _statsContainer;
        [SerializeField] private StatWidget _prefab;
        [SerializeField] private Button _buyBtn;
        [SerializeField] private ItemWidget _price;
        [SerializeField] private GameObject _maxLevelInfo;

        private DataGroup<StatDef, StatWidget> _dataGroup;

        private GameSession _session;

        protected override void Start()
        {
            base.Start();

            _dataGroup = new DataGroup<StatDef, StatWidget>(_prefab, _statsContainer);

            _session = GameSessionSearch.Get(FindObjectsOfType<GameSession>);

            _session.StatsModel.InterfaceSelectedStat.Value = DefsFacade.I.Stats.All[0].StatId;
            _trash.Retain(_session.StatsModel.Subscribe(OnStatsChanged));

            OnStatsChanged();
        }

        public void OnUpgrade()
        {
            var selected = _session.StatsModel.InterfaceSelectedStat.Value;
            _session.StatsModel.LevelUp(selected);
        }

        private void OnStatsChanged()
        {
            var stats = DefsFacade.I.Stats.All;
            _dataGroup.SetData(stats);

            var selected = _session.StatsModel.InterfaceSelectedStat.Value;
            var nextLevel = _session.StatsModel.GetCurrentLevel(selected) + 1;
            var def = _session.StatsModel.GetLevelDef(selected, nextLevel);
            
            if (def.Price.Count == 0)
            {
                _price.gameObject.SetActive(false);
                _buyBtn.gameObject.SetActive(false);  
                _maxLevelInfo.gameObject.SetActive(true);
            }
            else
            {
                _price.SetData(def.Price);
                _buyBtn.gameObject.SetActive(true);
                _maxLevelInfo.gameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}