using UnityEngine;
using UnityEngine.Assertions;

public class AiSpawnerBehavior : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject _seagullPrefab;
    [SerializeField] private GameObject _fishPrefab;
    [SerializeField] private GameObject _krakenPrefab;

    [Header("Percentages")]
    [SerializeField] private float _seagullPercentage = 45f;
    [SerializeField] private float _fishPercentage = 45f;
    [SerializeField] private float _krakenPercentage = 10f;

    [Header("Cycle")]
    [SerializeField] private int _spawnPercentage = 30;
    [SerializeField] int _spawnIncreasePerStorm = 5;

    [Header("Spawn locations")]
    [SerializeField] float _krakenSpawnHeight = -5f;
    [SerializeField] float _fishSpawnHeight = -6f;

    private bool _isCalm = true;

    [SerializeField] private float _spawnTime = 0.5f;
    private float _currentSpawnTime = 0f;

    public bool IsCalm
    {
        set { _isCalm = value; }
    }

    private void Awake()
    {
        Assert.IsNotNull(_seagullPrefab);
        Assert.IsNotNull(_fishPrefab);
        Assert.IsNotNull(_krakenPrefab);

        float total = _krakenPercentage + _seagullPercentage + _fishPercentage;
        _krakenPercentage = (_krakenPercentage / total) * 100f;
        _fishPercentage = (_fishPercentage / total) * 100f;
        _seagullPercentage = (_seagullPercentage / total) * 100f;
    }

    private void Update()
    {
        if (_isCalm)
        {
            _currentSpawnTime = _spawnTime;
            return;
        }

        _currentSpawnTime += Time.deltaTime;
        if (_currentSpawnTime >= _spawnTime)
        {
            int value = Random.Range(0, 100);
            if (value <= _spawnPercentage)
            {
                SpawnEnemy();
            }
            _currentSpawnTime = 0f;
        }
    }

    public void IncreaseSpawnPercentage()
    {
        _spawnPercentage += _spawnIncreasePerStorm;
    }

    private void SpawnEnemy()
    {
        int value = Random.Range(0, 100);
        if (value <= _krakenPercentage)
        {
            int xValue = Random.Range(0, 14);
            Instantiate(_krakenPrefab, new Vector3(xValue - 7, _krakenSpawnHeight, 0f), Quaternion.identity);
        }
        else if (value <= _fishPercentage + _krakenPercentage)
        {
            SpawnFish();
        }
        else
        {
            SpawnSeagull();
        }
    }

    private void SpawnFish()
    {
        int left = Random.Range(0, 2);
        if (left == 0)
            left = -1;

        float x = left * Random.Range(0, 13f) - 6.5f;
        var fish = Instantiate(_fishPrefab, new Vector3(x, _fishSpawnHeight, 0f), Quaternion.identity);
        var fishBehavior = fish.GetComponent<FishBehavior>();
        fishBehavior.JumpForce = Random.Range(fishBehavior.JumpForceBounds.x, fishBehavior.JumpForceBounds.y);
        fishBehavior.SetDirection(-left);
    }

    private void SpawnSeagull()
    {
        int left = Random.Range(0, 2);
        if (left == 0)
            left = -1;
        float y = Random.Range(0, 5f) - 1.5f;
        float x = left * 9.5f;
        var seagull = Instantiate(_seagullPrefab, new Vector3(x, y, 0f), Quaternion.identity);

        var seagullBehavior = seagull.GetComponent<SeagullBehavior>();
        if (seagullBehavior)
        {
            switch (left)
            {
                case 0:
                    seagullBehavior.IsMovingLeft = false;
                    break;
                case 1:
                    seagullBehavior.IsMovingLeft = true;
                    break;
            }
        }
    }
}
