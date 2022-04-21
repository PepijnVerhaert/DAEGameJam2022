using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeagullBehavior : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 20f;
    [SerializeField] bool _isMovingLeft = false;
    [SerializeField] private float _stunDuration = 0.5f;
    [SerializeField] private float _knockbackForce = 50f;
    private int _moveDirection;

    private Collider2D[] _colliders = new Collider2D[4];

    public bool IsMovingLeft
    {
        set 
        { 
            _isMovingLeft = value;

            if (_isMovingLeft)
                _moveDirection = -1;
            else
                _moveDirection = 1;
        }
    }

    private void Awake()
    {
        if (_isMovingLeft)
        {
            _moveDirection = -1;
        }
        else
        {
            _moveDirection = 1;
        }

    }

    private void Update()
    {
        if(_isMovingLeft)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }

        float horizontal = _moveDirection * _movementSpeed * Time.deltaTime;
        Vector2 newPos = new Vector2(transform.position.x + horizontal, transform.position.y);
        transform.position = newPos;

        if (transform.position.x < -20f || transform.position.x > 20f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // Check if player has already been hit
            foreach (var collider in _colliders)
            {
                if (collider == collision)
                {
                    return;
                }
            }

            // Player has not been hit so hit player
            for (int i = 0; i < _colliders.Length; i++)
            {
                if (_colliders[i] == null)
                {
                    collision.gameObject.GetComponent<PlayerBehavior>().Stun(_stunDuration);
                    Vector2 knockback = new Vector2(_knockbackForce * _moveDirection, _knockbackForce);
                    collision.gameObject.GetComponent<Rigidbody2D>().AddForce(knockback);
                    _colliders[i] = collision;
                    return;
                }
                else if(_colliders[i] == collision)
                {
                    continue;
                }
            }
        }
    }
}
