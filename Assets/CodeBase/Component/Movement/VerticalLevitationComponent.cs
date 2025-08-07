using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PixelCrew.Components
{
    public class VerticalLevitationComponent : MonoBehaviour
    {
        [SerializeField] private float _frequency = 1f;
        [SerializeField] private float _amplitude = 1f;
        [SerializeField] private bool _randomize = true;

        private float _originalY;
        private Rigidbody2D _rigidBody;
        private float _seed;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            _originalY = _rigidBody.position.y;
            if (_randomize)
            {
                _seed = Random.value * Mathf.PI * 2;
            }
        }

        private void FixedUpdate()
        {
            var pos = _rigidBody.position;
            pos.y = _originalY + Mathf.Sin(_seed + Time.time * _frequency) * _amplitude;
            _rigidBody.MovePosition(pos);
        }
    }
}