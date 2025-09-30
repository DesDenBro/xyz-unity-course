using UnityEngine;
using PixelCrew.Utils;

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