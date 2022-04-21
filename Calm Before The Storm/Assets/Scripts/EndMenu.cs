using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class EndMenu : MonoBehaviour
{
    [SerializeField] private string gameScene = "Jochen";
    [SerializeField] private float _replayDelay = 5f;
    [SerializeField] private LoomingCloudBehavior _cloudBehavior;

    public void RestartGame(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            StartCoroutine(Restart());
        }
    }

    public IEnumerator Restart()
    {
        _cloudBehavior.ChangeWeather(true);
        yield return new WaitForSeconds(_replayDelay);
        PlayerManager.Instance.Players.Clear();
        SceneManager.LoadScene(gameScene);
    }

    public void EndGame(InputAction.CallbackContext context)
    {
        if (context.performed) Application.Quit();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
