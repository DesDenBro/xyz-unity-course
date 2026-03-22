using PixelCrew.Utils;
using System.Collections;
using UnityEngine;

namespace PixelCrew.GameObjects.Creatures.Boss
{
    public class FloodController : MonoBehaviour
    {
        [SerializeField] private Animator _floodAnimator;
        [SerializeField] private float _floodTime;

        public void StartFlooding()
        {
            StartCoroutine(Animate());
        }

        private IEnumerator Animate()
        {
            _floodAnimator.SetKeyVal(AnimationKeys.BossPatric.Water.IsFlooding, true);
            yield return new WaitForSeconds(_floodTime);
            _floodAnimator.SetKeyVal(AnimationKeys.BossPatric.Water.IsFlooding, false);
        }
    }
}