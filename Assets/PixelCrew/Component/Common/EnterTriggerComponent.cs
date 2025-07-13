using PixelCrew.Common.Tech;
using UnityEngine;

namespace PixelCrew.Components
{
    public class EnterTriggerComponent : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] private UnityEventGameObject[] _actions;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(_tag))
            {
                foreach (var action in _actions)
                {
                    action?.Invoke(other.gameObject);
                }
            }
        }
    }
}
