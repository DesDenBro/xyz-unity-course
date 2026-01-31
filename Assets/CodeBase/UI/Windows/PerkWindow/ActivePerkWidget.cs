using PixelCrew.GameObjects.Creatures;
using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.Model.Definitions.Repositories;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Perks
{
    public class ActivePerkWidget : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _isLocked;
        [SerializeField] private Text _timeToEnd;

        private Hero _hero;
        private GameSession _session;

        private void Awake()
        {
            _session = GameSessionSearch.Get(FindObjectsOfType<GameSession>);
            
            UpdateView();
        }

        public void FixedUpdate()
        {
            var def = GetCurrentPerkDef();
            if (def.IsVoid || !def.IsCooldownExist) return;
            
            if (_hero == null) _hero = FindObjectOfType<Hero>();
            if (_hero != null)
            {
                var timeToEnd = _hero.TimeToCooldownEnd(def.Id);
                if (timeToEnd > 0)
                {
                    _isLocked.SetActive(true);
                    _timeToEnd.gameObject.SetActive(true);
                    _timeToEnd.text = timeToEnd.ToString("F1");
                }
                else
                {
                    _isLocked.SetActive(false);
                    _timeToEnd.gameObject.SetActive(false);
                }
            }
        }

        public void UpdateView()
        {
            gameObject.SetActive(false);
            var def = GetCurrentPerkDef();
            if (def.IsVoid || def.IsPassive) return;
            
            gameObject.SetActive(true);
            _isLocked.SetActive(false);
            _icon.sprite = def.Icon;
        }

        private PerkDef GetCurrentPerkDef()
        {
            return DefsFacade.I.Perks.Get(_session?.PerksModel?.Used);
        }
    }
}