using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class EndMenu : MonoBehaviour
{
    [SerializeField] private string gameScene = "Jochen";

    public void RestartGame(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PlayerManager.Instance.Players.Clear();
            SceneManager.LoadScene(gameScene);
        }
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
