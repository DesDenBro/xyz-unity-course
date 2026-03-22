using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.GameObjects.Creatures
{
    public class HeroMovementLock : MonoBehaviour
    {
        private PlayerInput _input;

        private void Awake()
        {
            _input = GetComponent<PlayerInput>();
        }

        public void SetLock(bool isLock)
        {
            _input.enabled = !isLock;
        }
    }
}