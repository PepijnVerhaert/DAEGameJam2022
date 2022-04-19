using UnityEngine;
using UnityEngine.InputSystem;

public class MovementBehavior : MonoBehaviour
{
    [SerializeField] private Vector2 _desiredMovementDirection = Vector2.zero;
    [SerializeField] private float _movementSpeed = 2f;
   

    private InputAction _moveInput;
    private InputAction _jumpInput;
    private Rigidbody2D _rigidBody;

    public Vector2 DesiredMovementDirection
    {
        get { return _desiredMovementDirection; }
        set { _desiredMovementDirection = value; }
    }

    public void Jump(float force)
    {
        if(_rigidBody) _rigidBody.AddForce(new Vector2(0f, force));
    }

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
        float moveX = _moveInput.ReadValue<float>();
        Debug.Log(moveX);
        _rigidBody.velocity = new Vector2(moveX * _movementSpeed * Time.deltaTime, _rigidBody.velocity.y);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump");
        _rigidBody.AddForce(new Vector2(0f, _jumpForce));
    }
}