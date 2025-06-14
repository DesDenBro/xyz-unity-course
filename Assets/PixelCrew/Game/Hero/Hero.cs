using PixelCrew.Common;
using PixelCrew.Components;
using UnityEngine;

public class Hero : MonoBehaviour
{
    private static readonly int _anim_isGrounded = Animator.StringToHash("is-grounded");
    private static readonly int _anim_isRunning = Animator.StringToHash("is-running");
    private static readonly int _anim_verticalVelocity = Animator.StringToHash("vertical-velocity");
    private static readonly int _anim_triggerHit = Animator.StringToHash("trigger-hit");
    private static readonly int _anim_triggerHealing = Animator.StringToHash("trigger-healing");

    [SerializeField] private float _speed;
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private float _damageJumpSpeed;
    [SerializeField] private LayerCheck _groundCheck;
    [SerializeField] private float _interactionRadius;
    [SerializeField] private LayerMask _interactionLayer;

    private Animator _animator;
    private SpriteRenderer _spriteRender;
    private Rigidbody2D _rigidbody;
    private Vector2 _direction;
    private bool _isJumpingPressed;
    private bool _isGrounded;
    private bool _isAllowedDoubleJump;
    private Collider2D[] _interactionResult = new Collider2D[1];

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRender = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _isGrounded = _groundCheck.IsTouchingLayer;
    }

    public void FixedUpdate()
    {
        var xVel = _direction.x * _speed;
        var yVel = CalcYVelocity();
        _rigidbody.velocity = new Vector2(xVel, yVel);

        UpdateSpriteDirection();
        UpdateAnimatorParamsState();
    }

    private float CalcYVelocity()
    {
        var yVel = _rigidbody.velocity.y;

        if (_isGrounded) _isAllowedDoubleJump = true;
        if (_isJumpingPressed)
        {
            yVel = CalJumpVelocity(yVel);        
        }
        else if (_rigidbody.velocity.y > 0)
        {
            yVel *= 0.5f;
        }

        return yVel;
    }
    private float CalJumpVelocity(float yVel)
    {
        var isFalling = _rigidbody.velocity.y <= 0.001f;
        if (!isFalling) return yVel;

        if (_isGrounded)
        {
            yVel += _jumpSpeed;
        }
        else if (_isAllowedDoubleJump)
        {
            yVel = _jumpSpeed;
            _isAllowedDoubleJump = false;
        }

        return yVel;
    }
    private void UpdateSpriteDirection()
    {
        if (_direction.x != 0)
        {
            _spriteRender.flipX = _direction.x < 0;
        }
    }
    private void UpdateAnimatorParamsState()
    {
        _animator.SetFloat(_anim_verticalVelocity, _rigidbody.velocity.y);
        _animator.SetBool(_anim_isRunning, _direction.x != 0);
        _animator.SetBool(_anim_isGrounded, _isGrounded);
    }



    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }    
    public void SetIsJumping(bool val)
    {
        _isJumpingPressed = val;
    }
    public void TakeDamage()
    {
        _animator.SetTrigger(_anim_triggerHit);
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y + _damageJumpSpeed);
    }
    public void TakeHealth()
    {
        _animator.SetTrigger(_anim_triggerHealing);
    }

    public void Interact()
    {
        var size = Physics2D.OverlapCircleNonAlloc(transform.position, _interactionRadius, _interactionResult, _interactionLayer);
        
        for (int i = 0; i < size; i++)
        {
            var interactable = _interactionResult[i].GetComponent<InteractableComponent>();
            if (interactable == null) continue;
            interactable.Interact();
        }    
    }


    private void OnDrawGizmos()
    {
        //Debug.DrawRay(transform.position, Vector2.down, IsGrounded() ? Color.green : Color.red);
        Gizmos.color = _isGrounded ? Color.green : Color.red;
        Gizmos.DrawSphere(transform.position + new Vector3(0, -0.15f), 0.29f);
    }

    public void Say()
    {

    }
}
