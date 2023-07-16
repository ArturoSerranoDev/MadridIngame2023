using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.tvOS;

public class Ventilador : BaseScene
{
    VentiladorRefs refs;
    Vector3 _velocity;
    public float speed = 10f;
    //public float Maxspeed = 10f;
    public bool start = false;
    public override void Init(InputController inputControllerRef)
    {
        refs = FindAnyObjectByType<VentiladorRefs>();
        sceneCamera = refs.gameCamera;

        base.Init(inputControllerRef);
    }
    public override void StartGame()
    {
        base.StartGame();
        start = true;
        refs.ventiladorPlayer.start = true;
        refs.ventiladorGiro.start = true;
    }

    protected override void OnKeyboardInputPressed(KeyCode keyPressed)
    {
        base.OnKeyboardInputPressed(keyPressed);
        if (keyPressed == KeyCode.A)
        {
            refs.player.velocity = new Vector3(-speed, 0, 0);
        }

        else if (keyPressed == KeyCode.D)
        {
            refs.player.velocity = new Vector3(speed, 0, 0);
        }
    }
    protected override void TimeoutEnded()
    {
        base.TimeoutEnded();
        if (refs.ventiladorPlayer.hot > refs.ventiladorPlayer.maxHot / 2)
        {
            Lose();
        }
        else
        {
            Win();
        }
    }
}