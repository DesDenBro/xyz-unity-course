using UnityEngine;

namespace PixelCrew.Components
{
    public class DestroyObjectComponent : MonoBehaviour
    {
        [SerializeField] private bool _selfDestroyAfterTime = false;
        [SerializeField] private Cooldown _selfDestroyTime = new Cooldown(10);

        public void Awake()
        {
            if (_selfDestroyAfterTime)
            {
                _selfDestroyTime.Reset();
            }
        }

        public void FixedUpdate()
        {
            if (_selfDestroyAfterTime && _selfDestroyTime.IsReady)
            {
                SelfDestroy();
            }
        }

        public void DestroyObj(GameObject target)
        {
            Destroy(target);
        }
        public void SelfDestroy()
        {
            DestroyObj(this.gameObject);
        }
    }
}