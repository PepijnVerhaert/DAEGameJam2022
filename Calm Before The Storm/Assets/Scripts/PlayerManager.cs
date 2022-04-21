using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region SINGLETON
    private static PlayerManager _instance;
    public static PlayerManager Instance
    {
        get
        {
            if (_instance == null && !_applicationQuiting)
            {
                //find it in case it was placed in the scene
                _instance = FindObjectOfType<PlayerManager>();
                if (_instance == null)
                {
                    //none was found in the scene, create a new instance
                    GameObject newObject = new GameObject("Singleton_PlayerManager");
                    _instance = newObject.AddComponent<PlayerManager>();
                }
            }
            return _instance;
        }
    }
    private static bool _applicationQuiting = false;
    public void OnApplicationQuit()
    {
        _applicationQuiting = true;
    }
    void Awake()
    {
        //we want this object to persist when a scene changes
        DontDestroyOnLoad(gameObject);
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private List<PlayerBehavior> _players = new List<PlayerBehavior>();
    private int _nrPlayers = 1;
    private int _maxPlayers = 4;
    private int _minPlayers = 1;
    
    public int NrPlayers
    {
        get { return _nrPlayers; }
    }

    public List<PlayerBehavior> Players
    {
        get { return _players; }
    }

    public void AddPlayer()
    {
        if (_nrPlayers < _maxPlayers)
        {
            _nrPlayers++;
            Debug.Log(_nrPlayers);
        }
    }

    public void RemovePlayer()
    {
        if(_nrPlayers > _minPlayers)
        {
            _nrPlayers--;
            Debug.Log(_nrPlayers);
        }
    }
    public void SpawnPlayers(List<Transform> spawnTransforms, GameObject playerPrefab)
    {
        if (!playerPrefab) return;
        
        for (int i = 0; i < PlayerManager.Instance.NrPlayers; i++)
        {
            if (spawnTransforms.Count >= i && _players.Count < _maxPlayers)
            {
                GameObject player = Instantiate(playerPrefab, spawnTransforms[i].position, Quaternion.identity);
                PlayerBehavior playerComp = player.GetComponent<PlayerBehavior>();
                _players.Add(playerComp);
            }
        }
    }
    void Start()
    {
    }

    void Update()
    {
        
    }
}