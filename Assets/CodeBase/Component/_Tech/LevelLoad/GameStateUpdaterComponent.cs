using System;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    [Obsolete]
    public class GameStateUpdaterComponent : MonoBehaviour
    {
        [SerializeField] private UnityEvent _updateFunction;

        public void UpdateState()
        {
            _updateFunction?.Invoke();
        }
    }
}