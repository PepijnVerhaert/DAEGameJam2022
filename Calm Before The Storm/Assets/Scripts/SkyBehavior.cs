using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBehavior : MonoBehaviour
{
    private CloudBehavior[] _cloudBehaviors = new CloudBehavior[3];
    private LoomingCloudBehavior _loomingCloud = null;

    // Start is called before the first frame update
    void Start()
    {
        _cloudBehaviors = FindObjectsOfType<CloudBehavior>();
        _loomingCloud = FindObjectOfType<LoomingCloudBehavior>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeWeather(bool isCalm)
    {
        for (int i = 0; i < _cloudBehaviors.Length; i++)
        {
            _cloudBehaviors[i].ChangeWeather(isCalm);
        }
        _loomingCloud.ChangeWeather(isCalm);
    }
}