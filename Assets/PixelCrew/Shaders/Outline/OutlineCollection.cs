using UnityEngine;

namespace PixelCrew.Components
{
    public class OutlineCollection : MonoBehaviour
    {
        private OutlineSettings[] outlines;
        private bool _isEnabled;

        private void Start()
        {
            outlines = GetComponentsInChildren<OutlineSettings>();
            Disable();
        }

        public bool IsEnabled => _isEnabled;

        public void Enable()
        {
            if (outlines == null) return;

            _isEnabled = true;
            foreach (var outline in outlines)
            {
                if (outline == null) continue;  
                outline.Enable();
            }
        }
        public void Disable()
        {
            if (outlines == null) return;

            _isEnabled = false;
            foreach (var outline in outlines)
            {
                if (outline == null) continue;
                outline.Disable();
            }
        }
    }
}
