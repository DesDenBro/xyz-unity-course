using PixelCrew.Common.Tech;
using UnityEngine;

namespace PixelCrew.GameObjects.Creatures.Boss
{
    public class BossCrabby : MonoBehaviour
    {
        [SerializeField] CheckCircleOverlap _leftClaw;
        [SerializeField] CheckCircleOverlap _rightClaw;

        public void CheckClawAttack()
        {
            _leftClaw.Check();
            _rightClaw.Check();
        }
    }
}
