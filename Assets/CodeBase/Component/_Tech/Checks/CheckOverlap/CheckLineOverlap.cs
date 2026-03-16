using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Common.Tech
{
    public class CheckLineOverlap : BaseCheckOverlap
    {
        [SerializeField] private float _toLeftLength = 1f;
        [SerializeField] private float _toLeftRight = 1f;

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = HandlesUtils.Green;
            UnityEditor.Handles.DrawLine(CalcVectorToLeft(), CalcVectorToRigth());
        }
#endif

        public override void Check()
        {
            var size = Physics2D.OverlapAreaNonAlloc(CalcVectorToLeft(), CalcVectorToRigth(), interactionResult, mask);
            CheckOnSize(size);
        }

        private Vector2 CalcVectorToLeft()
        {
            return transform.position + new Vector3(-_toLeftLength, 0) * transform.lossyScale.x;
        }
        private Vector2 CalcVectorToRigth()
        {
            return transform.position + new Vector3(_toLeftRight, 0) * transform.lossyScale.x;
        }
    }
}
