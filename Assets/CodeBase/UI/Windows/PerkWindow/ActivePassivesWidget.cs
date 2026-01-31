using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Perks
{
    public class ActivePassivesWidget : MonoBehaviour
    {
        private const int _itemsInRowCount = 5;

        [SerializeField] private Transform _container;
        [SerializeField] private GameObject _rowPrefab;
        [SerializeField] private GameObject _itemPrefab;
        
        private GameSession _session;

        private void Awake()
        {
            _session = GameSessionSearch.Get(FindObjectsOfType<GameSession>);

            UpdateView();
        }

        public void UpdateView()
        {
            UIWindowUtils.ClearContainer(_container.gameObject);

            var passives = _session.PerksModel.ActivePassivesList;
            if (passives.Count == 0) return;

            var rowsCount = (passives.Count / _itemsInRowCount) + 1;
            var listIndex = 0;
            for (int i = 0; i < rowsCount; i++)
            {
                var row = Instantiate(_rowPrefab, _container);
                row.SetActive(true);
                for (int j = 0; j < _itemsInRowCount; j++)
                {
                    var item = Instantiate(_itemPrefab, row.transform);
                    var imageComp = item.GetComponent<Image>();
                    if (imageComp != null)
                    {
                        var def = DefsFacade.I.Perks.Get(passives[listIndex]);
                        if (!def.IsVoid)
                        { 
                            imageComp.sprite = def.Icon;
                            item.SetActive(true);
                        }
                    }

                    listIndex++;
                    if (listIndex >= passives.Count) break;
                }
            }
        }
    }   
}