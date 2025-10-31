using PixelCrew.Model;
using PixelCrew.Utils.Disposables;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.UI.Hud.QuickInventory
{
    public class QuickInventoryController : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private GameObject _prefab;

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private GameSession _session;
        private InventoryDataItem[] _inventory;
        private List<InventoryItemWidget> _createdItem = new List<InventoryItemWidget>();

        private void Start()
        {
            _session = GameSessionSearch.Get(FindObjectsOfType<GameSession>);
            _trash.Retain(_session.QuickInventory.Subscribe(Rebuld));
            Rebuld();
        }

        private void Rebuld()
        {
            _inventory = _session.QuickInventory.Inventory;
            if (_inventory == null) return;

            var rect = GetComponent<RectTransform>();
            if (_inventory.Length > 0)
            {
                rect.sizeDelta = new Vector2(_inventory.Length * 15 + 6, 24);

                // create req
                for (int i = _createdItem.Count; i < _inventory.Length; i++)
                {
                    var item = Instantiate(_prefab, _container);
                    _createdItem.Add(item.GetComponent<InventoryItemWidget>());
                }

                // update data and activate
                for (var i = 0; i < _inventory.Length; i++)
                {
                    _createdItem[i].SetData(_inventory[i], i);
                    _createdItem[i].gameObject.SetActive(true);
                }
            }
            else
            {
                rect.sizeDelta = Vector2.zero;
            }

            // hide unused
            for (int i = _inventory.Length; i < _createdItem.Count; i++)
            {
                var gm = _createdItem[i].gameObject;
                _createdItem[i].gameObject.SetActive(false);
                _createdItem.Remove(_createdItem[i]);
                Destroy(gm);
            }
        }

        public void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}
