using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessRunnerScene : BaseScene
{
    // private GameObject playerCharacter;
    // private GameObject playerCharacter;
    private EndlessRunnerRefs _endlessRunnerRefs;

    private GameObject playerCharacter;
    private GameObject 
    public override void Init(InputController inputControllerRef)
    {
        base.Init(inputControllerRef);
        
        _endlessRunnerRefs = FindObjectOfType<EndlessRunnerRefs>();
        
        
    }

    // Method used whenever time ends. It may yield a positive or negative result
    // depending on Scene Logic
    protected override void TimeoutEnded()
    {

    }

    public override void Unload()
    {
        base.Unload();
    }

    protected override void OnMouseLeftClick()
    {
    }

    protected override void OnMouseRightClick()
    {
    }

    protected override void OnMouseMove(Vector2 obj)
    {
    }

    protected override void OnKeyboardInputPressed(KeyCode keyPressed)
    {
    }

    public override void Win()
    {
        SceneWon?.Invoke();
    }

    protected override void Lose()
    {
        SceneLost?.Invoke();
    }
}
