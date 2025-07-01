using UnityEngine;

namespace PixelCrew.Components
{
    public class OutlineSettings : MonoBehaviour
    {
        [SerializeField] private Color _outlineColor = Color.white;
        [SerializeField] private float _outlineWidth = 1;
        [SerializeField] private bool _isEnabled = false;

        private MaterialPropertyBlock _propertyBlock;
        private SpriteRenderer _spriteRenderer;
        
        public bool IsEnabled => _isEnabled;

        public void Enable()
        {
            _isEnabled = true;
            OnValidate();
        }

        public void Disable()
        {
            _isEnabled = false;
            OnValidate();
        }

        private void OnValidate()
        {
            if (_spriteRenderer == null || _propertyBlock == null)
            {
                _propertyBlock = new MaterialPropertyBlock();
                _spriteRenderer = GetComponent<SpriteRenderer>();

                _isEnabled = true;
            }

            var outlineWidth = _outlineWidth;
            if (!_isEnabled)
            {
                outlineWidth = 0;
            }

            _spriteRenderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetColor("_OutlineColor", _outlineColor);
            _propertyBlock.SetFloat("_OutlineWidth", outlineWidth);
            _propertyBlock.SetFloat("_TextureSize", _spriteRenderer.sprite.texture.width);
            _spriteRenderer.SetPropertyBlock(_propertyBlock);
        }
    }
}