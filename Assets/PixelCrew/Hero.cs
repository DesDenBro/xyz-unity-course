using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private float _speed;
    private Vector2 _direction;
    
    public void SetDirection(Vector2 direction)
    {
        _direction = direction;
    }

    public void Say()
    {
        Debug.Log("hiiiii");
    }

    public void Update()
    {
        transform.position = new Vector3(
            _calcNextPos(transform.position.x, _direction.x),
            _calcNextPos(transform.position.y, _direction.y),
            0
        );
    }
    private float _calcNextPos(float startPos, float change)
    {
        if (change == 0) return startPos;
        return startPos + change * _speed * Time.deltaTime;
    }
}
