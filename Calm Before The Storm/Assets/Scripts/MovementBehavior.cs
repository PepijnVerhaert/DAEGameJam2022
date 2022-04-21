using UnityEngine;

public class MovementBehavior : MonoBehaviour
{
    private Rigidbody2D _rigidBody;

    public void Jump(float force)
    {
        if(_rigidBody) _rigidBody.AddForce(new Vector2(0f, force));
    }

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 moveDirection, Vector2 maxVel, float movementSpeed)
    {
        _rigidBody.AddForce(moveDirection * movementSpeed);
        float xVelocity = Mathf.Clamp(_rigidBody.velocity.x, -maxVel.x, maxVel.x);
        float yVelocity = Mathf.Clamp(_rigidBody.velocity.y, -maxVel.y, maxVel.y);
        _rigidBody.velocity = new Vector2(xVelocity, yVelocity);
    }

    public void Move(float moveXDirection, Vector2 maxVel, float movementSpeed)
    {
        _rigidBody.AddForce(new Vector2(moveXDirection * movementSpeed, 0f));
        float xVelocity = Mathf.Clamp(_rigidBody.velocity.x, -maxVel.x, maxVel.x);
        float yVelocity = Mathf.Clamp(_rigidBody.velocity.y, -maxVel.y, maxVel.y);
        _rigidBody.velocity = new Vector2(xVelocity, yVelocity);
    }
}