using PixelCrew.UI.Widgets;
using UnityEngine;

namespace PixelCrew.UI.Perks
{
    public class PerkWidget : MonoBehaviour, IItemRenderer<string>
    {
        [SerializeField] private GameObject _icon;
        [SerializeField] private GameObject _isLocked;
        [SerializeField] private GameObject _isUsed;
        [SerializeField] private GameObject _isSelected;

        public void SetData(string data, int index)
        {
            throw new System.NotImplementedException();
        }

        public void OnSelect()
        {
            
        }
    }
}