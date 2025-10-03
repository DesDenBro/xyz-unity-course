using UnityEngine;
using PixelCrew.Utils;
using PixelCrew.UI.Widgets;
using System.Linq;

namespace PixelCrew.UI
{
    public class AnimatedWindow : MonoBehaviour
    {
        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();

            _animator.SetKeyVal(PixelCrew.GameObjects.AnimationKey.UI.MenuWindow.TriggerShow);
        }

        public void SetButtonsVisible(bool isMainMenu)
        {
            var btns = gameObject.GetComponentsInChildren<CustomButton>().Where(x => x.VisibleConstraint != ButtonMenuVisibleConstraint.None).ToList();
            foreach (var btn in btns)
            {
                if (!isMainMenu && btn.VisibleConstraint == ButtonMenuVisibleConstraint.OnlyMainMenu
                    || isMainMenu && btn.VisibleConstraint == ButtonMenuVisibleConstraint.OnlyGame)
                {
                    btn.gameObject.SetActive(false);
                }
            }
        }

        public void Close()
        {
            _animator.SetKeyVal(PixelCrew.GameObjects.AnimationKey.UI.MenuWindow.TriggerHide);
        }

        public virtual void OnCloseAnimationComplete()
        {
            Destroy(this.gameObject);
        }
    }
}