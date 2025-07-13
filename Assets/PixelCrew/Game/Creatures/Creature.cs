using PixelCrew.Common;
using PixelCrew.Common.Tech;
using PixelCrew.Components;
using PixelCrew.Model;
using PixelCrew.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.GameObjects.Creatures
{
    public class Creature : MonoBehaviour
    {
        private static readonly int _anim_isGrounded = Animator.StringToHash("is-grounded");
        private static readonly int _anim_isRunning = Animator.StringToHash("is-running");
        private static readonly int _anim_verticalVelocity = Animator.StringToHash("vertical-velocity");
        private static readonly int _anim_triggerHit = Animator.StringToHash("trigger-hit");
        private static readonly int _anim_triggerHealing = Animator.StringToHash("trigger-healing");
        private static readonly int _anim_triggerAttack = Animator.StringToHash("trigger-attack");
        private static readonly int _anim_isDead = Animator.StringToHash("is-dead");

        [Header("Base params")]
        [SerializeField] private bool _invertScale;
        [SerializeField] private float _speed;
        [SerializeField] protected float jumpSpeed;
        [SerializeField] private float _damageJumpSpeed;
        [SerializeField] private int _baseDamage;
        [SerializeField] private CreatureState _creatureStateInfo = CreatureState.Alive;

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

        protected virtual bool _IsFalling => _rigidbody.velocity.y < 0f;
        protected bool _IsNormalMove => _currentMovement == MovementStateType.Default;
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
        }

        protected virtual void FixedUpdate()
        {
            StartUpdateOperations();
            UpdateOperations();
            EndUpdateOperations();
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

            return yVel;
        }
        protected virtual float CalJumpVelocity(float inputYVel)
        {
            float yVel = inputYVel;
            if (!_IsFalling && _isGrounded) yVel = jumpSpeed;

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
                    _needPlayHardFallSas = true;
                    _maxYInJump = null;
                }
                if (fallDistance > 7.5f)
                {
                    _health.ApplyDamage(5);
                }
            }
        }
        #endregion

        public virtual void SetIsJumping(bool val)
        {
            if (!_isJumpingPressed && val && _isGrounded)
            {
                _needPlayJumpSas = true;
            }
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
            _animator.SetFloat(_anim_verticalVelocity, _rigidbody.velocity.y);
            _animator.SetBool(_anim_isRunning, _direction.x != 0);
            _animator.SetBool(_anim_isGrounded, _isGrounded);
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

        public virtual void MovementDefaultAction() { }
        public virtual void MovementHangAction() { }
        public virtual void MovementGrabAction() { }


        public virtual void TakeDamage()
        {
            _animator.SetTrigger(_anim_triggerHit);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y + _damageJumpSpeed);
        }
        public virtual void TakeHealth()
        {
            _animator.SetTrigger(_anim_triggerHealing);
        }

        public virtual void InitAttack()
        {
            _animator.SetTrigger(_anim_triggerAttack);
        }
        public virtual void OnAttack()
        {
            _attackRange.Check();
        }
        public void DealDamage(GameObject go)
        {
            var hp = go.GetComponent<HealthComponent>();
            if (hp == null) return;            
            hp.ApplyDamage(_Damage);
        }

        public void InitDie()
        {
            if (_creatureStateInfo == CreatureState.Dead) return;

            _creatureStateInfo = CreatureState.Dead;
            _animator.SetBool(_anim_isDead, true);
        }
        public void OnDie()
        {
            _deadParams.SetParams();
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
}
