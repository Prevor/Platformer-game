using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public PlayerInput playerInput;

    private void Awake()
    {
        playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
        
    }


    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = playerInput.Player.Move.ReadValue<Vector2>();
        return inputVector;
    }
}
