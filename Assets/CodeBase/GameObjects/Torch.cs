using PixelCrew.Common.Tech;
using UnityEngine;

namespace PixelCrew.GameObjects
{
    [RequireComponent(typeof(SpriteAnimation))]
    public class Torch : OnOffObject
    {
        private SpriteAnimation _spriteAnimation;
        private void Awake()
        {
            _spriteAnimation = GetComponent<SpriteAnimation>();
        }

        public override void TurnOff()
        {
            _spriteAnimation.SetClip("nonfire-idle");
        }
        public override void TurnOn()
        {
            _spriteAnimation.SetClip("idle");
        }
    }
}
