using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private string gameScene = "Jochen";

    public void AddPlayer(InputAction.CallbackContext context)
    {
        if(context.performed) PlayerManager.Instance.AddPlayer();
    }

    public void RemovePlayer(InputAction.CallbackContext context)
    {
        if (context.performed) PlayerManager.Instance.RemovePlayer();
    }

    public void StartGame(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(gameScene);
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
