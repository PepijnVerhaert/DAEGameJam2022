using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormBehavior : MonoBehaviour
{
    private BoatBehavior _boat = null;
    private OceanBehavior _ocean = null;

    private bool _isCalm = true;
    private float _timerToChange = 0.0f;
    private float _minCalmDuration = 1.0f;
    [SerializeField] private float _stormDuration = 0.0f;
    [SerializeField] private float _stormDurationStart = 15.0f;
    [SerializeField] private float _calmDuration = 0.0f;
    [SerializeField] private float _calmDurationStart = 30.0f;
    [SerializeField] private float _durationChange = 5.0f;

    public bool IsCalm
    {
        get { return _isCalm; }
    }

    void Start()
    {
        _boat = FindObjectOfType<BoatBehavior>();
        _ocean = FindObjectOfType<OceanBehavior>();
        
        _stormDuration = _stormDurationStart;
        _calmDuration = _calmDurationStart;
        _timerToChange = _calmDuration;
    }

    void Update()
    {
        UpdateStormChange();
    }
    private void UpdateStormChange()
    {
        if (!_isCalm && _calmDuration < _minCalmDuration) return;

        _timerToChange -= Time.deltaTime;
        if (_timerToChange < 0.0f)
        {
            _isCalm = !_isCalm;

            OnChange();

            if (_isCalm)
            {
                _timerToChange = _calmDuration;
                _calmDuration = Mathf.Clamp(_calmDuration - _durationChange, 0.0f, float.MaxValue);
            }
            else
            {
                _timerToChange = _stormDuration;
                _stormDuration += _durationChange;
            }
        }
    }

    private void OnChange()
    {
        if (_boat) _boat.IsCalm = _isCalm;
        if (_ocean) _ocean.ChangeWeather(_isCalm);
    }
}
