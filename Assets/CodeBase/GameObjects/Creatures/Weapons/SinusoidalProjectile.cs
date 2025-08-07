using UnityEngine;

namespace PixelCrew.Components
{
    public class SinusoidalProjectile : BaseProjectile
    {
        [SerializeField] private float _frequency = 1f;
        [SerializeField] private float _amplitude = 1f;

        private float _originalY;
        private float _time;

        protected override void Start()
        {
            base.Start();
            _originalY = rigidBody.position.y;
            _time = Time.fixedDeltaTime;
        }

        private void FixedUpdate()
        {
            if (isDead) return;

            var position = rigidBody.position;
            position.x += direction * speed;
            position.y = _originalY + Mathf.Sin(_time * _frequency) * _amplitude;
            rigidBody.MovePosition(position);
            _time += Time.fixedDeltaTime; 
        }
    }
}