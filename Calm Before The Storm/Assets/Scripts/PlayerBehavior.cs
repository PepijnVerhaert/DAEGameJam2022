using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehavior : MonoBehaviour
{
    private Rigidbody2D _rigidBody = null;
    private Collider2D _collider = null;
    private const string _platformLayer = "Platform";
    private const string _platformIgnore = "PlatformIgnore";
    [SerializeField] private float _jumpForce = 500f;
    [SerializeField] private float _negativeYCollisionThreshHold = 0.1f;
    [SerializeField] private float _playerLength = 1.0f;
    [SerializeField] private float _maxSlopeAngle = 30f;

    [SerializeField] private float _rayDistanceDown = 1.0f;
    [SerializeField] private float _rayDistanceIsGrounded = 0.1f;
    private InputAction _moveInput;
    private InputAction _jumpInput;

    private MovementBehavior _movementBehavior;

    private bool _isGrounded = false;

    void Start()
    {
        _movementBehavior = GetComponent<MovementBehavior>();

        _rigidBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();

        PlayerInput characterInput = GetComponent<PlayerInput>();
        _moveInput = characterInput.actions["Movement"];

        _jumpInput = characterInput.actions["Jump"];
        _jumpInput.performed += OnJump;
    }

    void FixedUpdate()
    {
        UpdateIsGrounded();
        CheckPlatformCollision();
    }

    private void Update()
    {
        float moveX = _moveInput.ReadValue<float>();
        _movementBehavior.MoveX(moveX);
    }

    private void UpdateIsGrounded()
    {
        if (!_rigidBody) return;

        //if moving up return
        if (_rigidBody.velocity.y > _negativeYCollisionThreshHold)
        {
            _isGrounded = false;
            return;
        }

        RaycastHit2D hitInfoIsGrounded = Physics2D.Raycast(transform.position, -transform.up
            , _rayDistanceIsGrounded, LayerMask.GetMask(_platformLayer));

        if (hitInfoIsGrounded.collider && transform.position.y + _playerLength / 10.0f >  hitInfoIsGrounded.point.y )
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }

        Debug.DrawRay(transform.position, -transform.up * _rayDistanceIsGrounded, Color.yellow);
    }

    private void CheckPlatformCollision()
    {
        if (!_rigidBody) return;

        //if y velocity is bigger than epsilon, set no coll with platforms and return
        if (_rigidBody.velocity.y > _negativeYCollisionThreshHold)
        {
            gameObject.layer = 6;
            return;
        }
        
        for (int i = 0; i < 3; i++)
        {

            Vector3 dir = Quaternion.AngleAxis(_maxSlopeAngle  - _maxSlopeAngle * i, transform.forward) * -transform.up;
            RaycastHit2D hitInfoColission = Physics2D.Raycast(transform.position, dir
            , _rayDistanceDown, LayerMask.GetMask(_platformLayer));

            Debug.DrawRay(transform.position, dir * _rayDistanceDown, Color.blue);

            if(hitInfoColission.collider && transform.position.y + _playerLength / 10.0f > hitInfoColission.point.y)
            {
                gameObject.layer = LayerMask.GetMask("Default");
                return;
            }
        }
        //gameObject.layer = 6;

    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (!_isGrounded) return;
        _rigidBody.AddForce(new Vector2(0f, _jumpForce));
    }
}
