using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private Animator _animator = null;

    private Rigidbody2D _rigidBody = null;
    private Collider2D _collider = null;
    private const string _platformLayer = "Platform";
    private const string _platformIgnore = "PlatformIgnore";

    [Header("Death")]
    private bool _isDead = false;
    [SerializeField] private float _yPositionToTransform = 3.3f;
    [SerializeField] private float _movementSpeedUp = 5f;

    [Header("Rays")]
    [SerializeField] private float _negativeYCollisionThreshHold = 0.1f;
    [SerializeField] private float _playerLength = 1.0f;
    [SerializeField] private float _maxSlopeAngle = 30f;
    [SerializeField] private float _rayDistanceDown = 1.0f;
    [SerializeField] private float _rayDistanceIsGrounded = 0.1f;

    // INPUT
    private InputAction _moveInput;
    private InputAction _jumpInput;

    // MOVEMENT
    [Header("Movement")]
    private bool _disableInput = false;
    [SerializeField] private Vector2 _maxVelocity = new Vector2(5f, 10f);
    [SerializeField] private float _movementSpeed = 10f;
    [SerializeField] private float _movementSpeedInAir = 3f;
    [SerializeField] private float _changeDirDelay = 2f;
    private float _changeDirCurrent = 0f;
    [SerializeField] private float _jumpForce = 500f;

    [Header("Storm")]
    [SerializeField] private float _calmDelay = 3f;
    private float _calmDelayCurrent = 0f;

    private MovementBehavior _movementBehavior;
    private bool _isGrounded = false;
    private bool _isCalmPreviousFrame = false;

    private float _stunTimer = 0.0f;

    private StormBehavior _stormBehavior;

    //fishing
    private bool _fishing = false;
    private int _score = 0;
    [SerializeField] private float _fishingCooldownTime = 2.0f;
    private float _fishingCooldownTimer = 0.0f;
    [SerializeField] private float _fishingWaitTime = 1.0f;
    private float _fishingWaitTimer = 0.0f;
    [SerializeField] private float _fishingSpeedCalm = 1.0f;
    [SerializeField] private float _fishingSpeedStorm = 0.5f;
    private float _fishingElapsed = 0.0f;

    public bool IsDead
    {
        get { return _isDead; }
    }

    public void Fish(InputAction.CallbackContext context)
    {
        if (context.performed && !_fishing)
        {
            if (_fishingCooldownTimer < 0.0f)
            {
                _fishingWaitTimer = _fishingWaitTime;
                _fishing = true;
            }
        }
        else if(_fishing)
        {
            _fishing = false;
            _fishingWaitTimer = 0.0f;
            _fishingCooldownTimer = _fishingCooldownTime;
        }
    }

    public int Score
    {
        get { return _score; }
    }

    public void Stun(float time)
    {
        _stunTimer = time;
        if(_fishing)
        {
            _fishing = false;
            _fishingCooldownTimer = _fishingCooldownTime;
            _fishingWaitTime = 0.0f;
        }
    }

    void Start()
    {
        _movementBehavior = GetComponent<MovementBehavior>();

        _rigidBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();

        PlayerInput characterInput = GetComponent<PlayerInput>();
        _moveInput = characterInput.actions["MovementRenee"];

        _jumpInput = characterInput.actions["Jump"];
        _jumpInput.performed += OnJump;

        _stormBehavior = FindObjectOfType<StormBehavior>();
    }

    void FixedUpdate()
    {
        UpdateIsGrounded();
        CheckPlatformCollision();
    }

    private bool DelayDirection()
    {
        _changeDirCurrent += Time.deltaTime;
        if (_changeDirCurrent < _changeDirDelay)
        {
            return true;
        }
        return false;
    }

    private void ResetDelay()
    {
        _changeDirCurrent = 0f;
    }

    private void Update()
    {
        _stunTimer -= Time.deltaTime;
        if (_stunTimer > 0.0f || _fishing || _isDead) _disableInput = true;
        else
        {
            _disableInput = false;
        }


        if (_stormBehavior.IsCalm)
        {
            _calmDelayCurrent += Time.deltaTime;
            if (_calmDelayCurrent >= _calmDelay && !_isCalmPreviousFrame)
            {
                _isCalmPreviousFrame = true;
            }
        }
        else
        {
            _isCalmPreviousFrame = false;
            _calmDelayCurrent = 0f;
        }

        UpdateMovement();
        UpdateFishing();
    }

    private void UpdateMovement()
    {
        if (!_movementBehavior) return;

        if (_disableInput) return;

        float moveX = _moveInput.ReadValue<float>();

        if (_stormBehavior.IsCalm && _isCalmPreviousFrame)
        {
            if (_isGrounded)
            {
                // reset velocity for snapping turns
                if (_rigidBody.velocity.x > 0f && moveX <= 0f)
                {
                    _rigidBody.velocity = new Vector2(0f, _rigidBody.velocity.y);
                }
                if (_rigidBody.velocity.x < 0f && moveX >= 0f)
                {
                    _rigidBody.velocity = new Vector2(0f, _rigidBody.velocity.y);
                }
            }
        }
        else
        {
            if (_rigidBody.velocity.x > 0f && moveX <= 0f)
            {
                if (DelayDirection())
                {
                    return;
                }
            }
            else if (_rigidBody.velocity.x < 0f && moveX >= 0f)
            {
                if (DelayDirection())
                {
                    return;
                }
            }
        }

        float speed;
        if (_isGrounded)
        {
            speed = _movementSpeed;
        }
        else
        {
            speed = _movementSpeedInAir;
        }

        _movementBehavior.MoveX(moveX, _maxVelocity, speed);
        ResetDelay();
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

        if (hitInfoIsGrounded.collider && transform.position.y + _playerLength / 10.0f > hitInfoIsGrounded.point.y)
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

            Vector3 dir = Quaternion.AngleAxis(_maxSlopeAngle - _maxSlopeAngle * i, transform.forward) * -transform.up;
            RaycastHit2D hitInfoColission = Physics2D.Raycast(transform.position, dir
            , _rayDistanceDown, LayerMask.GetMask(_platformLayer));

            Debug.DrawRay(transform.position, dir * _rayDistanceDown, Color.blue);

            if (hitInfoColission.collider && transform.position.y + _playerLength / 10.0f > hitInfoColission.point.y)
            {
                gameObject.layer = LayerMask.GetMask("Default");
                return;
            }
        }
        //gameObject.layer = 6;

    }

    public void OnDeath()
    {
        _isDead = true;
        StartCoroutine(MoveUp());
    }

    private void UpdateFishing()
    {
        _fishingCooldownTimer -= Time.deltaTime;
        _fishingWaitTimer -= Time.deltaTime;

        if (!_fishing || _fishingWaitTimer > 0.0f || _fishingCooldownTimer > 0.0f) return;

        _fishingElapsed += Time.deltaTime;

        float fishingSpeed = _fishingSpeedCalm;
        if(_stormBehavior && !_stormBehavior.IsCalm) fishingSpeed = _fishingSpeedStorm;
        
        if(_fishingElapsed > fishingSpeed)
        {
            _fishingElapsed = 0.0f;
            _score++;
            Debug.Log("FishScore: " + _score);
        }
    }

    IEnumerator MoveUp()
    {
        _rigidBody.velocity = Vector2.zero;
        _rigidBody.gravityScale = 0f;
        _collider.enabled = false;
        while (transform.position.y < _yPositionToTransform)
        {
            float vertical = _movementSpeedUp * Time.deltaTime;
            Vector2 newPos = new Vector2(transform.position.x, transform.position.y + vertical);
            transform.position = newPos;
            yield return null;
        }

        // transform to seagull
        enabled = false;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (!_isGrounded) return;
        _rigidBody.AddForce(new Vector2(0f, _jumpForce));
    }
}
