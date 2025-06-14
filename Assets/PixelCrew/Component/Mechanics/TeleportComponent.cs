
using UnityEngine;

namespace PixelCrew.Components
{
    public class TeleportComponent : MonoBehaviour
    {
        [SerializeField] private bool _isActive;
        [SerializeField] private Transform _destTransform;

        private SwitchComponent _switchComponent;

        public void Awake()
        {
            _switchComponent = GetComponent<SwitchComponent>();
        }

        public void FixedUpdate()
        {
            _isActive = _switchComponent.State;
        }

        public void Teleport(GameObject target)
        {
            if (!_isActive) return;
            target.transform.position = _destTransform.position;
        }
    }
}
