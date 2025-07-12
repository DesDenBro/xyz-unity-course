using PixelCrew.Common;
using System.Collections;
using UnityEngine;

namespace PixelCrew.GameObjects.Creatures
{
    public class MobAI : MonoBehaviour
    {
        [SerializeField] private LayerCheck _vision;
        [SerializeField] private LayerCheck _canAttack;
        [SerializeField] private float _alarmDelay = 2f;
        [SerializeField] private float _attackCooldown = 1f;
        [SerializeField] private float _confuseCooldown = 3f;

        private bool _isDead = false;
        private Coroutine _current;
        private GameObject _target;

        private Creature _creature;

        private void Awake()
        {
            _creature = GetComponent<Creature>();
        }

        private void Start()
        {
            StartState(Patrolling());
        }

        private void StartState(IEnumerator coroutine)
        {
            _creature.SetDirection(Vector2.zero);
            if (_current != null) StopCoroutine(coroutine);

            _current = StartCoroutine(coroutine);
        }

        private IEnumerator Patrolling()
        {
            yield return null;
        }

        private IEnumerator AgroToHero()
        {
            _creature.SpawnAction("alarm");
            yield return new WaitForSeconds(_alarmDelay);
            StartState(GoToHero());
        }

        private IEnumerator GoToHero()
        {
            bool _isHeroFound = false;
            while (_vision.IsTouchingLayer)
            {
                _isHeroFound = true;
                if (_canAttack.IsTouchingLayer)
                {
                    StartState(Attack());
                }
                else
                {
                    SetDirectionToTarget();
                }
                yield return null;
            }

            if (_isHeroFound) _creature.SpawnAction("question");
            yield return new WaitForSeconds(_confuseCooldown);
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
            var direction = _target.transform.position - transform.position;
            direction.y = 0;
            _creature.SetDirection(direction.normalized);
        }

        public void OnHeroInVision(GameObject go)
        {
            if (_isDead) return;

            _target = go;
            StartState(AgroToHero());
        }

        public void OnDie()
        {
            _isDead = true;
            _creature.InitDie();
            if (_current != null) StopCoroutine(_current);
        }
    }
}
