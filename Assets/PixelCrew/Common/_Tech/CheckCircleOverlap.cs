using PixelCrew.Utils;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace PixelCrew.Common.Tech
{
    public class CheckCircleOverlap : MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;
        [SerializeField] private LayerMask _mask;
        [SerializeField] private string[] _tags;
        [SerializeField] private UnityEventGameObject _onOverlap;
        private readonly Collider2D[] _interactionResult = new Collider2D[10];

        private void OnDrawGizmosSelected()
        {
            Handles.color = HandlesUtils.TransparentRed;
            Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius);
        }

        public void Check()
        {
            var size = Physics2D.OverlapCircleNonAlloc(transform.position, _radius, _interactionResult, _mask);
            for (int i = 0; i < size; i++)
            {
                var overlapRes = _interactionResult[i];
                if (overlapRes == null) continue;

                var isInTags = _tags.Any(tag => overlapRes.CompareTag(tag));
                if (isInTags)
                {
                    _onOverlap?.Invoke(overlapRes.gameObject);
                }
            }
        }
    }
}
