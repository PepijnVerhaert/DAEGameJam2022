using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatBehavior : MonoBehaviour
{
    [SerializeField]
    bool _isCalm = true;

    [SerializeField]
    float _calmRotationSpeed = 5f;
    [SerializeField]
    float _stormRotationSpeed = 5f;

    [SerializeField]
    float _calmMaxAngle = 5f;
    [SerializeField]
    float _stormMaxAngle = 5f;

    [SerializeField]
    float _calmMinAngle = -5f;
    [SerializeField]
    float _stormMinAngle = -5f;

    [SerializeField]
    float _driftSpeed = 10;
    [SerializeField]
    float _driftStrength = 1;


    Vector3 _originalPos;
    float _driftAngle = 0;
    bool _rotateLeft = true;
    // Start is called before the first frame update
    void Start()
    {
        _originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float angle = transform.rotation.eulerAngles.z;
        if(angle > 180f)
        {
            angle -= 360f;
        }
        if (_isCalm)
        {
            if (_rotateLeft)
            {
                transform.Rotate(0, 0, _calmRotationSpeed * Time.deltaTime);
                if(angle >= _calmMaxAngle)
                {
                    _rotateLeft = false;
                }
            }
            else
            {
                transform.Rotate(0, 0, -_calmRotationSpeed * Time.deltaTime);
                if (angle <= _calmMinAngle)
                {
                    _rotateLeft = true;
                }
            }
        }
        else
        {
            if (_rotateLeft)
            {
                transform.Rotate(0, 0, _stormRotationSpeed * Time.deltaTime);
                if (angle >= _stormMaxAngle)
                {
                    _rotateLeft = false;
                }
            }
            else
            {
                transform.Rotate(0, 0, -_stormRotationSpeed * Time.deltaTime);
                if (angle <= _stormMinAngle)
                {
                    _rotateLeft = true;
                }
            }

            _driftAngle += _driftSpeed * Time.deltaTime;
            Vector3 newPos = _originalPos;
            newPos.x = Mathf.Sin(_driftAngle) * _driftStrength;
            transform.SetPositionAndRotation(newPos, transform.rotation);
        }
    }
}
