using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBehavior : MonoBehaviour
{
    private CloudBehavior[] _cloudBehaviors = new CloudBehavior[3];
    [SerializeField]
    private LoomingCloudBehavior _loomingCloud = null;
    [SerializeField]
    private LoomingCloudBehavior[] _lightnings = null;
    [SerializeField]
    private Animator[] _lightningAnimators = null;
    [SerializeField]
    private BackgroundBehavior _background = null;

    [SerializeField]
    private float _cycleOffsetMax = 1f;

    // Start is called before the first frame update
    void Start()
    {
        _cloudBehaviors = FindObjectsOfType<CloudBehavior>();
        //_loomingCloud = FindObjectOfType<LoomingCloudBehavior>();

        for (int i = 0; i < _lightningAnimators.Length; i++)
        {
            if (_lightningAnimators[i]) _lightningAnimators[i].SetFloat("CycleOffset", Random.Range(0f, _cycleOffsetMax));
        }
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
        for (int i = 0; i < _lightnings.Length; i++)
        {
            if (_lightnings[i]) _lightnings[i].ChangeWeather(isCalm);
            if(_lightningAnimators[i]) _lightningAnimators[i].SetTrigger("Strike");
        }

        if (_lightnings[0])
        {
        }
        if(_background) _background.ChangeWeather(isCalm);
    }
}