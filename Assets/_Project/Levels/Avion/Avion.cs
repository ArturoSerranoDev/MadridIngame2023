using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avion : BaseScene
{
    AvionReffs _references;
    Vector3 _velocity;
    public float acceleration = 10f;
    public float Maxspeed = 10f;
    bool gravity = true;
    public GameObject nubesPrefab01;
    public GameObject nubesPrefab02;
    public GameObject nubesPrefab03;
    public float spawnRateMin = .5f, spawnRateMax = 1.5f, spawnTimer = .5f;
    int nube;
    public float rebote = 1f;
    public int nubesRecogidas = 0, nubesVictoria;

    public override void Init(InputController inputControllerRef)
    {


        _references = FindObjectOfType<AvionReffs>();

        _references.avion.velocity = _velocity;
        sceneCamera = _references.avionCamera;

        base.Init(inputControllerRef);
    }
    protected override void TimeoutEnded()
    {
        base.TimeoutEnded();
        if (nubesRecogidas > nubesVictoria)
            Win();
        else
            Lose();
    }
    protected override void OnKeyboardInputPressed(KeyCode keyPressed)
    {
        base.OnKeyboardInputPressed(keyPressed);

        if (keyPressed == KeyCode.A)
        {
            gravity = !gravity;
            print(gravity);
        }
    }
    private void LateUpdate()
    {
        
        if (!HasGameStarted)
            return;

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            nube = Random.Range(1, 4);
            if (nube == 1)
            {
                Instantiate(nubesPrefab01, new Vector3 (10, Random.Range(-2,3),40), Quaternion.identity);
            }
            if (nube == 2)
            {
                Instantiate(nubesPrefab02, new Vector3(10, Random.Range(-2, 3), 40), Quaternion.identity);
            }
            if (nube == 3)
            {
                Instantiate(nubesPrefab03, new Vector3(10, Random.Range(-2, 3), 40), Quaternion.identity);
            }

            spawnTimer = Random.Range(spawnRateMin, spawnRateMax);
        }

       

        if (_references.avionTransform.position.y >= 5)
        {
            _references.avion.velocity = new Vector3(0,-rebote,0);

        }
        else if (_references.avionTransform.position.y <= 0)
        {
            _references.avion.velocity = new Vector3(0, rebote, 0);

        }
        else
        {
            if (gravity)
            {
                if (_references.avion.velocity.y >= -Maxspeed)
                    _references.avion.velocity -= new Vector3(0, acceleration * Time.deltaTime, 0);
            }

            else
            {
                if (_references.avion.velocity.y <= Maxspeed)
                    _references.avion.velocity += new Vector3(0, acceleration * Time.deltaTime, 0);
                if (transform.position.y <= -1)
                {
                    _references.avion.velocity = Vector3.zero;
                }
            }
        }

    }
}

