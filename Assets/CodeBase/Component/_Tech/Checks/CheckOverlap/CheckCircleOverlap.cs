using PixelCrew.Utils;
using UnityEditor;
using UnityEngine;

namespace PixelCrew.Common.Tech
{
    public class CheckCircleOverlap : BaseCheckOverlap
    {
        [SerializeField] private float _radius = 1f;

        public float Radius => _radius;

        private void OnDrawGizmosSelected()
        {
            Handles.color = HandlesUtils.TransparentRed;
            Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius);
        }

        public override void Check()
        {
            var size = Physics2D.OverlapCircleNonAlloc(transform.position, _radius, interactionResult, mask);
            CheckOnSize(size);
        }

        public void SetRange(float radius)
        {
            _radius = radius;
        }
    }
}
