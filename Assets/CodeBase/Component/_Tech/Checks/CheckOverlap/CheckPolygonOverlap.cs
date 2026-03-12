using UnityEngine;

namespace PixelCrew.Common.Tech
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class CheckPolygonOverlap : BaseCheckOverlap
    {
        private BoxCollider2D _boxCollider;

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider2D>();
        }

        public override void Check()
        {
            var size = Physics2D.OverlapBoxNonAlloc(_boxCollider.bounds.center, _boxCollider.size, _boxCollider.transform.eulerAngles.z, interactionResult, mask);
            CheckOnSize(size);
        }
    }
}
