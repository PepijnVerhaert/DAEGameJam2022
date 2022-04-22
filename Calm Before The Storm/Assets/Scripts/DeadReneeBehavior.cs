using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadReneeBehavior : MonoBehaviour
{
    GameObject wave;

    [SerializeField] private float _movementSpeed = 5f;
    float heightDif;
    void Start()
    {
        wave = GameObject.Find("wave (1)");
        heightDif = transform.position.y - wave.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = _movementSpeed * Time.deltaTime;
        transform.position = new Vector2(transform.position.x + horizontal, wave.transform.position.y + heightDif -0.5f);
    }
}
