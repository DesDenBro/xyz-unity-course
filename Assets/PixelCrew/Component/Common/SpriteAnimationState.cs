using UnityEngine;
using UnityEngine.Events;

namespace PixelCrew.Common.Tech
{
    public class SpriteAnimationState : MonoBehaviour
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private bool _flipX;
        [SerializeField] private bool _isFixedFlip;
        [SerializeField] private bool _isLoop;
        [SerializeField] private bool _allowNext;
        [SerializeField] private UnityEvent _onComplete;

        private bool _isPlaying = true;

        public string Name => _name;
        public Sprite[] Sprites => _sprites;
        public bool FlipX => _flipX;
        public bool IsFixedFlip => _isFixedFlip;
        public bool AllowNext => _allowNext;
        public bool IsLoop => _isLoop;
        public bool IsPlaying => _isPlaying;

        public void TogglePlay(bool state)
        {
            _isPlaying = state;
        }
        public void InvokeComplete()
        {
            _onComplete?.Invoke();
        }
    }
}
