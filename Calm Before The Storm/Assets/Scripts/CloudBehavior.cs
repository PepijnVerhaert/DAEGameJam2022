using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudBehavior : MonoBehaviour
{
    [SerializeField]
    bool _isCalm = true;

    private bool _isLeaveDirectionLeft;
    private float _enterSpeed;
    private float _leaveSpeed;
    private float _enterDelay;
    private float _leaveDelay;
    private float _leaveDistance;

    private float _weatherChangeTimeAcc = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _weatherChangeTimeAcc += Time.deltaTime;

        if(_isCalm)
        {
            if (_isLeaveDirectionLeft)
            {
                transform.Translate(-_leaveSpeed * Time.deltaTime, 0, 0);
                if (transform.position.x <= _leaveDistance)
                {

                }
            }
            else
            {
                if(transform.position.x >= _leaveDistance)
                {

                }
            }
        }
    }
}
