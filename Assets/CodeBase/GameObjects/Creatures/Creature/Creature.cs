﻿using PixelCrew.Common;
using PixelCrew.Common.Tech;
using PixelCrew.Components;
using PixelCrew.Model;
using PixelCrew.Utils;
using PixelCrew.GameObjects;
using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace PixelCrew.GameObjects.Creatures
{
    public class Creature : MonoBehaviour
    {
        [Header("Base tech params")]
        [SerializeField] private bool _invertScale;

        [Header("Base params")]
        [SerializeField] private CreatureState _creatureStateInfo = CreatureState.Alive;
        [SerializeField] private float _speed;
        [SerializeField] protected float jumpSpeed;
        [SerializeField] private float _damageJumpSpeed;
        [SerializeField] private int _baseDamage;
        [SerializeField] private Cooldown _throwCooldown;

        [Header("Base checks")]
        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] private CheckCircleOverlap _attackRange;
        
        
        private float? _maxYInJump = null;
        protected bool _isJumpingPressed;
        protected bool _needPlayJumpSas;
        protected bool _needPlayHardFallSas;
        private DeadParamsComponent _deadParams;
        protected SpawnActionComponent _actionParticles;
        protected MovementStateComponent _movementState;
        protected InventoryComponent _inventory;
        protected HealthComponent _health;
        protected Animator _animator;
        protected Rigidbody2D _rigidbody;
        protected Vector2 _direction;
        protected MovementStateType _currentMovement;
        protected bool _isGrounded;
        protected GameSession _session;
        private ThrowType _lastThrowType;
        private Coroutine _throwCoroutine;

        protected virtual bool _IsFalling => _rigidbody.velocity.y < -1f && _IsNormalMove;
        protected bool _IsNormalMove => _currentMovement == MovementStateType.Default;
        protected bool _IsAlive => _creatureStateInfo != CreatureState.Dead;
        protected virtual int _Damage => _baseDamage;


        protected virtual void Awake()
        {
            _deadParams = GetComponent<DeadParamsComponent>();
            _actionParticles = GetComponent<SpawnActionComponent>();
            _movementState = GetComponent<MovementStateComponent>();
            _health = GetComponent<HealthComponent>();
            _inventory = GetComponent<InventoryComponent>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }


        protected virtual void Start()
        {
            _session = FindObjectsOfType<GameSession>().Where(x => !x.Disposed).FirstOrDefault();
            _session.Data.Inventory.onInventoryChanged += OnInventoryChanged;
        }
        protected virtual void FixedUpdate()
        {
            StartUpdateOperations();
            UpdateOperations();
            EndUpdateOperations();
        }
        protected virtual void OnDestroy()
        {
            _session.Data.Inventory.onInventoryChanged -= OnInventoryChanged;
        }


        protected virtual void StartUpdateOperations()
        {
            _currentMovement = _movementState.SelectedState;
            _isGrounded = _groundCheck.IsTouchingLayer;
        }
        protected virtual void UpdateOperations()
        {
            CalcPosition();
            CalcFall();
        }
        protected virtual void EndUpdateOperations()
        {
            UpdateSpriteDirection();
            UpdateAnimatorParamsState();
            PlayStateAnimationsByState();
        }

        protected virtual void OnInventoryChanged(string id, int value)
        {

        }


        #region CalcPosition
        protected void CalcPosition()
        {
            var xVel = CalcXVelocity();
            var yVel = CalcYVelocity();
            _rigidbody.velocity = new Vector2(xVel, yVel);
        }

        protected virtual float CalcXVelocity()
        {
            return _direction.x * _speed;
        }
        protected virtual float CalcYVelocity()
        {
            var yVel = _rigidbody.velocity.y;

            if (_isJumpingPressed)
            {
                yVel = CalJumpVelocity(yVel);
            }
            else if (_rigidbody.velocity.y > 0)
            {
                yVel *= 0.5f;
            }

            if (yVel > 10)
            {
                yVel = 10;
            }

            return yVel;
        }
        protected virtual float CalJumpVelocity(float inputYVel)
        {
            float yVel = inputYVel;
            if (!_IsFalling && _isGrounded && _IsNormalMove)
            {
                yVel = jumpSpeed;
                _needPlayJumpSas = true;
            }

            return yVel;
        }
        private void CalcFall()
        {
            // пока герой в полете - вычисляем его максимальную высоту
            if (_IsFalling && (!_maxYInJump.HasValue || _maxYInJump.Value < _rigidbody.position.y))
            {
                _maxYInJump = _rigidbody.position.y;
            }
            // а когда он приземлился и ускорение стало нулевым проверяем, что высота подходит под анимацию
            else if (_isGrounded && _rigidbody.velocity.y == 0 && _maxYInJump.HasValue)
            {
                var fallDistance = _maxYInJump.Value - _rigidbody.position.y;
                if (fallDistance > 2.5f)
                {
                    if (_IsNormalMove)
                    {
                        _needPlayHardFallSas = true;
                    }
                    _maxYInJump = null;
                }
                if (fallDistance > 7.5f && _IsNormalMove)
                {
                    _health.ApplyDamage(5);
                }
            }
        }
        #endregion

        public virtual void SetIsJumping(bool val)
        {
            _isJumpingPressed = val;
        }


        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }
        private void UpdateSpriteDirection()
        {
            if (_direction.x != 0)
            {
                var multiplier = _invertScale ? -1 : 1;
                var isToLeft = _direction.x < 0;
                transform.localScale = new Vector3((isToLeft ? -1 : 1) * multiplier, 1, 1);
            }
        }
        private void UpdateAnimatorParamsState()
        {
            _animator.SetKeyVal(AnimationKey.Creature.VerticalVelocity, _rigidbody.velocity.y);
            _animator.SetKeyVal(AnimationKey.Creature.IsRunning, _direction.x != 0);
            _animator.SetKeyVal(AnimationKey.Creature.IsGrounded, _isGrounded);
        }
        private void PlayStateAnimationsByState()
        {
            // Анимация партиклов тяжелого падения
            if (_needPlayHardFallSas)
            {
                SpawnAction("fall");
                _needPlayHardFallSas = false;
            }

            // Анимация партиклов прыжка
            if (_needPlayJumpSas)
            {
                SpawnAction("jump");
                _needPlayJumpSas = false;
            }
        }

        public void SpawnAction(string sasName) => _actionParticles.SpawnAction(sasName);

        public virtual void MovementDefaultAction()
        {
            _animator.SetKeyVal(AnimationKey.Creature.IsClimb, false);
        }
        public virtual void MovementHangAction() 
        {
            _animator.SetKeyVal(AnimationKey.Creature.IsClimb, true);
        }
        public virtual void MovementGrabAction() { }


        public virtual void TakeDamage()
        {
            if (!_IsAlive) return;

            _animator.SetKeyVal(AnimationKey.Creature.TriggerHit);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpSpeed);
        }
        public virtual void TakeHealth()
        {
            if (!_IsAlive) return;

            _animator.SetKeyVal(AnimationKey.Creature.TriggerHealing);
        }

        public virtual void InitAttack()
        {
            if (!_IsAlive) return;

            _animator.SetKeyVal(AnimationKey.Creature.TriggerAttack);
        }
        public virtual void OnAttack()
        {
            _attackRange.Check();
        }


        public virtual void InitThrow(ThrowType type = ThrowType.Once)
        {
            if (!_IsAlive) return;

            if (!_throwCooldown.IsReady || _inventory.ThrowsCount <= 0) return;
            _throwCooldown.Reset();

            _lastThrowType = type;
            _animator.SetKeyVal(AnimationKey.Creature.TriggerThrow);
        }
        public virtual void OnThrow()
        {
            if (_throwCoroutine != null)
            {
                StopCoroutine(_throwCoroutine);
            }

            var countToThrow = 0;
            switch (_lastThrowType)
            {
                case ThrowType.Once:
                    countToThrow = 1;
                    break;
                case ThrowType.Multi:
                    countToThrow = 3;
                    break;
            }
            if (countToThrow == 0) return;

            _throwCoroutine = StartCoroutine(Throw(countToThrow));
        }
        protected virtual IEnumerator Throw(int count)
        {
            yield return null;
        }


        public void DealDamage(GameObject go)
        {
            var hp = go.GetComponent<HealthComponent>();
            if (hp == null) return;            
            hp.ApplyDamage(_Damage);
        }

        public void InitDie()
        {
            if (!_IsAlive) return;

            _creatureStateInfo = CreatureState.Dead;
            _animator.SetKeyVal(AnimationKey.Creature.IsDead, true);
            SetDirection(Vector3.zero);
        }
        public void OnDie()
        {
            if (_deadParams != null)
            {
                _deadParams.SetParams();
            }
        }
        

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.color = _isGrounded ? HandlesUtils.TransparentGreen : HandlesUtils.TransparentRed;
            Handles.DrawSolidDisc(transform.position + new Vector3(0, -0.15f), Vector3.forward, 0.29f);
        }
#endif
    }

    public enum CreatureState : byte
    {
        Alive = 0,
        Dead = 1
    }

    public enum ThrowType : byte
    {
        Once = 0,
        Multi = 1
    }
}
