using UnityEngine;

namespace PixelCrew.Utils
{
    public static class AnimatorExtensions
    {
        public static void SetKeyVal(this Animator animator, AnimationKey key, object arg = null)
        {
            if (key is BoolAnimationKey bkey) { bkey?.SetVal(animator, (bool)arg); return; }
            if (key is TriggerAnimationKey tkey) { tkey?.SetVal(animator); return; }
            if (key is FloatAnimationKey fkey) { fkey?.SetVal(animator, (float)arg); return; }
            if (key is IntAnimationKey ikey) { ikey?.SetVal(animator, (int)arg); return; }
        }
    }

    public abstract class AnimationKey
    {
        public string Name { get; }
        public int Id { get; }

        public AnimationKey(string name)
        {
            Name = name;
            Id = Animator.StringToHash(name);
        }
    }
    public class BoolAnimationKey : AnimationKey
    {
        public BoolAnimationKey(string name) : base(name) { }

        public void SetVal(Animator animator, bool val)
        {
            animator?.SetBool(Id, val);
        }
    }
    public class TriggerAnimationKey : AnimationKey
    {
        public TriggerAnimationKey(string name) : base(name) { }

        public void SetVal(Animator animator)
        {
            animator?.SetTrigger(Id);
        }
    }
    public class FloatAnimationKey : AnimationKey
    {
        public FloatAnimationKey(string name) : base(name) { }

        public void SetVal(Animator animator, float val)
        {
            animator?.SetFloat(Id, val);
        }
    }
    public class IntAnimationKey : AnimationKey
    {
        public IntAnimationKey(string name) : base(name) { }

        public void SetVal(Animator animator, int val)
        {
            animator?.SetInteger(Id, val);
        }
    }
}