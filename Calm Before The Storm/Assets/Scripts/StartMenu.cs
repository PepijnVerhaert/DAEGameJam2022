using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private string gameScene = "Jochen";
    [SerializeField] private GameObject players1;
    [SerializeField] private GameObject players2;
    [SerializeField] private GameObject players3;
    [SerializeField] private GameObject players4;

    [SerializeField] private GameObject controlls;
    [SerializeField] private AudioSource _clickEffect = null;

    private int _timesPressed = 0;
    [SerializeField] private float _delayBeforeSceneSwap = 2f;

    public void AddPlayer(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PlayerManager.Instance.AddPlayer();
            if (_clickEffect) _clickEffect.Play();
        }
    }

    public void RemovePlayer(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        PlayerManager.Instance.RemovePlayer();
        if (_clickEffect) _clickEffect.Play();
    }

    private IEnumerator SwitchScene()
    {
        yield return new WaitForSeconds(_delayBeforeSceneSwap);
        SceneManager.LoadScene(gameScene);
    }

    public void StartGame(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            switch (_timesPressed)
            {
                case 0:
                    ++_timesPressed;
                    players1.transform.parent.gameObject.GetComponent<LoomingCloudBehavior>().ChangeWeather(true);
                    if (_clickEffect) _clickEffect.Play();
                    break;
                case 1:
                    controlls.GetComponent<LoomingCloudBehavior>().ChangeWeather(true);
                    StartCoroutine(SwitchScene());
                    if (_clickEffect) _clickEffect.Play();
                    break;
            }
        }
    }

    void Update()
    {
        switch (PlayerManager.Instance.NrPlayers)
        {
            case 1:
                players1.gameObject.SetActive(true);
                players2.gameObject.SetActive(false);
                players3.gameObject.SetActive(false);
                players4.gameObject.SetActive(false);
                break;
            case 2:
                players1.gameObject.SetActive(false);
                players2.gameObject.SetActive(true);
                players3.gameObject.SetActive(false);
                players4.gameObject.SetActive(false);
                break;
            case 3:
                players1.gameObject.SetActive(false);
                players2.gameObject.SetActive(false);
                players3.gameObject.SetActive(true);
                players4.gameObject.SetActive(false);
                break;
            case 4:
                players1.gameObject.SetActive(false);
                players2.gameObject.SetActive(false);
                players3.gameObject.SetActive(false);
                players4.gameObject.SetActive(true);
                break;
        }
    }
}
