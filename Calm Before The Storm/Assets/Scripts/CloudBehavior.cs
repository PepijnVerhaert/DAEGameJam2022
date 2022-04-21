using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudBehavior : MonoBehaviour
{
    [SerializeField]
    bool _isCalm = true;

    [SerializeField]
    private bool _isLeaveDirectionLeft;
    [SerializeField]
    private float _enterSpeed;
    [SerializeField]
    private float _leaveSpeed;
    [SerializeField]
    private float _enterDelay;
    [SerializeField]
    private float _leaveDelay;
    [SerializeField]
    private float _leaveDistance;

    private float _weatherChangeTimeAcc = 0;

    [SerializeField]
    private float _swingAngle = 0f;
    [SerializeField]
    private float _swingStrength = 0f;
    [SerializeField]
    private float _swingSpeed = 0f;

    private float _originalX;
    private float _moveX = 0;

    // Start is called before the first frame update
    void Start()
    {
        _originalX = transform.position.x;
        if (_isCalm)
        {
            if (_isLeaveDirectionLeft)
            {
                transform.Translate(-_leaveDistance, 0, 0);
                _moveX -= _leaveDistance;
            }
            else
            {
                transform.Translate(_leaveDistance, 0, 0);
                _moveX += _leaveDistance;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        _weatherChangeTimeAcc += Time.deltaTime;

        _swingAngle += _swingSpeed * Time.deltaTime;
        float swingX = Mathf.Sin(_swingAngle) * _swingStrength;

        Vector3 pos;
        pos.x = _originalX;
        pos.y = transform.position.y;
        pos.z = transform.position.z;

        if (_isCalm)
        {
            if(_weatherChangeTimeAcc < _leaveDelay)
            {
                return;
            }
            if (_isLeaveDirectionLeft)
            {
                _moveX -= _leaveSpeed * Time.deltaTime;
                if (_moveX <= -_leaveDistance)
                {
                    _moveX = -_leaveDistance;
                }
                pos.x += _moveX;
                pos.x += swingX;
                transform.SetPositionAndRotation(pos, transform.rotation);
            }
            else
            {
                _moveX += _leaveSpeed * Time.deltaTime;
                if (_moveX >= _leaveDistance)
                {
                    _moveX = _leaveDistance;
                }
                pos.x += _moveX;
                pos.x += swingX;
                transform.SetPositionAndRotation(pos, transform.rotation);
            }
        }
        else
        {
            if (_weatherChangeTimeAcc < _enterDelay)
            {
                return;
            }
            if (_isLeaveDirectionLeft)
            {
                _moveX += _enterSpeed * Time.deltaTime;
                if (_moveX >= 0f)
                {
                    _moveX = 0f;
                }
                pos.x += _moveX;
                pos.x += swingX;
                transform.SetPositionAndRotation(pos, transform.rotation);
            }
            else
            {
                _moveX -= _enterSpeed * Time.deltaTime;
                if (_moveX <= 0)
                {
                    _moveX = 0f;
                }
                pos.x += _moveX;
                pos.x += swingX;
                transform.SetPositionAndRotation(pos, transform.rotation);
            }
        }
    }

    public void ChangeWeather(bool isCalm)
    {
        _isCalm = isCalm;
        _weatherChangeTimeAcc = 0f;
    }
}
