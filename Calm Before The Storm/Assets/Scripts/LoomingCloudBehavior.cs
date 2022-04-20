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
    // Start is called before the first frame update
    void Start()
    {
        if (_isCalm)
        {
            transform.Translate(0, _leaveDistance, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _weatherChangeTimeAcc += Time.deltaTime;

        Vector3 pos;
        pos.x = transform.position.x;
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
