using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PixelCrew.Effects
{
    public class InfiniteBackground : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _container;

        private Bounds _containerBounds;
        private Bounds _allBounds;

        private Vector3 _boundsToTransformDelta;
        private Vector3 _containerDelta;

        void Start()
        {
            var sprites = _container.GetComponentsInChildren<SpriteRenderer>();
            for (var sprite in sprites)
            {
                _containerBounds.Encapsulate(sprite.bounds);
            }

            _allBounds = _containerBounds;

            _boundsToTransformDelta = transofrm.position - _allBounds.center;
            _containerDelta = _container.position - _allBounds.center;
        }

        private void OnDrawGizmos()
        {
            // 16:55
        }
    }
}