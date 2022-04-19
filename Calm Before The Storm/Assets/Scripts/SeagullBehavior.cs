using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeagullBehavior : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 20f;
    [SerializeField] bool _isMovingLeft = false;


    private void Update()
    {
        int moveDirection = 1;
        if (!_isMovingLeft)
        {
            moveDirection *= -1;
        }
        float horizontal = moveDirection * _movementSpeed * Time.deltaTime;
        Vector2 newPos = new Vector2(transform.position.x + horizontal, transform.position.y);
        transform.position = newPos;

    }
}
