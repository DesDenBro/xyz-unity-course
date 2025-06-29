﻿using PixelCrew.Common;
using PixelCrew.Common.Tech;
using PixelCrew.Components;
using UnityEngine;

namespace PixelCrew.GameObjects
{
    public class Hero : MonoBehaviour
    {
        private static readonly int _anim_isGrounded = Animator.StringToHash("is-grounded");
        private static readonly int _anim_isRunning = Animator.StringToHash("is-running");
        private static readonly int _anim_verticalVelocity = Animator.StringToHash("vertical-velocity");
        private static readonly int _anim_triggerHit = Animator.StringToHash("trigger-hit");
        private static readonly int _anim_triggerHealing = Animator.StringToHash("trigger-healing");

        [SerializeField] private float _speed;
        [SerializeField] private float _jumpSpeed;
        [SerializeField] private float _damageJumpSpeed;
        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] private float _interactionRadius;
        [SerializeField] private bool _isDoubleJumpEnable;
        [SerializeField] private LayerMask _interactionLayer;
        [SerializeField] private SpawnComponent _footActionParticles;
        [SerializeField] private ParticleSystem _hitParticles;

        private MovementStateComponent _movementState;
        private InventoryComponent _inventory;
        private HealthComponent _health;
        private Animator _animator;
        private Rigidbody2D _rigidbody;
        private Vector2 _direction;

        private MovementStateType _currentMovement;
        private bool _isJumpingPressed;
        private bool _needPlayJumpSas;
        private bool _needPlayHardFallSas;
        private bool _isGrounded;
        private bool _isDoubleJumpActive;
        private float? _maxYInJump = null;
        private Collider2D[] _interactionResult = new Collider2D[1];

        private bool _IsFalling => _rigidbody.velocity.y < 0.001f && !_IsHangingMove;
        private bool _IsNormalMove => _currentMovement == MovementStateType.Default;
        private bool _IsGrabMove => _currentMovement == MovementStateType.Grab;
        private bool _IsHangingMove => _currentMovement == MovementStateType.Hanging;


        private void Awake()
        {
            _movementState = GetComponent<MovementStateComponent>();
            _health = GetComponent<HealthComponent>();
            _inventory = GetComponent<InventoryComponent>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        public void FixedUpdate()
        {
            StartUpdateOperations();
            UpdateOperations();
            EndUpdateOperations();
        }

        private void StartUpdateOperations()
        {
            _currentMovement = _movementState.SelectedState;
            _isGrounded = _groundCheck.IsTouchingLayer || _IsHangingMove;
            HigthlightInteractble();
        }
        private void UpdateOperations()
        {
            CalcPosition();
        }
        private void EndUpdateOperations()
        {
            UpdateSpriteDirection();
            UpdateAnimatorParamsState();
            PlayStateAnimationsByState();
        }


        private void CalcPosition()
        {
            var xVel = CalcXVelocity();
            var yVel = CalcYVelocity();
            _rigidbody.velocity = new Vector2(xVel, yVel);
        }
        private float CalcXVelocity()
        {
            if (_IsHangingMove) return 0f;

            return _direction.x * _speed;
        }
        private float CalcYVelocity()
        {
            if (_IsHangingMove && !_isJumpingPressed) return _direction.y * 3f; // ползем наверх

            var yVel = _rigidbody.velocity.y;

            if (_isGrounded && _isDoubleJumpEnable) _isDoubleJumpActive = true;
            if (_isJumpingPressed)
            {
                yVel = CalJumpVelocity(yVel);
            }
            else if (_rigidbody.velocity.y > 0)
            {
                yVel *= 0.5f;
            }

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

            return yVel;
        }
        private float CalJumpVelocity(float yVel)
        {
            if (!_IsFalling) return yVel;

            if (_isGrounded)
            {
                yVel = _jumpSpeed;
            }
            else if (_isDoubleJumpActive)
            {
                yVel = _jumpSpeed;
                _isDoubleJumpActive = false;
                _needPlayJumpSas = true;
            }

            return yVel;
        }

        private void UpdateSpriteDirection()
        {
            if (_direction.x != 0)
            {
                var isToLeft = _direction.x < 0;
                transform.localScale = new Vector3(isToLeft ? -1 : 1, 1, 1);
            }
        }
        private void UpdateAnimatorParamsState()
        {
            _animator.SetFloat(_anim_verticalVelocity, _rigidbody.velocity.y);
            _animator.SetBool(_anim_isRunning, _direction.x != 0);
            _animator.SetBool(_anim_isGrounded, _isGrounded);
        }


        public void MovementDefaultAction()
        {

        }
        public void MovementHangAction()
        {
            _isJumpingPressed = false;
        }
        public void MovementGrabAction()
        {
            _isJumpingPressed = false;
        }


        private void PlayStateAnimationsByState()
        {
            // Анимация партиклов тяжелого падения
            if (_needPlayHardFallSas)
            {
                SpawnFootAction("fall");
                _needPlayHardFallSas = false;
            }

            // Анимация партиклов прыжка
            if (_needPlayJumpSas)
            {
                SpawnFootAction("jump");
                _needPlayJumpSas = false;
            }
        }


        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }    
        public void SetIsJumping(bool val)
        {
            if (_IsGrabMove) return; // пока что-то тащим не можем прыгать
            if (_IsHangingMove) { Interact(val); _isDoubleJumpActive = true; }

            if (!_isJumpingPressed && val && _isGrounded)
            {
                _needPlayJumpSas = true;
            }

            _isJumpingPressed = val;
        }
        public void TakeDamage()
        {
            _animator.SetTrigger(_anim_triggerHit);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y + _damageJumpSpeed);

            if (_inventory.MoneyCount > 0)
            {
                SpawnCoins();
            }
        }
        private void SpawnCoins()
        {
            var coinsToDrop = Mathf.Min(_inventory.MoneyCount, 5);
            if (coinsToDrop == 0) return;

            _inventory.ChangeMoneyAmount(-coinsToDrop);

            var defaultBurst = _hitParticles.emission.GetBurst(0);
            defaultBurst.count = coinsToDrop;
            _hitParticles.emission.SetBurst(0, defaultBurst);

            _hitParticles.gameObject.SetActive(true);
            _hitParticles.Play();
        }

        public void TakeHealth()
        {
            _animator.SetTrigger(_anim_triggerHealing);
        }

        private void HigthlightInteractble() => OperationWithInteractable(InteractableOperation.Higthlight, false);
        public void Interact(bool isPressed) => OperationWithInteractable(InteractableOperation.Activate, isPressed);

        private void OperationWithInteractable(InteractableOperation operationType, bool isPressed)
        {
            var size = Physics2D.OverlapCircleNonAlloc(transform.position, _interactionRadius, _interactionResult, _interactionLayer);
            for (int i = 0; i < size; i++)
            {
                var interactable = _interactionResult[i].GetComponent<InteractableComponent>();
                if (interactable == null) continue;

                switch (operationType)
                {
                    case InteractableOperation.Higthlight:
                        interactable.Highlight();
                        break;
                    case InteractableOperation.Activate:
                        interactable.Interact(this.gameObject, isPressed);
                        break;
                }
            }
        }


        private void OnDrawGizmos()
        {
            //Debug.DrawRay(transform.position, Vector2.down, IsGrounded() ? Color.green : Color.red);
            Gizmos.color = _isGrounded ? Color.green : Color.red;
            Gizmos.DrawSphere(transform.position + new Vector3(0, -0.15f), 0.29f);
        }

        public void Say()
        {

        }

        public void SpawnFootAction(string sasName)
        {
            var obj = _footActionParticles.Spawn();

            var saComp = obj?.GetComponent<SpriteAnimation>();
            if (saComp == null) return;

            saComp.SetStartSasName(sasName);

            // добавляем вектор отклонения относительно нахождения стартовой точки спавна из sas
            var sasTransform = saComp.GetSasTransform(sasName);
            if (sasTransform != null)
            {
                obj.transform.position += new Vector3(
                    sasTransform.localPosition.x * sasTransform.parent.lossyScale.x,
                    sasTransform.localPosition.y * sasTransform.parent.lossyScale.y,
                    sasTransform.localPosition.z * sasTransform.parent.lossyScale.z
                );
            }
        }
    }

}