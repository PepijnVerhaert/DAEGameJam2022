using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoomingCloudBehavior : MonoBehaviour
{
    [SerializeField]
    bool _isCalm = true;

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

    [SerializeField]
    private bool _instaLeave = true;

    // Start is called before the first frame update
    void Start()
    {
        if (_isCalm && _instaLeave)
        {
            transform.Translate(0, _leaveDistance, 0);
        }

        _weatherChangeTimeAcc = 0f;

        _originalX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        _weatherChangeTimeAcc += Time.deltaTime;

        _swingAngle += _swingSpeed * Time.deltaTime;
        float swingX = Mathf.Sin(_swingAngle) * _swingStrength;

        Vector3 pos;
        pos.x = _originalX;
        pos.y = 0;
        pos.z = transform.position.z;

        if (_isCalm)
        {
            if (_weatherChangeTimeAcc < _leaveDelay)
            {
                return;
            }
            transform.Translate(0, _leaveSpeed * Time.deltaTime, 0);
            if (transform.position.y >= _leaveDistance)
            {
                pos.y = _leaveDistance;
                transform.SetPositionAndRotation(pos, transform.rotation);
            }
            pos.x += swingX;
            transform.SetPositionAndRotation(new Vector3(pos.x, transform.position.y, transform.position.z), transform.rotation);
        }
        else
        {
            if (_weatherChangeTimeAcc < _enterDelay)
            {
                return;
            }
            transform.Translate(0, -_enterSpeed * Time.deltaTime, 0);
            if (transform.position.y <= 0)
            {
                pos.y = 0f;
                transform.SetPositionAndRotation(pos, transform.rotation);
            }
            pos.x += swingX;
            transform.SetPositionAndRotation(new Vector3(pos.x, transform.position.y, transform.position.z), transform.rotation);
        }
    }

    public void ChangeWeather(bool isCalm)
    {
        _isCalm = isCalm;
        _weatherChangeTimeAcc = 0f;
    }
}
