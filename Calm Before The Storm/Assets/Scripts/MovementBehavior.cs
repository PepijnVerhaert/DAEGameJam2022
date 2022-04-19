using UnityEngine;

public class MovementBehavior : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 2f;
   
    private Rigidbody2D _rigidBody;

    public void Jump(float force)
    {
        if(_rigidBody) _rigidBody.AddForce(new Vector2(0f, force));
    }

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 moveDirection)
    {
        _rigidBody.velocity = moveDirection * _movementSpeed * Time.deltaTime;
    }

    public void MoveX(float valueX)
    {
        _rigidBody.velocity = new Vector2(valueX * _movementSpeed * Time.deltaTime, _rigidBody.velocity.y);
    }
}