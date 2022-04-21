using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnTransforms = new List<Transform>();
    [SerializeField] private GameObject _playerPrefab = null;
    //private int _maxScoreFishing = 0;

    void Start()
    {
        PlayerManager.Instance.SpawnPlayers(_spawnTransforms, _playerPrefab);
    }

    void Update()
    {
        var players = PlayerManager.Instance.Players;
        int nrPlayersAlive = 0;
        foreach (var player in players)
        {
            if (player.tag != "SeagullPlayer" && !player.IsDead) nrPlayersAlive++;
        }

        if(nrPlayersAlive <= 0)
        {
            SceneManager.LoadScene("EndMenu");
            players.Clear();
        }
    }


}
