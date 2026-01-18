using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Localization;
using PixelCrew.Model.Definitions.Repositories;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Perks
{
    public class ManagePerksWindow : AnimatedWindow
    {
        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _useButton;
        [SerializeField] private RequiredItemWidget _price;
        [SerializeField] private Text _info;
        [SerializeField] private Transform _container;

        private PedefinedDataGroup<PerkDef, PerkWidget> _perksDataGroup;
        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private GameSession _session;

        protected override void Start()
        {
            base.Start();

            _perksDataGroup = new PedefinedDataGroup<PerkDef, PerkWidget>(_container);
            _session = GameSessionSearch.Get(FindObjectsOfType<GameSession>);

            _trash.Retain(_session.PerksModel.Subscribe(OnPerksChanged));
            _trash.Retain(_buyButton.onClick.Subscribe(OnBuy));
            _trash.Retain(_buyButton.onClick.Subscribe(OnUse));

            OnPerksChanged();
        }

        private void OnPerksChanged()
        {
            _perksDataGroup.SetData(DefsFacade.I.Perks.All);

            var selected = _session.PerksModel.InterfaceSelection.Value;
            var def = DefsFacade.I.Perks.Get(selected);
            
            _useButton.gameObject.SetActive(_session.PerksModel.IsUnlocked(selected) && !_session.PerksModel.IsPassive(selected));
            _useButton.interactable = _session.PerksModel.Used != selected;

            _buyButton.gameObject.SetActive(!_session.PerksModel.IsUnlocked(selected));
            _buyButton.interactable = _session.PerksModel.CanBuy(selected);

            _price.SetData(def.Price);

            _info.text = LocalizationManager.I.Localize(def.Info);
        }

        public void OnBuy()
        {
            var selected = _session.PerksModel.InterfaceSelection.Value;
            _session.PerksModel.Unlock(selected);
        }

        public void OnUse()
        {
            var selected = _session.PerksModel.InterfaceSelection.Value;
            _session.PerksModel.Use(selected);
        }

        private void OnDestroy() 
        {
            _trash.Dispose();
        }
    }
}