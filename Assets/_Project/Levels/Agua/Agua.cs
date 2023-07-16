using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agua : BaseScene
{
    AguaReffs aguaReffs;
    public float fuerzaImpulso = 10, timerImpulso = .5f, minImpulso=.2f, maxImpulso=1f;
    public bool start = false;
    public int gotasBebidas = 0, gotasVictoria;

    public override void Init(InputController inputControllerRef)
    {
        aguaReffs = FindAnyObjectByType<AguaReffs>();
        sceneCamera = aguaReffs.gameCamera;

        foreach (var gota in aguaReffs.gotasAgua)
        {
            gota.agua = this;
        }
        
        base.Init(inputControllerRef);
    }

    public override void StartGame()
    {
        
        base.StartGame();
        start = true;
        Destroy(aguaReffs.delete);
    }

    protected override void OnKeyboardInputPressed(KeyCode keyPressed)
    {
        base.OnKeyboardInputPressed(keyPressed);

        if (keyPressed == KeyCode.A)
        {
            if (start)
            {
                aguaReffs.player.AddForce(Vector3.up * fuerzaImpulso / 1.5f, ForceMode.Force);
                aguaReffs.player.AddForce(Vector3.right * fuerzaImpulso , ForceMode.Force);
            }
        }
    }
    private void FixedUpdate()
    {
        if (start)
        {
            if (aguaReffs.botella.position.y >= 3.5)
            {
                aguaReffs.player.velocity = Vector3.zero;
            }
            timerImpulso -= Time.deltaTime;

            if (timerImpulso <= 0)
            {
                aguaReffs.player.AddForce(Vector3.up * -fuerzaImpulso / 1.5f, ForceMode.Force);
                aguaReffs.player.AddForce(Vector3.right * -fuerzaImpulso, ForceMode.Force);
                timerImpulso = .3f;/* Random.Range(minImpulso,maxImpulso);*/

            }
        }
    }
    protected override void TimeoutEnded()
    {
        base.TimeoutEnded();
        if (gotasBebidas >= gotasVictoria)
            Win();
        else
            Lose();
    }
}
