using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace PixelCrew.UI.Postprocessing
{
    public class VignetteController : MonoBehaviour
    {
        private Vignette _vignette;
        private Coroutine _coroutine;

        private void Awake()
        {
            GetComponent<Volume>().profile.TryGet(out _vignette);
            _vignette.intensity.value = 0;
        }

        [ContextMenu("Show")]
        public void Show()
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(Change(0, 0.6f, 0.1f));
        }

        [ContextMenu("Hide")]
        public void Hide()
        {
            if (_coroutine != null) StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(Change(0.6f, 0, -0.1f));
        }

        private IEnumerator Change(float from, float to, float step)
        {
            var currentState = from;
            do
            {
                currentState += step;
                _vignette.intensity.value = currentState;
                yield return new WaitForSeconds(0.07f);
            }
            while (currentState * (step > 0 ? 1 : -1) < to);
        }
    }
}