using PixelCrew.Common.Tech;
using PixelCrew.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.GameObjects.Creatures
{
    public class Hero : Creature
    {
        [Header("Hero params")]
        [SerializeField] private bool _isDoubleJumpEnable;

        [Header("Hero checks")]
        [SerializeField] protected CheckCircleOverlap _possibleInteractionCheck;
        [SerializeField] protected CheckCircleOverlap _doInteractionCheck;

        [Header("Hero object links")]
        [SerializeField] private Weapon _weapon;
        [SerializeField] private ParticleSystem _hitParticles;

        [Header("Hero animators")]
        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _disarmed;


        private bool _isDoubleJumpActive;
        private ActionInteractComponent _actionInteract;
        private Collider2D[] _interactionResult = new Collider2D[1];

        private bool _IsGrabMove => _currentMovement == MovementStateType.Grab;
        private bool _IsHangingMove => _currentMovement == MovementStateType.Hanging;
        protected override bool _IsFalling => base._IsFalling && !_IsHangingMove;
        protected override int _Damage => base._Damage * _weapon?.Damage ?? 1;
        private bool _IsArmed => _weapon != null;


        protected override void Awake()
        {
            base.Awake();
            _actionInteract = GetComponent<ActionInteractComponent>();
        }
        protected override void Start()
        {
            base.Start();
            SetSessionData();
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }


        protected override float CalcXVelocity()
        {
            if (_IsHangingMove) return 0f;

            return base.CalcXVelocity();
        }
        protected override float CalcYVelocity()
        {
            if (_IsHangingMove && !_isJumpingPressed) return _direction.y * 3f; // ползем по вертикали
            if (_isGrounded && _isDoubleJumpEnable) _isDoubleJumpActive = true;

            return base.CalcYVelocity();
        }
        protected override float CalJumpVelocity(float inputYVel)
        {
            var yVel = base.CalJumpVelocity(inputYVel);
            if (_IsFalling && !_isGrounded && _isDoubleJumpActive)
            {
                yVel = jumpSpeed;
                _isDoubleJumpActive = false;
                _needPlayJumpSas = true;
            }

            return yVel;
        }

        public override void SetIsJumping(bool val)
        {
            if (_IsGrabMove) return; // пока что-то тащим не можем прыгать
            if (_IsHangingMove && val)
            {
                OnInteract(val);
                _isDoubleJumpActive = true;
            }

            base.SetIsJumping(val);
        }


        protected override void StartUpdateOperations()
        {
            base.StartUpdateOperations();
            _isGrounded = _isGrounded || _IsHangingMove;
            HigthlightInteractble();
        }
        protected override void UpdateOperations() // переопределено на будущее
        {
            base.UpdateOperations();
        }
        protected override void EndUpdateOperations() // переопределено на будущее
        {
            base.EndUpdateOperations();
        }


        public override void MovementDefaultAction()
        {
            base.MovementDefaultAction();
        }
        public override void MovementHangAction()
        {
            base.MovementHangAction();
            _isJumpingPressed = false;
        }
        public override void MovementGrabAction()
        {
            base.MovementGrabAction();
            _isJumpingPressed = false;
        }

         
        public override void TakeDamage()
        {
            base.TakeDamage();
            SpawnCoins();
        }
        private void SpawnCoins()
        {
            if (_inventory.MoneyCount <= 0) return;

            var coinsToDrop = Mathf.Min(_inventory.MoneyCount, 5);
            if (coinsToDrop == 0) return;

            _inventory.ChangeMoneyAmount(-coinsToDrop);

            var defaultBurst = _hitParticles.emission.GetBurst(0);
            defaultBurst.count = coinsToDrop;
            _hitParticles.emission.SetBurst(0, defaultBurst);

            _hitParticles.gameObject.SetActive(true);
            _hitParticles.Play();
        }



        private void HigthlightInteractble() => _possibleInteractionCheck.Check();
        public void OnInteract(bool isPressed)
        {
            _actionInteract.SetIsPressed(isPressed);
            _doInteractionCheck.Check();
        }

        public void ArmWeapon(Weapon newWeapon, bool woAddThrows = false)
        {
            if (newWeapon != null)
            {
                if (_weapon == null)
                {
                    _weapon = newWeapon;
                    _animator.runtimeAnimatorController = _armed;
                    if (!woAddThrows) _inventory.ChangeThrowsAmount(5);
                }
                else
                {
                    if (!woAddThrows) _inventory.ChangeThrowsAmount(1);
                }
            }
            else
            {
                _weapon = null;
                _animator.runtimeAnimatorController = _disarmed;
            }
        }
        public override void InitAttack()
        {
            if (!_IsArmed) return;
            base.InitAttack();
        }


        public override void InitThrow(ThrowType type = ThrowType.Once)
        {
            if (!_IsArmed) return;
            base.InitThrow(type);
        }
        public override void OnThrow()
        {
            base.OnThrow();
            //ArmWeapon(null);
        }
        protected override IEnumerator Throw(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (!_inventory.CheckThrowsCountToEvent(-1)) break;
                if (!_inventory.ChangeThrowsAmount(-1)) break;

                SpawnAction("sword-throw");
                yield return new WaitForSeconds(0.1f);
            }

            yield return null;
        }


        public virtual void SetSessionData()
        {
            if (_session == null) return;

            if (_session.Data.PositionOnLevel != null
                && _session.Data.PositionOnLevel.TryGetValue(SceneManager.GetActiveScene().name, out Vector3 position)
                && position != Vector3.zero)
            {
                transform.position = position;
            }
            _health.SetMaxHealth(_session.Data.MaxHealth);
            _health.SetHealth(_session.Data.Health);
            _inventory.SetMoney(_session.Data.Coins);
            _inventory.SetKeys(_session.Data.Keys);
            _inventory.SetThrows(_session.Data.Throws);
            ArmWeapon(_session.Data.Weapon, true);
        }
        public virtual void UpdateSessionData()
        {
            if (_session == null) return;

            var sceneName = SceneManager.GetActiveScene().name;
            if (_session.Data.PositionOnLevel == null) _session.Data.PositionOnLevel = new Dictionary<string, Vector3>();
            if (!_session.Data.PositionOnLevel.ContainsKey(sceneName)) { _session.Data.PositionOnLevel.Add(sceneName, transform.position); }
            else { _session.Data.PositionOnLevel[sceneName] = transform.position; }

            _session.Data.MaxHealth = _health.MaxHealth;
            _session.Data.Health = _health.Health;
            _session.Data.Coins = _inventory.MoneyCount;
            _session.Data.Keys = _inventory.KeysCount;
            _session.Data.Throws = _inventory.ThrowsCount;
            _session.Data.IsArmed = _IsArmed;
            _session.Data.Weapon = _weapon;
        }
    }
}