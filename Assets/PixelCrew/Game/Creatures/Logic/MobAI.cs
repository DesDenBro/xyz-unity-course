using PixelCrew.Common;
using System.Collections;
using UnityEngine;

namespace PixelCrew.GameObjects.Creatures
{
    public class MobAI : MonoBehaviour
    {
        [SerializeField] private LayerCheck _vision;
        [SerializeField] private LayerCheck _canAttack;
        [SerializeField] private LayerCheck _underLegsChecker;
        [SerializeField] private LayerCheck _wallChecker;
        [SerializeField] private float _alarmDelay = 2f;
        [SerializeField] private float _attackCooldown = 1f;
        [SerializeField] private float _confuseCooldown = 3f;

        private bool _isDead = false;
        private Coroutine _current;
        private GameObject _target;
        private Creature _creature;
        private BasePatrol _patrol;


        private void Awake()
        {
            _creature = GetComponent<Creature>();
            _patrol = GetComponent<BasePatrol>();
        }
        private void Start()
        {
            StartState(_patrol.DoPatrol());
        }


        public void OnHeroInVision(GameObject go)
        {
            _target = go;
            StartState(AgroToHero());
        }
        public void OnDie()
        {
            _isDead = true;
            _creature.InitDie();
            if (_current != null) StopCoroutine(_current);
        }


        private void StartState(IEnumerator coroutine)
        {
            _creature.SetDirection(Vector2.zero);
            if (_current != null) StopCoroutine(_current);
            if (_isDead) return;

            _current = StartCoroutine(coroutine);
        }
        private IEnumerator AgroToHero()
        {
            _creature.SpawnAction("alarm");
            yield return new WaitForSeconds(_alarmDelay);
            StartState(GoToHero());
        }
        private IEnumerator GoToHero()
        {
            while (_vision.IsTouchingLayer)
            {
                if (!_underLegsChecker.IsTouchingLayer || _wallChecker.IsTouchingLayer)
                {
                    _creature.SetDirection(Vector3.zero);
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    if (_canAttack.IsTouchingLayer)
                    {
                        StartState(Attack());
                    }
                    else
                    {
                        SetDirectionToTarget();
                    }
                }
                yield return null;
            }

            _creature.SetDirection(Vector3.zero);
            _creature.SpawnAction("question");
            yield return new WaitForSeconds(_confuseCooldown);
            StartState(_patrol.DoPatrol());
        }
        private IEnumerator Attack()
        {
            while (_canAttack.IsTouchingLayer)
            {
                _creature.InitAttack();
                yield return new WaitForSeconds(_attackCooldown);
            }

            StartState(GoToHero());
        }
        private void SetDirectionToTarget()
        {
            _creature.SetDirection(MobAIUtils.CalcDirectionX(transform, _target.transform));
        }
    }

    public static class MobAIUtils
    {
        public static Vector3 CalcDirectionV2(Transform start, Transform target, bool normalized = true)
        {
            return CalcDirectionV2(start.position, target.position, normalized);
        }
        public static Vector3 CalcDirectionX(Transform start, Transform target, bool normalized = true)
        {
            return CalcDirectionX(start.position, target.position, normalized);
        }
        public static Vector3 CalcDirectionY(Transform start, Transform target, bool normalized = true)
        {
            return CalcDirectionY(start.position, target.position, normalized);
        }

        public static Vector3 CalcDirectionV2(Vector3 start, Vector3 target, bool normalized = true)
        {
            var mobPos = new Vector2(start.x, 0);
            var pointPos = new Vector2(target.x, 0);
            var direction = pointPos - mobPos;
            return normalized ? direction.normalized : direction;
        }
        public static Vector3 CalcDirectionX(Vector3 start, Vector3 target, bool normalized = true)
        {
            var direction = CalcDirectionV2(start, target, normalized);
            direction.y = 0;
            return direction;
        }
        public static Vector3 CalcDirectionY(Vector3 start, Vector3 target, bool normalized = true)
        {
            var direction = CalcDirectionV2(start, target, normalized);
            direction.x = 0;
            return direction;
        }
    }
}
