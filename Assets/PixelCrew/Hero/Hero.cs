using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private int _money;

    [SerializeField] private LayerCheck _groundCheck;

    private Vector2 _direction;
    private Rigidbody2D _rigidbody;
    private bool _isJumping;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    public void SetIsJumping(bool val)
    {
        _isJumping = val;
    }

    public void AddMoney(GameObject obj)
    {
        var val = obj.GetComponent<ThingSpecification>()?.GetCost() ?? 0;
        if (val == 0) return;

        _money += val;
        Debug.Log("+" + val + ", всего денег: " + _money);
    }

    public void FixedUpdate()
    {
        _rigidbody.velocity = new Vector2(_direction.x * _speed, _rigidbody.velocity.y);

        if (_isJumping)
        {
            if (IsGrounded())
            {
                _rigidbody.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
            }
        } else if (_rigidbody.velocity.y > 0)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * 0.5f);
        }
    }

    private bool IsGrounded()
    {
        return _groundCheck.IsTouchingLayer;
    }

    private void OnDrawGizmos()
    {
        //Debug.DrawRay(transform.position, Vector2.down, IsGrounded() ? Color.green : Color.red);
        Gizmos.color = IsGrounded() ? Color.green : Color.red;
        Gizmos.DrawSphere(transform.position + new Vector3(0, -0.15f), 0.29f);
    }

    /*
    private float _calcNextPos(float startPos, float change)
    {
        if (change == 0) return startPos;
        return startPos + change * _speed * Time.deltaTime;
    }
    */

    public void Say()
    {
        //Debug.Log("hiiiii");
    }
}
