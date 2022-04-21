using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatBehavior : MonoBehaviour
{
    [SerializeField]
    bool _isCalm = true;

    [SerializeField]
    private float _calmTiltSpeed = 10;
    [SerializeField]
    private float _calmTiltStrength = 1;

    [SerializeField]
    private float _stormTiltSpeed = 10;
    [SerializeField]
    private float _stormTiltStrength = 1;

    [SerializeField]
    private float _driftSpeed = 10;
    [SerializeField]
    private float _driftStrength = 1;

    [SerializeField] GameObject _score;
    private Vector3 _originalPosScore;

    //gradual tilt change
    [SerializeField]
    private float _tiltSpeedMaxChange = 10;
    [SerializeField]
    private float _tiltStrengthMaxChange = 10;
    private float _currentTiltSpeed = 0;
    private float _currentTiltStrength = 0;

    //gradual drift change
    [SerializeField]
    private float _driftSpeedMaxChange = 10;
    [SerializeField]
    private float _driftStrengthMaxChange = 10;
    private float _currentDriftSpeed = 0;
    private float _currentDriftStrength = 0;

    private Vector3 _originalPos;
    private float _driftAngle = 0;
    private float _tiltAngle = 0;

    public bool IsCalm
    {
        get { return _isCalm; }
        set { _isCalm = value; }
    }    

    // Start is called before the first frame update
    void Start()
    {
        _originalPos = transform.position;

        _currentTiltSpeed = _calmTiltSpeed;
        _currentTiltStrength = _calmTiltStrength;

        _originalPosScore = _score.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isCalm)
        {
            //update calm tilt
            _currentTiltSpeed -= _tiltSpeedMaxChange * Time.deltaTime;
            if(_currentTiltSpeed <= _calmTiltSpeed)
            {
                _currentTiltSpeed = _calmTiltSpeed;
            }
            _currentTiltStrength -= _tiltStrengthMaxChange * Time.deltaTime;
            if(_currentTiltStrength <= _calmTiltStrength)
            {
                _currentTiltStrength = _calmTiltStrength;
            }

            //update calm drift
            _currentDriftSpeed -= _driftSpeedMaxChange * Time.deltaTime;
            if (_currentDriftSpeed <= 0f)
            {
                _currentDriftSpeed = 0f;
            }
            _currentDriftStrength -= _driftStrengthMaxChange * Time.deltaTime;
            if (_currentDriftStrength <= 0f)
            {
                _currentDriftStrength = 0f;
            }
        }
        else
        {
            //update storm tilt
            _currentTiltSpeed += _tiltSpeedMaxChange * Time.deltaTime;
            if (_currentTiltSpeed >= _stormTiltSpeed)
            {
                _currentTiltSpeed = _stormTiltSpeed;
            }

            _currentTiltStrength += _tiltStrengthMaxChange * Time.deltaTime;
            if (_currentTiltStrength >= _stormTiltStrength)
            {
                _currentTiltStrength = _stormTiltStrength;
            }

            //update storm drift
            _currentDriftSpeed += _driftSpeedMaxChange * Time.deltaTime;
            if (_currentDriftSpeed >= _driftSpeed)
            {
                _currentDriftSpeed = _driftSpeed;
            }

            _currentDriftStrength += _driftStrengthMaxChange * Time.deltaTime;
            if (_currentDriftStrength >= _driftStrength)
            {
                _currentDriftStrength = _driftStrength;
            }
        }
        //calculate z tilting
        _tiltAngle += _currentTiltSpeed * Time.deltaTime;
        float angle = Mathf.Sin(_tiltAngle) * _currentTiltStrength;

        //calculate x drifting
        _driftAngle += _currentDriftSpeed * Time.deltaTime;
        Vector3 newPos = _originalPos;
        newPos.x = Mathf.Sin(_driftAngle) * _currentDriftStrength;

        //update boat transform
        transform.SetPositionAndRotation(newPos, Quaternion.Euler(0, 0, angle));

        // Update score
        _score.transform.SetPositionAndRotation(newPos, Quaternion.Euler(0, 0, angle));
    }
}
