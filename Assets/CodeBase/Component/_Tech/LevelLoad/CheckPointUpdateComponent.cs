using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class CheckPointUpdateComponent : MonoBehaviour
    {
        [SerializeField] private UnityEvent _actionAfterCheckPoint;

        public void UpdateSessionData()
        {
            var gmuComps = FindObjectsOfType<GameStateUpdaterComponent>();
            foreach (var gmu in gmuComps)
            {
                if (gmu == null) continue;
                gmu.UpdateState();
            }

            _actionAfterCheckPoint?.Invoke();
        }
    }
}
