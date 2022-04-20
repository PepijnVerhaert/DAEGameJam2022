using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBehavior : MonoBehaviour
{
    private MovementBehavior _movementBehavior = null;
    private float _jumpForce = 300.0f;

    // Start is called before the first frame update
    void Start()
    {
        _movementBehavior = GetComponent<MovementBehavior>();
        if (_movementBehavior)
        {
           _movementBehavior.Jump(_jumpForce);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_movementBehavior)
        {
            //_movementBehavior.MoveX(1.0f);
        }
        //delete if under screen
    }
}
