using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundBehavior : MonoBehaviour
{
    [SerializeField]
    private bool _isCalm = true;
    [SerializeField]
    private float _topPosition;
    [SerializeField]
    private float _bottomPosition;

    [SerializeField]
    private float _upSpeed;
    [SerializeField]
    private float _downSpeed;

    [SerializeField]
    private float _upDelay;
    [SerializeField]
    private float _downDelay;

    private float _weatherChangeTimeAcc = 0f;
    Vector3 _originalPosition;
    // Start is called before the first frame update
    void Start()
    {
        _originalPosition = new Vector3(0,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        _weatherChangeTimeAcc += Time.deltaTime;

        float posY = transform.position.y;

        if(_isCalm)
        {
            if (_weatherChangeTimeAcc > _downDelay)
            {
                posY -= _downSpeed * Time.deltaTime;

                if(posY <= _bottomPosition)
                {
                    posY = _bottomPosition;
                }
            }
        }
        else
        {
            if (_weatherChangeTimeAcc > _upDelay)
            {
                posY += _upSpeed * Time.deltaTime;

                if (posY >= _topPosition)
                {
                    posY = _topPosition;
                }
            }
        }
        transform.SetPositionAndRotation(new Vector3(_originalPosition.x, posY, _originalPosition.z), transform.rotation);
    }

    public void ChangeWeather(bool isCalm)
    {
        _isCalm = isCalm;
        _weatherChangeTimeAcc = 0f;
    }
}
