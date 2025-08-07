using UnityEngine;

namespace PixelCrew.Components
{
    public class Projectile : BaseProjectile
    {
        protected override void Start()
        {
            base.Start();
            var force = new Vector2(direction * speed, 0);
            rigidBody.AddForce(force, ForceMode2D.Impulse);
        }
    }
}
