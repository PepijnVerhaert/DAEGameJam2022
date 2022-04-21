using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HUD : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> _scores = new List<TextMeshProUGUI>();
    [SerializeField] private List<SpriteRenderer> _greyWindows = new List<SpriteRenderer>();

    void Start()
    {
        
    }

    void Update()
    {
        UpdateScores();
        UpdateWindows();
    }

    private void UpdateWindows()
    {
        for(int i = 0; i < _greyWindows.Count; i++)
        {
            var players = PlayerManager.Instance.Players;
            if(i >= players.Count || players[i].IsDead)
            {
                if (i < _greyWindows.Count) _greyWindows[i].enabled = true;
            }
        }
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
