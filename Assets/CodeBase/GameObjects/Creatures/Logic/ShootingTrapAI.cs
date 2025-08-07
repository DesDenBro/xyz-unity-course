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
        [SerializeField] private Cooldown _rangeCooldown;
        [SerializeField] private LayerCheck _rangeCanAttack;


        private Animator _animator;
        private SpawnActionComponent _sac;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _sac = GetComponent<SpawnActionComponent>();
        }


        private void FixedUpdate()
        {
            if(_meleeCanAttack.IsTouchingLayer)
            {
                if (!_meleeCooldown.IsReady) return;
                _meleeCooldown.Reset();

                InitMeleeAttack();
                return;
            }

            if (_rangeCanAttack.IsTouchingLayer)
            {
                if (!_rangeCooldown.IsReady) return;
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
            _meleeAttack.Check();
        }

        private void InitRangeAttack()
        {
            _animator.SetKeyVal(AnimationKey.ShootingTrap.TriggerRange);
        }
        public void OnRangeAttack()
        {
            _sac.SpawnAction("pearl-throw");
        }
    }
}