using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnTransforms = new List<Transform>();
    [SerializeField] private GameObject _playerPrefab = null;
    private int _maxScoreFishing = 0;

    void Start()
    {
        PlayerManager.Instance.SpawnPlayers(_spawnTransforms, _playerPrefab);
    }

    void Update()
    {
        
    }


}
