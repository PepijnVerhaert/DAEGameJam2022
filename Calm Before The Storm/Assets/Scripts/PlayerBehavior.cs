using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    private Rigidbody2D _rigidBody = null;
    private const string _platformLayer = "Platform";
    [SerializeField]
    private float _rayDistance = 0.1f;

    void Start()
    {   
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        CheckPlatformCollision();
    }

    private void CheckPlatformCollision()
    {
        if (!_rigidBody) return;

        //if not negative y velocity return
        if (_rigidBody.velocity.y >= 0.0f)
        {
            _rigidBody.constraints = RigidbodyConstraints2D.None;
            _rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            //_rigidBody.gravityScale = 1.0f;
            return;
        }
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, -transform.up
            , _rayDistance, LayerMask.GetMask(_platformLayer));

        if(hitInfo.collider)
        {
            //_rigidBody.velocity = new Vector2(_rigidBody.velocity.x, 0.0f);
            //_rigidBody.gravityScale = 0.0f;
            _rigidBody.constraints = RigidbodyConstraints2D.FreezePositionY;
        }
    }
}
