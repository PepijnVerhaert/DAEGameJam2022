using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeagullBehavior : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 20f;
    [SerializeField] bool _isMovingLeft = false;
    [SerializeField] private float _knockbackForce = 50f;
    private int _moveDirection;

    private Collider2D[] _colliders = new Collider2D[4];

    private void Awake()
    {
        if (_isMovingLeft)
        {
            _moveDirection = 1;
        }
        else
        {
            _moveDirection = -1;
        }
    }

    private void Update()
    {
        float horizontal = _moveDirection * _movementSpeed * Time.deltaTime;
        Vector2 newPos = new Vector2(transform.position.x + horizontal, transform.position.y);
        transform.position = newPos;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            foreach(var collider in _colliders)
            {
                if(collider == collision)
                {
                    continue;
                }

                //Vector2 knockback = new Vector2(_knockbackForce, _knockbackForce);
                //collider.GetComponent<Rigidbody2D>().AddForce(knockback * _moveDirection * Time.deltaTime);
            }

        }
    }
}
