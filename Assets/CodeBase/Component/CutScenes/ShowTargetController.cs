using Cinemachine;
using PixelCrew.GameObjects;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Components
{
    public class ShowTargetController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private CinemachineVirtualCamera _cutSceneCamera;

        public void SetPosition(Vector3 targetPosition)
        {
            targetPosition.z = _cutSceneCamera.transform.position.z;
            _cutSceneCamera.transform.position = targetPosition;
        }

        public void SetState(bool isShown)
        {
            _animator.SetKeyVal(AnimationKeys.UI.CutScene.IsShow, isShown);
        }
    }
}