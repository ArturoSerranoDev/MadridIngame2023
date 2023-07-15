using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBaseScene : BaseScene
{

    [SerializeField] private GameObject testCanvas;
    
    public override void Init(InputController inputControllerRef)
    {
        base.Init(inputControllerRef);
    }

    public override void StartGame()
    {
        base.StartGame();
        testCanvas.SetActive(true);
    }
    protected override void OnKeyboardInputPressed(KeyCode keyPressed)
    {
        base.OnKeyboardInputPressed(keyPressed);
    }

    public override void Win()
    {
        base.Win();
        testCanvas.SetActive(false);
    }
}
