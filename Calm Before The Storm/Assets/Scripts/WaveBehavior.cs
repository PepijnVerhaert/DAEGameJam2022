using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBehavior : MonoBehaviour
{
    [SerializeField]
    bool _isCalm = true;

    [SerializeField]
    private float _stormHeight;
    [SerializeField]
    private float _stormStrength;
    [SerializeField]
    private float _stormSpeed;

    [SerializeField]
    private float _calmHeight;
    [SerializeField]
    private float _calmStrength;
    [SerializeField]
    private float _calmSpeed;

    private float _currentHeight;
    private float _currentStrength;
    private float _currentSpeed;
    private float _currentAngle;

    [SerializeField]
    private float _maxChangeHeight;
    [SerializeField]
    private float _maxChangeStrength;
    [SerializeField]
    private float _maxChangeSpeed;

    private float _originalY;

    [SerializeField]
    private float _offsetSpeed;
    [SerializeField]
    private float _offsetWidth;
    private float _currentOffset = 0;

    // Start is called before the first frame update
    void Start()
    {
        _currentSpeed = _calmSpeed;
        _currentStrength = _calmStrength;
        _currentHeight = _calmHeight;

        _originalY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        //move waves to the side
        _currentOffset += _offsetSpeed * Time.deltaTime;
        if(_currentOffset >= _offsetWidth)
        {
            _currentOffset -= _offsetWidth;
        }
        else if(_currentOffset <= -_offsetWidth)
        {
            _currentOffset += _offsetWidth;
        }

        if (_isCalm)
        {
            //update calm circular movement
            _currentSpeed -= _maxChangeSpeed * Time.deltaTime;
            if (_currentSpeed <= _calmSpeed)
            {
                _currentSpeed = _calmSpeed;
            }
            _currentStrength -= _maxChangeStrength * Time.deltaTime;
            if (_currentStrength <= _calmStrength)
            {
                _currentStrength = _calmStrength;
            }
            //updating calm height change
            _currentHeight -= _maxChangeHeight * Time.deltaTime;
            if (_currentHeight <= _calmHeight)
            {
                _currentHeight = _calmHeight;
            }
        }
        else
        {
            //update storm circular movement
            _currentSpeed += _maxChangeSpeed * Time.deltaTime;
            if (_currentSpeed >= _stormSpeed)
            {
                _currentSpeed = _stormSpeed;
            }
            _currentStrength += _maxChangeStrength * Time.deltaTime;
            if (_currentStrength >= _stormStrength)
            {
                _currentStrength = _stormStrength;
            }
            //updating storm height change
            _currentHeight += _maxChangeHeight * Time.deltaTime;
            if( _currentHeight >= _stormHeight)
            {
                _currentHeight = _stormHeight;
            }
        }
        //calculate x drifting
        _currentAngle += _currentSpeed * Time.deltaTime;
        Vector3 newPos = Vector3.zero;
        newPos.x = Mathf.Sin(_currentAngle) * _currentStrength + _currentOffset;
        newPos.y = (Mathf.Cos(_currentAngle) * _currentStrength) + _currentHeight + _originalY;
        newPos.z = transform.position.z;

        //update boat transform
        transform.SetPositionAndRotation(newPos, transform.rotation);
    }
}
