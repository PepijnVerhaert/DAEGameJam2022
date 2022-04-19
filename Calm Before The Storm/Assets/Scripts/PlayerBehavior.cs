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
    [SerializeField] private float _rayDistanceDown = 0.1f;
    [SerializeField] private float _playerLength = 1.0f;

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

        _jumpInput = characterInput.actions["Fish"];
        _jumpInput.performed += OnJump;
    }

    void FixedUpdate()
    {
        CheckPlatformCollision();
    }

    private void Update()
    {
        float moveX = _moveInput.ReadValue<float>();
        _movementBehavior.DesiredMovementDirection = new Vector2(moveX, 0f);
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

    private void OnJump(InputAction.CallbackContext context)
    {
        _rigidBody.AddForce(new Vector2(0f, _jumpForce));
    }
}
