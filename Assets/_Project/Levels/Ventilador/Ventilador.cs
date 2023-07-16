using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.tvOS;

public class Ventilador : BaseScene
{
    public float speed = 10f;
    //public float Maxspeed = 10f;
    public bool start = false;

    float _currentSpeed;

    VentiladorRefs refs;


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
            _currentSpeed = -speed;
        }

        else if (keyPressed == KeyCode.D)
        {
            _currentSpeed = speed;
        }
    }

    void LateUpdate()
    {
        // move player
        refs.player.transform.RotateAround(refs.ventiladorGiro.transform.position, Vector3.forward, _currentSpeed * Time.deltaTime);
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