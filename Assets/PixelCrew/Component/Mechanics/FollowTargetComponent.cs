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

            _rigidBody.velocity = new Vector2(
                _targetRigidBody.velocity.x + (_targetRigidBody.velocity.x > 0 ? 0.2f : -0.2f),
                _targetRigidBody.velocity.y + (_targetRigidBody.velocity.y > 0 ? 0.2f : -0.2f)
            );
        }
    }
}
