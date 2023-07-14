using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles all input of the game
/// </summary>
public class InputController : MonoBehaviour
{
    public Action<KeyCode> KeyboardInputAction;
    
    public Action MouseLeftClickAction;
    public Action MouseRightClickAction;
    public Action<Vector2> MouseMoveAction;
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            KeyboardInputAction?.Invoke(KeyCode.A);
            Debug.Log("Left Key Pressed");
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            KeyboardInputAction?.Invoke(KeyCode.D);
            Debug.Log("Right Key Pressed");
        }
        
        // Mouse Input
        if (Input.GetMouseButtonDown(0))
        {
            MouseLeftClickAction?.Invoke();
            Debug.Log("Left Click Pressed");
        }
        else if (Input.GetMouseButtonDown(1))
        {
            MouseRightClickAction?.Invoke();
            Debug.Log("Right Click Pressed");
        }
        
        MouseMoveAction?.Invoke(Input.mousePosition);
    }
}
