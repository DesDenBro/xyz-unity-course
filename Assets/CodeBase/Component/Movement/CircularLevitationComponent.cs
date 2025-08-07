using System.Linq;
using UnityEngine;

namespace PixelCrew.Components
{
    public class CircularLevitationComponent : MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;
        [SerializeField] private float _speed = 1f;

        private Rigidbody2D[] _rigidBodies;
        private float _time;

        private void Awake() => AwakeCall();
        private void AwakeCall()
        {
            _rigidBodies = GetComponentsInChildren<Rigidbody2D>();
            for (var i = 0; i < _rigidBodies.Length; i++) 
            {
                _rigidBodies[i].MovePosition(transform.position);
                var vert = _rigidBodies[i].gameObject.GetComponent<VerticalLevitationComponent>();
                if (vert != null) vert.enabled = false;
            }
        }

        private void FixedUpdate() => FixedUpdateCall(false);
        private void FixedUpdateCall(bool isDemo)
        {
            if (!isDemo && (_rigidBodies == null || _rigidBodies.Length == 0 || _rigidBodies.All(x => x == null)))
            {
                Destroy(this.gameObject);
                return;
            }

            var circlePos = transform.position;
            var btwDist = 2 * Mathf.PI / _rigidBodies.Length;
            for (var i = 0; i < _rigidBodies.Length; i++)
            {
                var rb = _rigidBodies[i];
                if (rb == null) continue;

                var pos = new Vector2(
                    circlePos.x + Mathf.Cos(btwDist * i + _time * _speed) * _radius,
                    circlePos.y + Mathf.Sin(btwDist * i + _time * _speed) * _radius
                );
                if (isDemo) rb.transform.position = pos;
                else rb.MovePosition(pos);
            }

            if (!isDemo) _time += Time.deltaTime;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            AwakeCall();
            FixedUpdateCall(true);
        }
#endif
    }
}
