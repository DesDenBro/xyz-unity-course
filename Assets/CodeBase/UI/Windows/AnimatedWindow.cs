using PixelCrew.GameObjects.Creatures;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils;
using System.Linq;
using UnityEngine;

namespace PixelCrew.UI
{
    public class AnimatedWindow : MonoBehaviour
    {
        private Animator _animator;

        protected virtual void Start()
        {
            _animator = GetComponent<Animator>();
            _animator.SetKeyVal(PixelCrew.GameObjects.AnimationKeys.UI.MenuWindow.TriggerShow);

            var heroMovementLock = FindObjectOfType<HeroMovementLock>();
            if (heroMovementLock != null) heroMovementLock.SetLock(true);
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
            var heroMovementLock = FindObjectOfType<HeroMovementLock>();
            if (heroMovementLock != null) heroMovementLock.SetLock(false);

            _animator.SetKeyVal(PixelCrew.GameObjects.AnimationKeys.UI.MenuWindow.TriggerHide);
        }

        public virtual void OnCloseAnimationComplete()
        {
            Destroy(this.gameObject);
        }
    }
}