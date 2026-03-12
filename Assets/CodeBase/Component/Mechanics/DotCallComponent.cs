using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class DotCallComponent : MonoBehaviour
    {
        [SerializeField] private Cooldown _checkPeriod = new Cooldown();
        [SerializeField] private UnityEvent _actions;

        private void Update()
        {
            if (!_checkPeriod.IsReady) return;
            ForceCall();
        }

        public void ForceCall()
        {
            _checkPeriod.Reset();
            _actions?.Invoke();
        }

        public void Reset()
        {
            _checkPeriod.Reset();
        }
    }
}