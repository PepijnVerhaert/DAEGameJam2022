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
    // Start is called before the first frame update
    void Start()
    {
        if (_isCalm)
        {
            if (_isLeaveDirectionLeft)
            {
                transform.Translate(-_leaveDistance, 0, 0);
            }
            else
            {
                transform.Translate(_leaveDistance, 0, 0);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        _weatherChangeTimeAcc += Time.deltaTime;

        Vector3 pos;
        pos.x = 0;
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
                transform.Translate(-_leaveSpeed * Time.deltaTime, 0, 0);
                if (transform.position.x <= -_leaveDistance)
                {
                    pos.x = -_leaveDistance;
                    transform.SetPositionAndRotation(pos, transform.rotation);
                }
            }
            else
            {
                transform.Translate(_leaveSpeed * Time.deltaTime, 0, 0);
                if(transform.position.x >= _leaveDistance)
                {
                    pos.x = _leaveDistance;
                    transform.SetPositionAndRotation(pos, transform.rotation);
                }
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
                transform.Translate(_enterSpeed * Time.deltaTime, 0, 0);
                if (transform.position.x >= 0f)
                {
                    transform.SetPositionAndRotation(pos, transform.rotation);
                }
            }
            else
            {
                transform.Translate(-_enterSpeed * Time.deltaTime, 0, 0);
                if (transform.position.x <= 0)
                {
                    transform.SetPositionAndRotation(pos, transform.rotation);
                }
            }
        }
    }

    public void ChangeWeather(bool isCalm)
    {
        _isCalm = isCalm;
        _weatherChangeTimeAcc = 0f;
    }
}
