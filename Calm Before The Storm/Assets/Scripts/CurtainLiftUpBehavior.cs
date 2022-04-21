using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurtainLiftUpBehavior : MonoBehaviour
{

    [SerializeField] private ZoomInOutBehavior _zoomInOut;
    [SerializeField] private LoomingCloudBehavior _loomingCloud;
    [SerializeField] private float _durationDisableInput = 2f;
    private void Start()
    {
        _zoomInOut.ChangeWeather(false);
        _loomingCloud.ChangeWeather(true);

        var players = PlayerManager.Instance.Players;
        for (int i = 0; i < players.Count; i++)
        {
            players[i].DisableInput = true;
        }
        StartCoroutine(EnableInput());

    }

    private IEnumerator EnableInput()
    {
        yield return new WaitForSeconds(_durationDisableInput);
        var players = PlayerManager.Instance.Players;
        for (int i = 0; i < players.Count; i++)
        {
            players[i].DisableInput = false;
        }
    }

    public void OnGameOver()
    {
        _zoomInOut.ChangeWeather(true);
        _loomingCloud.ChangeWeather(false);
    }

}
