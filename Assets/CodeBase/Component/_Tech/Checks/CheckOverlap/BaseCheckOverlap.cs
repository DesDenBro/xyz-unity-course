using System.Drawing;
using System.Linq;
using UnityEngine;

namespace PixelCrew.Common.Tech
{
    public abstract class BaseCheckOverlap : MonoBehaviour
    {
        [SerializeField] protected LayerMask mask;
        [SerializeField] protected string[] tags;
        [SerializeField] protected UnityEventGameObject onOverlap;
        [SerializeField] protected bool checkOnlyForward = false;
        protected readonly Collider2D[] interactionResult = new Collider2D[10];

        public abstract void Check();
        protected void CheckOnSize(int size)
        {
            if (size == 0) return;

            size = checkOnlyForward ? 1 : size;
            for (int i = 0; i < size; i++)
            {
                var overlapRes = interactionResult[i];
                if (overlapRes == null) continue;

                var isInTags = tags.Any(tag => overlapRes.CompareTag(tag));
                if (isInTags)
                {
                    onOverlap?.Invoke(overlapRes.gameObject);
                }
            }
        }

    }
}
