using UnityEngine;

public class MovementBehavior : MonoBehaviour
{
    [SerializeField] private Vector2 _desiredMovementDirection = Vector2.zero;
    [SerializeField] private float _movementSpeed = 2f;
   
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
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void LateUpdate()
    {
        _rigidBody.velocity = new Vector2(DesiredMovementDirection.x * _movementSpeed * Time.deltaTime, _rigidBody.velocity.y);
    }
}