using PixelCrew.Components;
using UnityEngine;

namespace PixelCrew.GameObjects
{
    public class Chain : MonoBehaviour
    {
        [SerializeField] private InteractableComponent _interactable;

        private ChainBlock[] _chainBlocks;

        private void Awake()
        {
            _chainBlocks = GetComponentsInChildren<ChainBlock>();
            var collider = _interactable.GetComponent<Collider2D>();
            //collider.offset = 
        }

        public void HangIn()
        {
            if (_chainBlocks == null || _chainBlocks.Length == 0) return;

            foreach (var cb in _chainBlocks)
            {
                var sr = cb.GetComponent<SpriteRenderer>();
                if (sr == null) continue;
                if (sr.sortingLayerName != "before object 3") continue;

                sr.sortingLayerName = "after object 1";
            }
        }

        public void HangOut()
        {
            if (_chainBlocks == null || _chainBlocks.Length == 0) return;

            foreach (ChainBlock cb in _chainBlocks)
            {
                var sr = cb.GetComponent<SpriteRenderer>();
                if (sr == null) continue;
                if (sr.sortingLayerName != "after object 1") continue;

                sr.sortingLayerName = "before object 3";
            }
        }
    }
}
