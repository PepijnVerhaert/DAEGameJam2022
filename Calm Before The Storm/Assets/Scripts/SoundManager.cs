using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> _calmAmbient = new List<AudioClip>();
    [SerializeField] private List<AudioClip> _stormAmbient = new List<AudioClip>();
    private List<GameObject> _currentSounds = new List<GameObject>();
    bool _isCalm = true;

    //effects
    private AudioSource _weatherChangeSound = null;
    [SerializeField] private AudioClip _stormStartEffect = null;
    [SerializeField] private AudioClip _calmStartEffect = null;

    public void ChangeWeather(bool isCalm)
    {
        if (_isCalm == isCalm) return;
        _isCalm = isCalm;

        //delete current ambient
        foreach (GameObject gameObject in _currentSounds)
        {
            Destroy(gameObject);
        }
        _currentSounds.Clear();

        SetAmbient();
    }
    private void SetAmbient()
    {
        if (_isCalm)
        {
            foreach (AudioClip audioClip in _calmAmbient)
            {
                GameObject newSound = new GameObject();
                AudioSource audioSource = newSound.AddComponent<AudioSource>();
                audioSource.clip = audioClip;
                audioSource.loop = true;
                audioSource.Play();
                _currentSounds.Add(newSound);
            }
            _weatherChangeSound.clip = _calmStartEffect;
            _weatherChangeSound.Play();
        }
        else
        {
            foreach (AudioClip audioClip in _stormAmbient)
            {
                GameObject newSound = new GameObject();
                AudioSource audioSource = newSound.AddComponent<AudioSource>();
                audioSource.clip = audioClip;
                audioSource.loop = true;
                audioSource.Play();
                _currentSounds.Add(newSound);
                _weatherChangeSound.clip = _stormStartEffect;
                _weatherChangeSound.Play();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _weatherChangeSound = GetComponent<AudioSource>();
        SetAmbient();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
