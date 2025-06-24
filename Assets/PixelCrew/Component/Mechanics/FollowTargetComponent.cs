using UnityEngine;

namespace PixelCrew.Components
{
    public class FollowTargetComponent : MonoBehaviour
    {
        private Rigidbody2D _rigidBody;

        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
        }

        public void FollowToObject(GameObject _target)
        {
            var _targetRigidBody = _target.GetComponent<Rigidbody2D>();
            if (_targetRigidBody == null) return;

            _rigidBody.velocity = _targetRigidBody.velocity;
        }
    }
}
