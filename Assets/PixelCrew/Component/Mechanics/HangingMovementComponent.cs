using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Components
{
    public class HangingMovementComponent : MonoBehaviour
    {
        private float? _yPos = null;
        private MovementStateComponent _targetMovementState;
        private Rigidbody2D _rigidbody;

        private void FixedUpdate()
        {
            if (!_yPos.HasValue) return;

            var yVel = _rigidbody.velocity.y < -0.1f ? -0.1f : _rigidbody.velocity.y - 0.1f;
            _rigidbody.velocity = new Vector2(0, yVel);
        }

        public void HangObject(GameObject _target)
        {
            _yPos = null;

            _targetMovementState = _target.GetComponent<MovementStateComponent>();
            if (_targetMovementState == null) return;

            _rigidbody = _target.GetComponent<Rigidbody2D>();
            if (_rigidbody == null) return;

            _yPos = _target.transform.position.y;
            _targetMovementState.SetState(MovementStateType.Hanging);
        }

        public void LeaveObject(GameObject _target)
        {
            _yPos = null;
            _targetMovementState.ResetState();
        }
    }
}
