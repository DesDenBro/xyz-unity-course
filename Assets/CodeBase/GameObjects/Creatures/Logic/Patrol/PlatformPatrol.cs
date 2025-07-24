using PixelCrew.Common;
using PixelCrew.Common.Tech;
using System.Collections;
using UnityEngine;

namespace PixelCrew.GameObjects.Creatures
{
    public class PlatformPatrol : BasePatrol
    {
        [SerializeField] private LayerCheck _underLegsChecker;
        [SerializeField] private LayerCheck _wallChecker;
        [SerializeField] private CheckLineOverlap _visionChecker;

        private Creature _creature;

        public void Awake()
        {
            _creature = GetComponent<Creature>();
        }

        public override IEnumerator DoPatrol()
        {
            var currentDirect = 1;
            while (enabled)
            {
                if (!_underLegsChecker.IsTouchingLayer || _wallChecker.IsTouchingLayer)
                {
                    _creature.SetDirection(Vector3.zero);
                    currentDirect = currentDirect * -1;
                }

                _creature.SetDirection(new Vector2(currentDirect, 0));
                _visionChecker.Check();

                yield return null;
            }
        }
    }
}
