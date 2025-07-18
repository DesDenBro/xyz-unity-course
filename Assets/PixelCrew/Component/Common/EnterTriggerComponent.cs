using PixelCrew.Common.Tech;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components
{
    public class EnterTriggerComponent : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] private LayerMask _layer = ~0;
        [SerializeField] private UnityEventGameObject[] _actions;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.IsInLayer(_layer)) return;
            if (!string.IsNullOrWhiteSpace(_tag) && !other.gameObject.CompareTag(_tag)) return;

            foreach (var action in _actions)
            {
                action?.Invoke(other.gameObject);
            }
        }
    }
}
