using UnityEngine;

namespace PixelCrew.GameObjects
{
    public class SpriteReplacer : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _source;
        [SerializeField] private SpriteRenderer _target;

        public void ReplaceSprite()
        {
            _target.sprite = _source.sprite;
        }
    }  
}
