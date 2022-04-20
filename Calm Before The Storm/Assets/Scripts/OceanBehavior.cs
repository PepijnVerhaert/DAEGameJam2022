using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanBehavior : MonoBehaviour
{
    private WaveBehavior[] _waveBehaviors = new WaveBehavior[3];

    // Start is called before the first frame update
    void Start()
    {
        _waveBehaviors = FindObjectsOfType<WaveBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeWeather(bool isCalm)
    {
        for(int i = 0; i < _waveBehaviors.Length; i++)
        {
            _waveBehaviors[i]._isCalm = isCalm;
        }
    }
}