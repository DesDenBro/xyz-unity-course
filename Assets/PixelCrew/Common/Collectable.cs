using PixelCrew.Common.Tech;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Common
{
    public class Collectable : MonoBehaviour
    {
        [SerializeField] private bool _isActiveToCollect = true;
        [SerializeField] private UnityEventGameObject[] _collectActions;

        public void Collect(GameObject target)
        {
            if (!_isActiveToCollect) return;
            _isActiveToCollect = false;

            foreach (var collectAction in _collectActions)
            {
                collectAction?.Invoke(target);
            }
        }
    }
}