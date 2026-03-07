using System.Collections;
using Cinemachine;
using UnityEngine;

namespace PixelCrew.Effects
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraShakeEffect : MonoBehaviour
    {
        [SerializeField] private float _animationTime = 0.3f;
        [SerializeField] private float _frequency = 3f;
        [SerializeField] private float _amplitude = 1f;

        private CinemachineBasicMultiChannelPerlin _camNoise;
        private Coroutine _coroutine;
 
        private void Awake()
        {
            var vCam = GetComponent<CinemachineVirtualCamera>();
            _camNoise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        public void Shake()
        {
            if (_coroutine != null) StopAnimation();
            _coroutine = StartCoroutine(StartAnimation());
        }

        private IEnumerator StartAnimation()
        {
            if (_camNoise != null)
            {
                _camNoise.m_FrequencyGain = _frequency;
                _camNoise.m_AmplitudeGain = _amplitude;
            }
            yield return new WaitForSeconds(_animationTime);
            StopAnimation();
        }

        private void StopAnimation()
        {
            if (_camNoise != null)
            {
                _camNoise.m_FrequencyGain = 0f;
                _camNoise.m_AmplitudeGain = 0f;
            }
            StopCoroutine(_coroutine);
            _coroutine = null;
        }
    }
}