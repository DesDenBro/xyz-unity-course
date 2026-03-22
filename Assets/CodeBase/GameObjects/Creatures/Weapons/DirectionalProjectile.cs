using UnityEngine;

namespace PixelCrew.Components
{
    public class DirectionalProjectile : BaseProjectile
    {
        public void Launch(Vector2 direction)
        { 
            var rb = GetComponent<Rigidbody2D>();
            rb.AddForce(direction * speed, ForceMode2D.Impulse);
        }
    }
}
