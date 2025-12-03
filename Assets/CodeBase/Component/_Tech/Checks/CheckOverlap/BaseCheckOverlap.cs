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

            float mindelta = -1;
            Collider2D mildeltaCollider = null;

            for (int i = 0; i < size; i++)
            {
                var overlapRes = interactionResult[i];
                if (overlapRes == null) continue;

                if (checkOnlyForward)
                {
                    var newmindelta = ((overlapRes.gameObject.transform.position.x < 0 ? -1 : 1) * overlapRes.gameObject.transform.position.x)
                        - ((gameObject.transform.position.x < 0 ? -1 : 1) * gameObject.transform.position.x);
                    newmindelta = (newmindelta < 0 ? -1 : 1) * newmindelta;
                    if (mindelta > newmindelta || mindelta == -1)
                    {
                        mindelta = newmindelta;
                        mildeltaCollider = overlapRes;
                    }
                }
                else
                {
                    CheckTags(overlapRes);
                }
            }

            if (checkOnlyForward) CheckTags(mildeltaCollider);
        }
        private void CheckTags(Collider2D overlapRes)
        {
            if (overlapRes == null) return;

            var isInTags = tags.Any(tag => overlapRes.CompareTag(tag));
            if (isInTags)
            {
                onOverlap?.Invoke(overlapRes.gameObject);
            }
        }
    }
}
