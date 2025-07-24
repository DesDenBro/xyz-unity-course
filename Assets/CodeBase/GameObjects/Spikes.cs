using PixelCrew.Common;
using PixelCrew.Common.Tech;
using UnityEngine;

namespace PixelCrew.GameObjects
{
    public class Spikes : MonoBehaviour
    {
        private LayerCheck _layerChecker;
        private CheckLineOverlap _lineChecker;

        private void Awake()
        {
            _layerChecker = GetComponent<LayerCheck>();
            _lineChecker = GetComponent<CheckLineOverlap>();
        }

        private void Update()
        {
            if (_layerChecker.IsTouchingLayer)
            {
                _lineChecker.Check();
            }
        }
    }
}
