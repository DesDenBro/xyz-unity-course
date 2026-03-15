using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PixelCrew.Common.Tech
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteAnimation : MonoBehaviour
    {
        [SerializeField] private int _frameRate = 10;
        [SerializeField] private string _startSasName;
        [SerializeField] private bool autoStart = true;

        private SpriteAnimationState[] _states;
        private SpriteRenderer _spriteRenderer;
        private SpriteAnimationState _currentState;
        private float _secondsPerFrame;
        private int _currentSpriteIndex;
        private float _nextFrameTime;

        private IReadOnlyDictionary<string, SpriteAnimationState> _statesDict; 
        
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _states = GetComponentsInChildren<SpriteAnimationState>(true);
            _statesDict = _states?.ToDictionary(x => x.Name, y => y) ?? new Dictionary<string, SpriteAnimationState>(0);
        }

        public void Start()
        {
            if (autoStart) SetupAnimation();
        }

        public void SetupAnimation(string startSasName = "")
        {
            if (!string.IsNullOrWhiteSpace(startSasName))
            {
                _startSasName = startSasName;
            }

            if (_statesDict.Count > 0)
            {
                var sasName = string.IsNullOrWhiteSpace(_startSasName) || !_statesDict.ContainsKey(_startSasName) ? _states[0]?.Name : _startSasName;
                SetClip(sasName);
            }
        }

        public void SetClip(string name)
        {
            if (!autoStart) _spriteRenderer.sprite = null;

            name = name?.Trim();
            if (string.IsNullOrEmpty(name)) return;
            if (_statesDict == null || _statesDict.Count == 0 || !_statesDict.TryGetValue(name, out SpriteAnimationState stateVal) || stateVal == null) return;

            _currentState?.TogglePlay(false);
            if (_currentState != null && !_currentState.AllowNext) return;

            _currentSpriteIndex = 0;
            _secondsPerFrame = 1f / _frameRate;
            _nextFrameTime = Time.time + _secondsPerFrame;

            _currentState = stateVal;
            _spriteRenderer.flipX = _currentState.FlipX;
            if (_currentState.IsFixedFlip)
            {
                var tr = _currentState.transform;
                tr.localScale = new Vector3(
                    tr.localScale.x > 0 ? tr.localScale.x : -tr.localScale.x, 
                    tr.localScale.y > 0 ? tr.localScale.y : -tr.localScale.y,
                    tr.localScale.z > 0 ? tr.localScale.z : -tr.localScale.z
                );
            }

            if (autoStart) RunAnimation();
        }

        public void RunAnimation()
        {
            _currentState?.TogglePlay(true);
        }

        private void Update()
        {
            if (_currentState == null || !_currentState.IsPlaying || _nextFrameTime > Time.time) return;

            if (_currentSpriteIndex >= _currentState.Sprites.Length)
            {
                if (_currentState.IsLoop)
                {
                    _currentSpriteIndex = 0;
                }
                else
                {
                    _currentState?.TogglePlay(false);
                    _currentState?.InvokeComplete();
                    _currentState.gameObject.SetActive(false);                    
                    return;
                }
            }

            _spriteRenderer.sprite = _currentState.Sprites[_currentSpriteIndex];
            _nextFrameTime += _secondsPerFrame;
            _currentSpriteIndex++;
        }

        public SpriteAnimationState GetSas(string sasName)
        {
            if (!_statesDict.TryGetValue(sasName, out SpriteAnimationState sas) || sas == null) return null;
            return sas;
        }
    }
}
