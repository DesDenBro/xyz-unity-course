using PixelCrew.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace PixelCrew.Common.Tech
{
    public class CheckLineOverlap : BaseCheckOverlap
    {
        [SerializeField] private float _toLeftLength = 1f;
        [SerializeField] private float _toLeftRight = 1f;

        private void OnDrawGizmosSelected()
        {
            Handles.color = HandlesUtils.Green;
            Handles.DrawLine(CalcVectorToLeft(), CalcVectorToRigth());
        }

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
