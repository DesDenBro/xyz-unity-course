using PixelCrew.Common.Tech;
using System.Collections;
using UnityEngine;

namespace PixelCrew.GameObjects.Creatures
{
    public class PointPatrol : BasePatrol
    {
        [SerializeField] private Transform[] _points;
        [SerializeField] private float _treshold = 1f;
        [SerializeField] private CheckLineOverlap _visionChecker;

        private Creature _creature;
        private int _destinationPointIndex;

        public void Awake()
        {
            _creature = GetComponent<Creature>();
        }

        public override IEnumerator DoPatrol()
        {
            while (enabled)
            {
                if (IsOnPoint())
                {
                    _destinationPointIndex = (int)Mathf.Repeat(_destinationPointIndex + 1, _points.Length);
                }

                _creature.SetDirection(MobAIUtils.CalcDirectionX(transform, _points[_destinationPointIndex]));
                _visionChecker.Check();

                yield return null;
            }
        }

        private bool IsOnPoint()
        {
            return MobAIUtils.CalcDirectionX(transform, _points[_destinationPointIndex], false).magnitude < _treshold;
        }
    }
}
