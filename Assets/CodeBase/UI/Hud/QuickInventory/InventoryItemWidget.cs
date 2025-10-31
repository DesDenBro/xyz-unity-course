using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.UI;

namespace PixelCrew.UI.Hud.QuickInventory
{
    public class InventoryItemWidget : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _selection;
        [SerializeField] private Text _value;

        private readonly CompositeDisposable _trash = new CompositeDisposable();
        private int _index;

        private void Start()
        {
            var session = GameSessionSearch.Get(FindObjectsOfType<GameSession>);
            _trash.Retain(session.QuickInventory.SelectedIndex.SubscribeAndInvoke(OnIndexChanged));
        }

        private void OnIndexChanged(int newValue, int oldValue)
        {
            _selection.SetActive(_index == newValue);
        }

        public void SetData(InventoryDataItem item, int index)
        {
            _index = index;
            var def = DefsFacade.I.Items.Get(item.Id);
            _icon.sprite = def.Icon;
            _value.text = !def.IsStackOnlyOne ? ("x" + item.Value.ToString()) : string.Empty;
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}