using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Common.Tech
{
    public class CheckCircleOverlap : BaseCheckOverlap
    {
        [SerializeField] private float _radius = 1f;

        public float Radius => _radius;

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = HandlesUtils.TransparentRed;
            UnityEditor.Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius);
        }
#endif

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
