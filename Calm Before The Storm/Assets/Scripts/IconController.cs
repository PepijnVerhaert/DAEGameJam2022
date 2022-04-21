using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _icon = null;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateIcon();
    }

    private void UpdateIcon()
    {
        if (!_icon) return;
        float offset = 0.5f;
        Debug.Log(offset);
        float screenHalfWidth = 9.0f;
        float screenHalfHeight = 5.0f;

        if (transform.position.x < -screenHalfWidth)
        {
            _icon.transform.position = new Vector3(-screenHalfWidth + offset, _icon.transform.position.y, _icon.transform.position.z);
            _icon.enabled = true;
        }
        else if (transform.position.x > screenHalfWidth)
        {
            _icon.transform.position = new Vector3(screenHalfWidth - offset, _icon.transform.position.y, _icon.transform.position.z);
            _icon.enabled = true;
        }
        else if (transform.position.y > screenHalfHeight)
        {
            _icon.transform.position = new Vector3(_icon.transform.position.x, screenHalfHeight - offset, _icon.transform.position.z);
            _icon.enabled = true;
        }
        else
        {
            _icon.enabled = false;
        }
    }
}
