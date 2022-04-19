using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    private Rigidbody2D _rigidBody = null;
    private Collider2D _collider = null;
    private const string _platformLayer = "Platform";
    private const string _platformIgnore = "PlatformIgnore";
    [SerializeField] private float _jumpForce = 500f;
    [SerializeField] private float _rayDistanceDown = 0.1f;
    [SerializeField] private float _playerLength = 1.0f;

    private bool _isGrounded = false;

    void Start()
    {   
        _rigidBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        CheckPlatformCollision();
    }

    private void CheckPlatformCollision()
    {
        if (!_rigidBody) return;

        //if not negative y velocity return
        if (_rigidBody.velocity.y > 0.0f)
        {
            _isGrounded = false;
            gameObject.layer = 6;
            return;
        }

        RaycastHit2D hitInfoDown = Physics2D.Raycast(transform.position, -transform.up
            , _rayDistanceDown, LayerMask.GetMask(_platformLayer));

        if(hitInfoDown.collider)
        {
            //if raycast up doesnt collide w platf 
            Vector2 origin = new Vector2(transform.position.x, transform.position.y + _playerLength / 2.0f);
            RaycastHit2D hitInfoUp = Physics2D.Raycast(origin, transform.up
            , _playerLength / 2.0f, LayerMask.GetMask(_platformLayer));

            if (!hitInfoUp.collider)
            {
                _isGrounded = true;
                gameObject.layer = LayerMask.GetMask("Default");
            }
        }
        else
        {
            _isGrounded = false;
            gameObject.layer = 6;
        }
    }
}
