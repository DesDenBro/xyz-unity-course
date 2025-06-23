
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroInputReader : MonoBehaviour
{
    [SerializeField] private Hero _hero;

    public void OnMovement(InputAction.CallbackContext context)
    {
        var direction = context.ReadValue<Vector2>();
        _hero.SetDirection(direction);
    }

    public void OnJump(InputAction.CallbackContext context)
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

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _hero.Interact(true);
        }
        if (context.canceled)
        {
            _hero.Interact(false);
        }
    }

    public void OnSay(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            _hero.Say();
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
