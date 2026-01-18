using PixelCrew.Model;
using PixelCrew.Model.Definitions.Repositories;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Perks
{
    public class PerkWidget : MonoBehaviour, IItemRenderer<PerkDef>
    {
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _isLocked;
        [SerializeField] private GameObject _isUsed;
        [SerializeField] private GameObject _isSelected;

        private GameSession _session;
        private PerkDef _data;

        private void Start()
        {
            _session = GameSessionSearch.Get(FindObjectsOfType<GameSession>);
            UpdateView();
        }

        public void SetData(PerkDef data, int index)
        {
            _data = data;
            if (_session != null) UpdateView();
        }

        public void OnSelect()
        {
            _session.PerksModel.InterfaceSelection.Value = _data.Id;
        }

        public void UpdateView()
        {
            _icon.sprite = _data.Icon;
            var isUsedActive = _session.PerksModel.IsUsed(_data.Id);
            _isUsed.SetActive(isUsedActive);
            if (isUsedActive)
            {
                _isUsed.GetComponent<Image>().color = UIWindowUtils.GetColor(_session.PerksModel.IsPassive(_data.Id)
                    ? UIWindowUtils.ColorPalette.PassivePerkUse
                    : UIWindowUtils.ColorPalette.ActivePerkUse
                );
            }
            _isSelected.SetActive(_session.PerksModel.InterfaceSelection.Value == _data.Id);
            _isLocked.SetActive(!_session.PerksModel.IsUnlocked(_data.Id));

        }
    }
}