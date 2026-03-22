using PixelCrew.GameObjects.Creatures;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Components
{
    public class ShowTargetComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _delay = 0.5f;
        [SerializeField] private UnityEvent _onDelay;
        [SerializeField] private ShowTargetController _controller;

        private HeroMovementLock _heroMovementLock;
        private Coroutine _coroutine;

        private void Awake()
        {
            _heroMovementLock = FindObjectOfType<HeroMovementLock>();
        }

        private void OnValidate()
        {
            if (_controller == null) _controller = FindObjectOfType<ShowTargetController>();
        }

        public void Play()
        {
            if (_heroMovementLock != null) _heroMovementLock.SetLock(true);

            _controller.SetPosition(_target.position);
            _controller.SetState(true);

            if (_coroutine != null) StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(WaitAndReturn());
        }

        private IEnumerator WaitAndReturn()
        {
            yield return new WaitForSeconds(_delay);

            if (_heroMovementLock != null) _heroMovementLock.SetLock(false);
            _onDelay?.Invoke();
            _controller.SetState(false);
        }
    }
}