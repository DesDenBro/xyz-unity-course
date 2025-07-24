using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class MovementStateComponent : MonoBehaviour
    {
        [SerializeField] private MovementStateType _movementState;
        [SerializeField] private UnityEvent _defaultEvent;
        [SerializeField] private UnityEvent _grabEvent;
        [SerializeField] private UnityEvent _hangEvent;

        public MovementStateType SelectedState => _movementState;

        public void SetState(MovementStateType movementState)
        {
            if (movementState == _movementState) return;
            _movementState = movementState;

            switch (_movementState)
            {
                case MovementStateType.Default: _defaultEvent?.Invoke(); break;
                case MovementStateType.Grab: _grabEvent?.Invoke(); break;
                case MovementStateType.Hanging: _hangEvent?.Invoke(); break;
            }
        }

        public void ResetState() => SetState(MovementStateType.Default);
    }

    public enum MovementStateType : byte
    {
        Default = 0,
        Grab = 1,
        Hanging = 2
    }
}
