
using PixelCrew.GameObjects.Creatures;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PixelCrew.GameObjects
{
    public class HeroInputReader : MonoBehaviour
    {
        [SerializeField] private Hero _hero;

        public void PressMovement(InputAction.CallbackContext context)
        {
            var direction = context.ReadValue<Vector2>();
            _hero.SetDirection(direction);
        }

        public void PressJump(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _hero.SetIsJumping(true);
            }
            if (context.canceled)
            {
                _hero.SetIsJumping(false);
            }
        }

        public void PressInteract(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _hero.OnInteract(true);
            }
            if (context.canceled)
            {
                _hero.OnInteract(false);
            }
        }

        public void PressAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.InitAttack();
            }
        }

        public void PressThrow(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _hero.InitThrow();
            }
        }

        /*
        private void Awake()
        {
            Debug.Log("Awake");
            _hero = GetComponent<Hero>();
        }

        private void OnEnable()
        {
            Debug.Log("OnEnable");
        }

        private void Start()
        {
            Debug.Log("Start");
        }

        private void FixedUpdate()
        {
            Debug.Log("FixedUpdate");
        } 

        private void Update()
        {
            //Debug.Log("Update");
            var horizontal = Input.GetAxis("Horizontal");
            _hero.SetDirection(horizontal);

            if (Input.GetButtonUp("Fire1"))
            {
                _hero.Say();
            }
        }

        private void LateUpdate()
        {
            Debug.Log("LateUpdate");
        }

        private void OnDisable()
        { 
            Debug.Log("OnDisable");
        }

        private void OnDestroy()
        {
            Debug.Log("OnDestroy");
        }
        */
    }
}