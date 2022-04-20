using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnTransforms = new List<Transform>();
    [SerializeField] private GameObject _playerPrefab = null;


    void Start()
    {
        SpawnPlayers();
    }

    void Update()
    {
        
    }

    private void SpawnPlayers()
    {
        if (!_playerPrefab) return;

        for (int i = 0; i < PlayerManager.Instance.NrPlayers; i++)
        {
            if (_spawnTransforms.Count >= i)
            {
                Instantiate(_playerPrefab, _spawnTransforms[i].position, Quaternion.identity);
            }
        }
    }
}
