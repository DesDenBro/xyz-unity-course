using UnityEngine;

namespace PixelCrew.Components
{
    public class BaseProjectile : MonoBehaviour
    {
        [SerializeField] protected float speed;
        [SerializeField] private bool _invertX;

        protected bool isDead;
        protected Rigidbody2D rigidBody;
        protected int direction;

        protected virtual void Start()
        {
            rigidBody = GetComponent<Rigidbody2D>();
            var mode = _invertX ? -1 : 1;
            direction = mode * transform.lossyScale.x > 0 ? 1 : -1;
        }

        public void Stop()
        {
            isDead = true;

            var collider = GetComponent<Collider2D>();
            if (collider != null) collider.enabled = false;

            rigidBody.velocity = Vector2.zero;
        }
    }
}