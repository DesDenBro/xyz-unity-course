using PixelCrew.Common;
using PixelCrew.Common.Tech;
using PixelCrew.Components;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.GameObjects.Creatures
{ 
    public class ShootingTrapAI : MonoBehaviour
    {
        [Header("Melee")]
        [SerializeField] private Cooldown _meleeCooldown;
        [SerializeField] private LayerCheck _meleeCanAttack;
        [SerializeField] private CheckCircleOverlap _meleeAttack;

        [Header("Range")]
        [SerializeField] private GameObject _rangeCaller;
        [SerializeField] private Cooldown _rangeCooldown;
        [SerializeField] private LayerCheck _rangeCanAttack;

        private Animator _animator;
        private SpawnActionComponent _sac;
        private bool _attackAfterCallerCooldown = false;
        private Cooldown _isRangeDoneCooldown = new Cooldown(0.2f);
        private Cooldown _rangeCallerCooldown = new Cooldown(0.3f);
        private bool _isRangeDone = false;

        public bool InRangeCooldown => !_rangeCooldown.IsReady;
        public bool InMeleeCooldown => !_meleeCooldown.IsReady;
        public bool IsRangeDone => _isRangeDone;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _sac = GetComponent<SpawnActionComponent>();
        }

        private void FixedUpdate()
        {
            // нужно, чтобы зависимые успели увидеть факт выстрела
            if (_isRangeDoneCooldown.IsReady) _isRangeDone = false;

            if (_meleeCanAttack != null && _meleeCanAttack.IsTouchingLayer)
            {
                if (InMeleeCooldown) return;

                _meleeCooldown.Reset();
                InitMeleeAttack();

                return;
            }

            if (_rangeCaller != null)
            {
                var trapAI = _rangeCaller.GetComponent<ShootingTrapAI>();
                if (trapAI != null)
                {
                    if (trapAI.IsRangeDone && !_attackAfterCallerCooldown)
                    {
                        _attackAfterCallerCooldown = true;
                        _rangeCallerCooldown.Reset();
                    }
                    if (_rangeCallerCooldown.IsReady && !InRangeCooldown && _attackAfterCallerCooldown)
                    {
                        _attackAfterCallerCooldown = false;
                        _rangeCooldown.Reset();
                        InitRangeAttack();
                    }
                }

                return;
            }

            if (_rangeCanAttack != null && _rangeCanAttack.IsTouchingLayer)
            {
                if (InRangeCooldown) return;

                _rangeCooldown.Reset();
                InitRangeAttack();

                return;
            }
        }

        private void InitMeleeAttack()
        {
            _animator.SetKeyVal(AnimationKey.ShootingTrap.TriggerMelee);
        }
        public void OnMeleeAttack()
        {
            if (_meleeAttack != null)
            {
                _meleeAttack.Check();
            }
        }

        private void InitRangeAttack()
        {
            _animator.SetKeyVal(AnimationKey.ShootingTrap.TriggerRange);
        }
        public void OnRangeAttack()
        {
            _isRangeDone = true;
            _isRangeDoneCooldown.Reset();

            _sac.SpawnAction("throw");
        }
    }
}