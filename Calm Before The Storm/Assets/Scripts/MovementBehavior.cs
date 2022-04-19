using UnityEngine;
using UnityEngine.InputSystem;

public class MovementBehavior : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 2f;
    [SerializeField] private float _jumpForce = 500f;

    private InputAction _moveInput;
    private InputAction _jumpInput;
    private Rigidbody2D _rigidBody;

    private void Start()
    {
        PlayerInput characterInput = GetComponent<PlayerInput>();
        _moveInput = characterInput.actions["Movement"];
        _rigidBody = GetComponent<Rigidbody2D>();

        _jumpInput = characterInput.actions["Fish"];
        _jumpInput.performed += OnJump;
    }

    private void Update()
    {
        float move2D = _moveInput.ReadValue<float>();
        Debug.Log(move2D);
        float horizontal = move2D * _movementSpeed * Time.deltaTime;
        Vector2 newPos = new Vector2(transform.position.x + horizontal, transform.position.y);
        _rigidBody.velocity = new Vector2(move2D * _movementSpeed * Time.deltaTime, _rigidBody.velocity.y);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump");
        _rigidBody.AddForce(new Vector2(0f, _jumpForce));
    }
}