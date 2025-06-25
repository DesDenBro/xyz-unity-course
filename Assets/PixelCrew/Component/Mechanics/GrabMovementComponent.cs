using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class GrabMovementComponent : MonoBehaviour
    {
        private MovementStateComponent _targetMovementState;
        private Rigidbody2D _targetRigidbody;
        private Rigidbody2D _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (_targetRigidbody == null) return;
             
            _rigidbody.velocity = new Vector2(
                _targetRigidbody.velocity.x + (_targetRigidbody.velocity.x > 0 ? 0.2f : -0.2f),
                _targetRigidbody.velocity.y + (_targetRigidbody.velocity.y > 0 ? 0.2f : -0.2f)
            );
        }

        public void GrabObject(GameObject _target)
        {
            _targetMovementState = _target.GetComponent<MovementStateComponent>();
            if (_targetMovementState == null) return;

            _targetRigidbody = _target.GetComponent<Rigidbody2D>();
            if (_targetRigidbody == null) return;

            _targetMovementState.SetState(MovementStateType.Grab);
        }

        public void LeaveObject(GameObject _target)
        {
            _targetRigidbody = null;
            _targetMovementState.ResetState();
        }
    }
}