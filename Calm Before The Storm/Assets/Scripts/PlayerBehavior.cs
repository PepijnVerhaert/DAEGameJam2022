using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    private Rigidbody2D _rigidBody = null;
    private Collider2D _collider = null;
    private const string _platformLayer = "Platform";
    private const string _platformIgnore = "PlatformIgnore";
    [SerializeField]
    private float _rayDistanceDown = 0.1f;
    [SerializeField]
    private float _rayDistanceUp = 1.0f;
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
            _isGrounded = true;
            gameObject.layer = LayerMask.GetMask("Default");
        }
        else
        {
            _isGrounded = false;
            gameObject.layer = 6;
        }
    }
}
