using PixelCrew.Common;
using PixelCrew.Common.Tech;
using PixelCrew.Components;
using PixelCrew.Model;
using PixelCrew.Model.Definitions;
using PixelCrew.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private SpawnComponent _throwSpawn;

        [Header("Hero animators")]
        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _disarmed;


        private bool _isDoubleJumpActive;
        private ActionInteractComponent _actionInteract;
        //private Collider2D[] _interactionResult = new Collider2D[1];
        private GameSession _session;
        private InventoryComponent _inventory;
        private string _selectedThrow;

        private bool _IsGrabMove => _currentMovement == MovementStateType.Grab;
        private bool _IsHangingMove => _currentMovement == MovementStateType.Hanging;
        protected override bool _IsFalling => base._IsFalling && !_IsHangingMove;
        protected override int _Damage => base._Damage * _weapon?.Damage ?? 1;
        private bool _IsArmed => _weapon != null;


        protected override void Awake()
        {
            base.Awake();
            _actionInteract = GetComponent<ActionInteractComponent>();
            _inventory = GetComponent<InventoryComponent>();
        }
        protected override void Start()
        {
            base.Start();
            SetSessionData();
            _inventory.InventoryData.onInventoryChanged += OnInventoryChanged;
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            _inventory.InventoryData.onInventoryChanged -= OnInventoryChanged;
        }

        private void OnInventoryChanged(string id, int value)
        {
            if (id == InventoryItemName.Sword)
            {
                ArmWeapon(_inventory.GetItem(InventoryItemName.Sword)?.Prefab);
            }
        }
        public void AddInInventory(string id, int count)
        {
            if (_inventory == null) return;

            _inventory.ChangeInventoryItemCount(id, count);
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
            var moneyCount = _inventory.Count(InventoryItemName.Money);
            if (moneyCount <= 0) return;

            var coinsToDrop = Mathf.Min(moneyCount, 5);
            if (coinsToDrop == 0) return;

            _inventory.ChangeInventoryItemCount(InventoryItemName.Money, -coinsToDrop);

            var defaultBurst = _hitParticles.emission.GetBurst(0);
            defaultBurst.count = coinsToDrop;
            _hitParticles.emission.SetBurst(0, defaultBurst);

            _hitParticles.gameObject.SetActive(true);
            _hitParticles.Play();
        }
        public void OnInteract(bool isPressed)
        {
            _actionInteract.SetIsPressed(isPressed);
            _doInteractionCheck.Check();
        }


        public void ArmWeapon(GameObject prefab, bool woAddThrows = false)
        {
            Weapon newWeapon = null;
            if (prefab != null) newWeapon = prefab.GetComponent<Weapon>();
            
            if (newWeapon != null)
            {
                if (!_IsArmed)
                {
                    _weapon = newWeapon;
                    _animator.runtimeAnimatorController = _armed;
                    if (!woAddThrows) _inventory.ChangeInventoryItemCount(InventoryItemName.SwordThrow, 5);
                }
                else
                {
                    if (!woAddThrows) _inventory.ChangeInventoryItemCount(InventoryItemName.SwordThrow, 1);
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


        public override void OnHeal()
        {
            var selectedHeal = _session.QuickInventory?.SelectedItem?.Id;
            var healPotion = _inventory.GetItem(selectedHeal);
            if (healPotion == null || healPotion.Prefab == null || _inventory.Count(selectedHeal) == 0) return;

            var healDef = DefsFacade.I.Items.Get(selectedHeal);
            if (!healDef.HasTag(ItemTag.Heal)) return;

            base.OnHeal();

            var ts = healPotion.Prefab.GetComponent<ThingSpecification>();
            _health.RecoverHealth(ts.HealthPoints);

            _inventory.ChangeInventoryItemCount(selectedHeal, -1);
        }

        public override void InitThrow(ThrowType type = ThrowType.Once)
        {
            if (!SelectedThrowAvailable(out _selectedThrow)) return;
            base.InitThrow(type);
        }
        public override void OnThrow()
        {
            base.OnThrow();
            //ArmWeapon(null);
        }
        protected override IEnumerator Throw(int count)
        {
            if (string.IsNullOrWhiteSpace(_selectedThrow)) yield return null;
            var throwableDef = DefsFacade.I.ThrowableItems.Get(_selectedThrow);

            for (int i = 0; i < count; i++)
            {
                if (!_inventory.CheckItemCountToEvent(throwableDef.Id, -1)) break;
                if (!_inventory.ChangeInventoryItemCount(throwableDef.Id, -1)) break;

                _throwSpawn.SetPrefab(throwableDef.Projectile);
                SpawnAction(throwableDef.ActionName);
                yield return new WaitForSeconds(0.1f);
            }

            yield return null;
        }
        private bool SelectedThrowAvailable(out string selectedThrow)
        {
            selectedThrow = null;

            var throwableId = _session.QuickInventory?.SelectedItem?.Id;
            var throwableDef = DefsFacade.I.ThrowableItems.Get(throwableId);

            switch (throwableDef.Id)
            {
                case InventoryItemName.SwordThrow:
                    if (_IsArmed && _inventory.Count(InventoryItemName.SwordThrow) > 0)
                    {
                        selectedThrow = InventoryItemName.SwordThrow;
                        return true;
                    }
                    break;
                case InventoryItemName.PearlThrow:
                    if (_inventory.Count(InventoryItemName.PearlThrow) > 0)
                    {
                        selectedThrow = InventoryItemName.PearlThrow;
                        return true;
                    }
                    break;
                default: break;
            }

            return false;
        }


        public void InitNextItem()
        {
            _session.QuickInventory.SetNextItem();
        }


        private void HigthlightInteractble() => _possibleInteractionCheck.Check();

        public virtual void SetSessionData()
        {
            _session = GameSessionSearch.Get(FindObjectsOfType<GameSession>);

            if (_session == null) return;

            var levelData = _session.LevelsData.Get(SceneManager.GetActiveScene().name);
            if (levelData != null) 
            {
                var checkpoint = FindObjectsOfType<CheckPointUpdateComponent>().FirstOrDefault(x => x.Id == levelData.CheckPointName);
                if (checkpoint != null) transform.position = checkpoint.gameObject.transform.position;
            }

            if (_session.PlayerData.MaxHealth > 0) _health.SetMaxHealth(_session.PlayerData.MaxHealth);
            if (_session.PlayerData.Health > 0) _health.SetHealth(_session.PlayerData.Health);

            _inventory.SetInventory(_session.PlayerData.Inventory.Clone());
            _session.ReloadLinks();

            ArmWeapon(_inventory.GetItem(InventoryItemName.Sword)?.Prefab, true);
        }
        public virtual void UpdateSessionData(string checkPointName)
        {
            if (_session == null) return;

            _session.LevelsData.SaveHeroPosition(SceneManager.GetActiveScene().name, checkPointName);

            _session.PlayerData.MaxHealth = _health.MaxHealth;
            _session.PlayerData.Health = _health.Health;

            _session.PlayerData.Inventory = _inventory.InventoryData.Clone();
        }
    }
}