using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class EndMenu : MonoBehaviour
{
    [SerializeField] private string gameScene = "Jochen";
    [SerializeField] private float _replayDelay = 5f;
    [SerializeField] private LoomingCloudBehavior _scoreBehavior;
    [SerializeField] private LoomingCloudBehavior _numbersBehavior;

    [SerializeField] private List<TextMeshProUGUI> _scores = new List<TextMeshProUGUI>();
    [SerializeField] private List<SpriteRenderer> _greyWindows = new List<SpriteRenderer>();

    private void Start()
    {
        var players = PlayerManager.Instance.Players;
        for (int i = 0; i < players.Count; i++)
        {
            if (_scores.Count >= i)
            {
                _scores[i].text = players[i].Score.ToString();
            }
        }

        for (int i = 0; i < _greyWindows.Count; i++)
        {
            if (i >= players.Count)
            {
                if (i < _greyWindows.Count) _greyWindows[i].enabled = true;
            }
        }
    }

    public void RestartGame(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StartCoroutine(Restart());
        }
    }

    public IEnumerator Restart()
    {
        _scoreBehavior.ChangeWeather(true);
        _numbersBehavior.ChangeWeather(true);
        yield return new WaitForSeconds(_replayDelay);
        PlayerManager.Instance.Players.Clear();
        SceneManager.LoadScene(gameScene);
    }

    public void EndGame(InputAction.CallbackContext context)
    {
        if (context.performed) Application.Quit();
    }
}
