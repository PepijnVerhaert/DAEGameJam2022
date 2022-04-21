using UnityEngine;
using UnityEngine.InputSystem;

public class SeagullPlayerBehavior : MonoBehaviour
{
    [SerializeField] private Animator _animator = null;
    
    private InputAction _moveInput;
    private MovementBehavior _movementBehavior;

    [SerializeField] private float _movementSpeed = 5f;
    [SerializeField] private float _maxVelocity = 5f;

    private Rigidbody2D _rigidBody;

    private Collider2D[] _playerColliders = new Collider2D[4];
    private float[] _playerHitCooldown = new float[4];
    [SerializeField] private float _maxPlayerHitCooldown = 3f;
    [SerializeField] private Vector2 _knockbackForce = new Vector2(500, 300);

    private StormBehavior _stormBehavior;

    private void Start()
    {
        PlayerInput characterInput = GetComponent<PlayerInput>();
        _moveInput = characterInput.actions["MovementSeagull"];

        _rigidBody = GetComponent<Rigidbody2D>();
        _movementBehavior = GetComponent<MovementBehavior>();
        _stormBehavior = FindObjectOfType<StormBehavior>();
    }

    private void OnEnable()
    {
        var players = PlayerManager.Instance.Players;
        for (int i = 0; i < players.Count; ++i)
        {
            _playerColliders[i] = players[i].GetComponent<Collider2D>();
            _playerHitCooldown[i] = _maxPlayerHitCooldown;
        }

        tag = "SeagullPlayer";

        if (_animator != null)
        {
            _animator.SetBool("IsSeagull", true);
        }
        if (_animator != null)
        {
            _animator.SetTrigger("Seagull");
        }
    }

    private void Update()
    {
        Vector2 move = _moveInput.ReadValue<Vector2>();
        _movementBehavior.Move(move, new Vector2(_maxVelocity, _maxVelocity), _movementSpeed);

        if (_rigidBody.velocity.x >= 0f)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }

        // reset velocity for snapping turns
        if (move.x.Equals(0f) && move.y.Equals(0f))
        {
            _rigidBody.velocity = Vector2.Lerp(_rigidBody.velocity, Vector2.zero, Time.deltaTime * 4f);
        }



        for (int i = 0; i < _playerHitCooldown.Length; i++)
        {
            _playerHitCooldown[i] += Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // Check if player has already been hit
            int playerIdx = -1;
            for (int i = 0; i < _playerHitCooldown.Length; i++)
            {
                if (_playerColliders[i] == collision)
                {
                    if (_playerHitCooldown[i] >= _maxPlayerHitCooldown)
                    {
                        playerIdx = i;
                        break;
                    }
                    else
                    {
                        return;
                    }
                }
            }

            PlayerBehavior behavior = collision.gameObject.GetComponent<PlayerBehavior>();
            if (_stormBehavior.IsCalm)
            {
                behavior.Stun(0.0f);
            }
            else
            {
                if (playerIdx != -1)
                {
                    int moveDir;
                    if (_rigidBody.velocity.x < 0f)
                    {
                        moveDir = -1;
                    }
                    else
                    {
                        moveDir = 1;
                    }

                    Vector2 knockback = new Vector2(_knockbackForce.x * moveDir, _knockbackForce.y);
                    collision.gameObject.GetComponent<Rigidbody2D>().AddForce(knockback);
                    _playerHitCooldown[playerIdx] = 0f;
                    StartCoroutine(behavior.ControllerVibrate(0.5f, 1f, 0.1f));
                }
            }
        }
    }
}