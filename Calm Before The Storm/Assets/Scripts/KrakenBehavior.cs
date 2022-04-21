using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KrakenBehavior : MonoBehaviour
{
    [SerializeField] private float _toAttackTime = 2.0f;
    private float _toAttackElapsed = 0.0f;
    [SerializeField] private float _movementSpeedYAttack = 50.0f;
    [SerializeField] private float _movementSpeedYBasic = 5.0f;
    [SerializeField] private float _spawnMovementY = 1.0f;
    [SerializeField] private float _attackMovementY = 5.0f;
    private float _yMovement = 0.0f;
    private bool _hasAttacked = false;

    [SerializeField] private float _knockBackForce = 200.0f;
    [SerializeField] private float _stunDuration = 1.0f;

    void Update()
    {
        UpdateMovement();

        if (transform.position.y < -10f)
        {
            Destroy(gameObject);
        }
    }

    private void UpdateMovement()
    {
        float movement = 0.0f;
        if (!_hasAttacked)
        {
            if (_yMovement < _spawnMovementY)
            {
                movement = _movementSpeedYBasic * Time.deltaTime;
            }
            else if (_yMovement < _attackMovementY)
            {
                _toAttackElapsed += Time.deltaTime;
                if (_toAttackElapsed > _toAttackTime)
                {
                    movement = _movementSpeedYAttack * Time.deltaTime;
                }
            }
            else
            {
                _hasAttacked = true;
            }
        }
        else
        {
            movement = -_movementSpeedYBasic * Time.deltaTime;
        }
        transform.position = new Vector2(transform.position.x, transform.position.y + movement);
        _yMovement += movement;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerBehavior player = other.gameObject.GetComponent<PlayerBehavior>();
            if (player)
            {
                player.Stun(_stunDuration);
                Rigidbody2D playerRB = player.GetComponent<Rigidbody2D>();
                if (playerRB)
                {
                    bool playerLeft = player.transform.position.x < transform.position.x;
                    Vector2 dir = new Vector2(1.0f, 1.5f);
                    if (playerLeft) dir.x = -1.0f;
                    playerRB.AddForce(dir.normalized * _knockBackForce);
                }
            }
        }
    }
}
