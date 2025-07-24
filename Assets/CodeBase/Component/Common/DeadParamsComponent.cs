using UnityEngine;

namespace PixelCrew.Components
{
    public class DeadParamsComponent : MonoBehaviour
    {
        [SerializeField] string _layerMask;
        [SerializeField] CapsuleCollider2D _capsuleColliderConfig;

        public void SetParams()
        {
            if (!string.IsNullOrWhiteSpace(_layerMask))
            {
                this.gameObject.layer = LayerMask.NameToLayer(_layerMask);
            }

            if (_capsuleColliderConfig != null)
            {
                var colliderFromGO = this.gameObject.GetComponent<CapsuleCollider2D>();
                if (colliderFromGO != null)
                {
                    colliderFromGO.offset = _capsuleColliderConfig.offset;
                    colliderFromGO.size = _capsuleColliderConfig.size;
                }
            }
        }
    }
}
