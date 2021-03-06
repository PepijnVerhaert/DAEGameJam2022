using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnTransforms = new List<Transform>();
    //[SerializeField] private List<Texture2D> _icons = new List<Texture2D>();
    [SerializeField] private List<GameObject> _playerPrefabs = null;
    //private int _maxScoreFishing = 0;
    [SerializeField] private float _loadToEndScreenTime = 2f;

    private CurtainLiftUpBehavior _curtainBehavior;

    [SerializeField] private List<GameObject> _deadRenee = new List<GameObject>();

    void Start()
    {
        PlayerManager.Instance.SpawnPlayers(_spawnTransforms, _playerPrefabs);
        _curtainBehavior = FindObjectOfType<CurtainLiftUpBehavior>();
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
            StartCoroutine(LoadEndScene());
        }
    }

    IEnumerator LoadEndScene()
    {
        _curtainBehavior.OnGameOver();
        yield return new WaitForSeconds(_loadToEndScreenTime);
        SceneManager.LoadScene("EndMenu");
    }

}
