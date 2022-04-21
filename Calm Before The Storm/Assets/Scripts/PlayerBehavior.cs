using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private Animator _animator = null;
    
    private AudioSource _audioSource = null;
    [SerializeField] private AudioClip _waterSplash = null;
    [SerializeField] private AudioClip _hit = null;

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

    private Vector3 _startPosition;

    [Header("Storm")]
    [SerializeField] private float _calmDelay = 3f;
    private float _calmDelayCurrent = 0f;

    private MovementBehavior _movementBehavior;
    private bool _isGrounded = false;
    private bool _isCalmPreviousFrame = false;

    private float _stunTimer = 0.0f;

    private StormBehavior _stormBehavior;

    private Gamepad _gamePad;

    //fishing
    private bool _fishingInput = false;
    private bool _fishing = false;
    private int _score = 0;
    [SerializeField] private float _fishingCooldownTime = 2.0f;
    private float _fishingCooldownTimer = 0.0f;
    [SerializeField] private float _fishingWaitTime = 1.0f;
    private float _fishingWaitTimer = 0.0f;
    [SerializeField] private float _fishingSpeed = 1.0f;
    [SerializeField] private int _fishingPointsCalm = 1;
    [SerializeField] private int _fishingPointsStorm = 2;
    private float _fishingElapsed = 0.0f;

    public bool IsDead
    {
        get { return _isDead; }
    }

    public Gamepad Controller
    {
        set { _gamePad = value; }
    }

    private void Awake()
    {
        _startPosition = transform.position;
    }

    public void Fish(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _fishingInput = true;
        }
        else
        {
            _fishingInput = false;
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
            if (_animator != null)
            {
                _animator.SetBool("Fishing", false);
            }
            _fishing = false;
            _fishingCooldownTimer = _fishingCooldownTime;
            _fishingWaitTime = 0.0f;
        }
        if(_animator != null)
        {
            _animator.SetBool("KnockedBack", true);
        }

        if (_audioSource && _hit)
        {
            _audioSource.clip = _hit;
            _audioSource.Play();
        }
    }

    void Start()
    {
        _movementBehavior = GetComponent<MovementBehavior>();

        _rigidBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _audioSource = GetComponent<AudioSource>();

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
        if (_animator != null)
        {
            _animator.SetBool("Running", false);
        }

        transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);

        _stunTimer -= Time.deltaTime;
        if (_stunTimer <= 0f)
        {
            if (_animator != null)
            {
                _animator.SetBool("KnockedBack", false);
            }
        }
        else
        {
            if (_rigidBody.velocity.x > 0f)
            {
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            }
        }
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

        if(moveX.Equals(0f))
        {
            if (_animator != null)
            {
                _animator.SetBool("Running", false);
            }
        }
        else
        {
            if (_isGrounded)
            {
                if (_animator != null)
                {
                    _animator.SetBool("Running", true);
                }
            }
            if (moveX > 0f)
            {
                transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
            }
        }

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

        _movementBehavior.Move(moveX, _maxVelocity, speed);
        ResetDelay();
    }

    private void UpdateIsGrounded()
    {
        if (!_rigidBody) return;

        //if moving up return
        if (_rigidBody.velocity.y > _negativeYCollisionThreshHold)
        {
            _isGrounded = false;
            if (_animator != null)
            {
                _animator.SetBool("JumpingUp", true);
            }
            return;
        }

        RaycastHit2D hitInfoIsGrounded = Physics2D.Raycast(transform.position, -transform.up
            , _rayDistanceIsGrounded, LayerMask.GetMask(_platformLayer));

        if (hitInfoIsGrounded.collider && transform.position.y + _playerLength / 10.0f > hitInfoIsGrounded.point.y)
        {
            _isGrounded = true;
            if (_animator != null)
            {
                _animator.SetBool("JumpingUp", false);
                _animator.SetBool("JumpingDown", false);
            }
        }
        else
        {
            _isGrounded = false;
            if (_animator != null)
            {
                _animator.SetBool("JumpingDown", true);
            }
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

    public IEnumerator ControllerVibrate(float lewF, float highF, float time)
    {
        _gamePad.SetMotorSpeeds(lewF, highF);

        yield return new WaitForSeconds(time);

        _gamePad.SetMotorSpeeds(0f, 0f);
    }

    public void OnDeath()
    {
        _isDead = true;
        StartCoroutine(ControllerVibrate(1f, 2f, 0.5f));

        if (_audioSource && _waterSplash)
        {
            _audioSource.clip = _waterSplash;
            _audioSource.Play();
        }

        if (_animator != null)
        {
            _animator.SetBool("IsDead", true);
        }

        StartCoroutine(MoveUp());
    }

    private void UpdateFishing()
    {
        if(_fishingInput)
        {
            if (!_fishing && _fishingCooldownTimer < 0.0f)
            {
                _fishingWaitTimer = _fishingWaitTime;
                _fishing = true;
                if (_animator != null)
                {
                    _animator.SetBool("Fishing", true);
                }
            }
        }
        else 
        {
            if (_fishing && _fishingWaitTimer < 0.0f)
            {
                _fishing = false;
                if (_animator != null)
                {
                    _animator.SetBool("Fishing", false);
                }
                _fishingCooldownTimer = _fishingCooldownTime;
            }
        }

        _fishingCooldownTimer -= Time.deltaTime;
        _fishingWaitTimer -= Time.deltaTime;

        if (!_fishing || _fishingWaitTimer > 0.0f || _fishingCooldownTimer > 0.0f)
        {
            return;
        }

        if(_fishingWaitTimer < 0.0f)
        {
            if (_animator != null)
            {
                _animator.SetTrigger("GotPoint");
            }
        }

        _fishingElapsed += Time.deltaTime;

        transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);

        int fishingPoints = _fishingPointsCalm;
        if(_stormBehavior && !_stormBehavior.IsCalm) fishingPoints = _fishingPointsStorm;
        
        if(_fishingElapsed > _fishingSpeed)
        {
            _fishingElapsed = 0.0f;
            _score += fishingPoints;
            Debug.Log("FishScore: " + _score);
        }
    }

    IEnumerator MoveUp()
    {
        _rigidBody.velocity = Vector2.zero;
        _rigidBody.gravityScale = 0f;
        _collider.enabled = false;

        if (transform.position.x < -9.5f || transform.position.x > 9.5f)
        {
            transform.position = new Vector2(_startPosition.x, transform.position.y);
        }

        while (transform.position.y < _yPositionToTransform)
        {
            float vertical = _movementSpeedUp * Time.deltaTime;
            Vector2 newPos = new Vector2(transform.position.x, transform.position.y + vertical);
            transform.position = newPos;
            yield return null;
        }


        _collider.enabled = true;
        _collider.isTrigger = true;
        GetComponent<SeagullPlayerBehavior>().enabled = true;

        enabled = false;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (!_isGrounded) return;
        _rigidBody.AddForce(new Vector2(0f, _jumpForce));
    }
}
