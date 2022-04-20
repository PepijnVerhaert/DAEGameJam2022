using UnityEngine;
using UnityEngine.InputSystem;

public class SeagullPlayerBehavior : MonoBehaviour
{
    private InputAction _moveInput;

    private void Start()
    {
        PlayerInput characterInput = GetComponent<PlayerInput>();
        _moveInput = characterInput.actions["MovementRenee"];

    }
}
