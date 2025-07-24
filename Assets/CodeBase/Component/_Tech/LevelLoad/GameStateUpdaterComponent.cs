using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class GameStateUpdaterComponent : MonoBehaviour
    {
        [SerializeField] private UnityEvent _updateFunction;

        public void UpdateState()
        {
            _updateFunction?.Invoke();
        }
    }
}