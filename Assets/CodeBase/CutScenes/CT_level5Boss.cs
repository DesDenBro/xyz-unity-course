using Cinemachine;
using PixelCrew.Components;
using PixelCrew.Components.Dialogs;
using PixelCrew.GameObjects;
using PixelCrew.GameObjects.Creatures;
using PixelCrew.UI.Widgets;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.CutScenes
{
    public class CT_level5Boss : MonoBehaviour
    {
        [Header("Scene")]
        [SerializeField] AudioSource _mainTheme;
        [SerializeField] AudioSource _bossTheme;
        [SerializeField] CinemachineConfiner _confiner;
        [SerializeField] PolygonCollider2D _defaultConfinerCollider;
        [SerializeField] PolygonCollider2D _bossConfinerCollider;
        [SerializeField] HeroActionsComponent _heroActionsComponent;

        [Header("Main")]
        [SerializeField] GameObject _boss;
        Animator _bossAnimator;
        [SerializeField] BossHpWidget _bossHpWidget;

        [Header("Phase0")]
        [SerializeField] ShowDialogComponent _startDialogs;

        [Header("Phase1")]
        [SerializeField] GameObject _p1_triggerJump;

        [Header("Phase2")]
        [SerializeField] ShowDialogComponent _startPhase2Dialogs;
        [SerializeField] GameObject _p2_jumpDamage;

        [Header("Phase3")]

        [Header("Phase4")]
        [SerializeField] ShowDialogComponent _endDialogs;

        private void Awake()
        {
            _bossAnimator = _boss.GetComponent<Animator>();
        }

        public void StartScene()
        {
            _confiner.m_BoundingShape2D = _bossConfinerCollider;
            _heroActionsComponent.SetImmune(true);
            _heroActionsComponent.SetInputLock(true);

            _mainTheme.Pause();
            _bossTheme.Play();

            _bossAnimator.SetKeyVal(AnimationKeys.Boss.Crabby.TriggerPhase0Run);
            _startDialogs.Show();
        }

        public void StartPhase1()
        {
            _bossHpWidget.ShowUI();
            _boss.GetComponent<HealthComponent>().SetImmune(false);
            _heroActionsComponent.SetImmune(false);
            _heroActionsComponent.SetInputLock(false);
            _p1_triggerJump.SetActive(true);
            _bossAnimator.SetKeyVal(AnimationKeys.Boss.Crabby.TriggerPhase1);
        }
        public void EndPhase1()
        {
            _p1_triggerJump.SetActive(false);

            _heroActionsComponent.SetImmune(true);
            _heroActionsComponent.SetInputLock(true);

            _p2_jumpDamage.gameObject.SetActive(true);

            _startPhase2Dialogs.Show();
        }

        public void StartPhase2()
        {
            _boss.GetComponent<HealthComponent>().SetImmune(false);
            _heroActionsComponent.SetImmune(false);
            _heroActionsComponent.SetInputLock(false);

            _bossAnimator.SetKeyVal(AnimationKeys.Boss.Crabby.TriggerPhase2);
        }
        public void EndPhase2()
        {

        }

        public void StartPhase3()
        {
            EndPhase2();


        }
        public void EndPhase3()
        {

        }

        public void StartPhase4()
        {
            EndPhase3();

            _bossHpWidget.HideUI();

            _mainTheme.Play();
            _bossTheme.Pause();

            _confiner.m_BoundingShape2D = _defaultConfinerCollider;
        }
    }
}