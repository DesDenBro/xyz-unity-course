using UnityEngine;

namespace PixelCrew.GameObjects
{
    public class Chain : MonoBehaviour
    {
        private SpriteRenderer[] _spriteRenderers;

        private void Awake()
        {
            _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        }

        public void HangIn()
        {
            if (_spriteRenderers == null || _spriteRenderers.Length == 0) return;

            foreach (var sr in _spriteRenderers)
            {
                sr.sortingLayerName = "after object 1";
            }
        }

        public void HangOut()
        {
            if (_spriteRenderers == null || _spriteRenderers.Length == 0) return;

            foreach (var sr in _spriteRenderers)
            {
                sr.sortingLayerName = "before object 3";
            }
        }
    }
}
