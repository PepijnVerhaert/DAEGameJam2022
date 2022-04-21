using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomInOutBehavior : MonoBehaviour
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
    Vector3 _originalScale;
    // Start is called before the first frame update
    void Start()
    {
        _originalScale = new Vector3(1, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        _weatherChangeTimeAcc += Time.deltaTime;

        float scale = transform.localScale.x;

        if (_isCalm)
        {
            if (_weatherChangeTimeAcc > _downDelay)
            {
                scale -= _downSpeed * Time.deltaTime;

                if (scale <= _bottomPosition)
                {
                    scale = _bottomPosition;
                }
            }
        }
        else
        {
            if (_weatherChangeTimeAcc > _upDelay)
            {
                scale += _upSpeed * Time.deltaTime;

                if (scale >= _topPosition)
                {
                    scale = _topPosition;
                }
            }
        }
        transform.localScale = new Vector3(scale, scale, 1f);
    }

    public void ChangeWeather(bool isCalm)
    {
        _isCalm = isCalm;
        _weatherChangeTimeAcc = 0f;
    }
}
