using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBehavior : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sprite = null;
    private Rigidbody2D _rigidBody2D = null;
    [SerializeField] private float _jumpForce = 300.0f;
    [SerializeField] private float _knockBackForce = 200.0f;
    [SerializeField] private float _stunDuration = 0.5f;
    [SerializeField] private float _xMoveForce = 10.0f;
    [SerializeField] private int _direction = 1;
    [SerializeField] private Vector2 _JumpForceBounds = new Vector2(400, 700);

    public float JumpForce
    {
        get { return _jumpForce; }
        set { _jumpForce = value; }
    }

    public Vector2 JumpForceBounds
    {
        get { return _JumpForceBounds; }
    }

    public void SetDirection(int direction)
    {
        _direction = direction;
        _xMoveForce = Mathf.Abs(_xMoveForce) * _direction;
        if (_sprite) _sprite.transform.localScale = new Vector3(_direction * 0.2f, 0.2f, 0.2f);
    }

    void Start()
    {
        SetDirection(_direction);

        _rigidBody2D = GetComponent<Rigidbody2D>();
        if (_rigidBody2D) _rigidBody2D.AddForce(transform.up * _jumpForce);
    }

    void Update()
    {
        if (!_rigidBody2D) return;

        _rigidBody2D.AddForce(transform.right * _xMoveForce * Time.deltaTime);

        UpdateRotation();

        //delete if under screen
        if(transform.position.y < -Screen.height)
        {
            Destroy(gameObject);
        }
    }

    private void UpdateRotation()
    {
        Vector2 velocity = _rigidBody2D.velocity.normalized;
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

        Debug.Log(angle);
        if (_sprite) _sprite.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerBehavior player = other.gameObject.GetComponent<PlayerBehavior>();
        if (player)
        {
            player.Stun(_stunDuration);
            Rigidbody2D playerRB = player.GetComponent<Rigidbody2D>();
            if (playerRB)
            {
                Vector2 dir = new Vector2(_direction, 1.5f);
                playerRB.AddForce(dir.normalized * _knockBackForce);
            }
        }
    }
}
