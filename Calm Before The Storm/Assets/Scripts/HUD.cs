using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> _scores = new List<TextMeshProUGUI>();

    void Start()
    {
        
    }

    void Update()
    {
        UpdateScores();
    }

    private void UpdateScores()
    {
        var players = PlayerManager.Instance.Players;
        for(int i = 0; i < players.Count; i++)
        {
            if(_scores.Count >= i)
            {
                _scores[i].text = players[i].Score.ToString();
            }
        }
    }
}
