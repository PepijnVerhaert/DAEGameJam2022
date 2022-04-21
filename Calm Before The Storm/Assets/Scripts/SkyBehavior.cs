using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBehavior : MonoBehaviour
{
    private CloudBehavior[] _cloudBehaviors = new CloudBehavior[3];
    [SerializeField]
    private LoomingCloudBehavior _loomingCloud = null;
    [SerializeField]
    private BackgroundBehavior _background = null;

    // Start is called before the first frame update
    void Start()
    {
        _cloudBehaviors = FindObjectsOfType<CloudBehavior>();
        //_loomingCloud = FindObjectOfType<LoomingCloudBehavior>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeWeather(bool isCalm)
    {
        for (int i = 0; i < _cloudBehaviors.Length; i++)
        {
            if(_cloudBehaviors[i]) _cloudBehaviors[i].ChangeWeather(isCalm);
        }
        if(_loomingCloud)_loomingCloud.ChangeWeather(isCalm);
        if(_background) _background.ChangeWeather(isCalm);
    }
}