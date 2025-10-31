using PixelCrew.Utils;

namespace PixelCrew.GameObjects
{
    public static class AnimationKeys
    {
        public static class Creature
        {
            public static readonly FloatAnimationKey VerticalVelocity = new FloatAnimationKey("vertical-velocity");

            public static readonly BoolAnimationKey IsGrounded = new BoolAnimationKey("is-grounded");
            public static readonly BoolAnimationKey IsRunning = new BoolAnimationKey("is-running");
            public static readonly BoolAnimationKey IsDead = new BoolAnimationKey("is-dead");
            public static readonly BoolAnimationKey IsClimb = new BoolAnimationKey("is-climb");

            public static readonly TriggerAnimationKey TriggerHit = new TriggerAnimationKey("trigger-hit");
            public static readonly TriggerAnimationKey TriggerHealing = new TriggerAnimationKey("trigger-healing");
            public static readonly TriggerAnimationKey TriggerAttack = new TriggerAnimationKey("trigger-attack");
            public static readonly TriggerAnimationKey TriggerThrow = new TriggerAnimationKey("trigger-throw");
        }

        public static class Hero
        {

        }

        public static class ShootingTrap
        {
            public static readonly TriggerAnimationKey TriggerRange = new TriggerAnimationKey("trigger-range");
            public static readonly TriggerAnimationKey TriggerMelee = new TriggerAnimationKey("trigger-melee");
            public static readonly TriggerAnimationKey TriggerHit = new TriggerAnimationKey("trigger-hit");

        }

        public static class UI
        {
            public static class MenuWindow
            {
                public static readonly TriggerAnimationKey TriggerShow = new TriggerAnimationKey("trigger-show");
                public static readonly TriggerAnimationKey TriggerHide = new TriggerAnimationKey("trigger-hide");
            }

            public static class DialogBox
            {
                public static readonly BoolAnimationKey IsOpen = new BoolAnimationKey("is-open");
            }
        }
    }
}