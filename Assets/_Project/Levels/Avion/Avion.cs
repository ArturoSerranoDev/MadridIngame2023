using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avion : BaseScene
{
    public float accelerationTime = .2f;
    public float Maxspeed = 10f;
    public float cloudSpeed = 5f;
    public GameObject nubesPrefab01;
    public GameObject nubesPrefab02;
    public GameObject nubesPrefab03;
    public float spawnCooldown = .5f, spawnTimer = .5f;
    public float rebote = 1f;
    public int nubesVictoria = 3;

    [HideInInspector] public int nubesRecogidas = 0;
    bool _gravity = true;
    Vector3 _velocity;
    int _nube;

    AvionReffs _references;

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
    protected override void OnMouseLeftClick()
    {
        base.OnMouseLeftClick();

        _gravity = !_gravity;
        print(_gravity);
    }
    private void LateUpdate()
    {
        
        if (!HasGameStarted)
            return;

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            _nube = Random.Range(1, 4);
            Nube instantiatedCloud = null;
            if (_nube == 1)
            {
                instantiatedCloud = Instantiate(nubesPrefab01, new Vector3 (10, Random.Range(-2,3),40), Quaternion.identity).GetComponent<Nube>();
            }
            if (_nube == 2)
            {
                instantiatedCloud = Instantiate(nubesPrefab02, new Vector3(10, Random.Range(-2, 3), 40), Quaternion.identity).GetComponent<Nube>();
            }
            if (_nube == 3)
            {
                instantiatedCloud = Instantiate(nubesPrefab03, new Vector3(10, Random.Range(-2, 3), 40), Quaternion.identity).GetComponent<Nube>();
            }

            if(instantiatedCloud != null)
                instantiatedCloud.cloudSpeed = cloudSpeed;

            spawnTimer = spawnCooldown;
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
            if (_gravity)
            {
                if (_references.avion.velocity.y >= -Maxspeed)
                {
                    //_references.avion.velocity -= new Vector3(0, acceleration * Time.deltaTime, 0);
                    _references.avion.velocity = Vector3.MoveTowards(_references.avion.velocity, Vector3.down * -Maxspeed, Time.deltaTime / accelerationTime);
                }
            }

            else
            {
                if (_references.avion.velocity.y <= Maxspeed)
                {
                    //_references.avion.velocity += new Vector3(0, acceleration * Time.deltaTime, 0);
                    _references.avion.velocity = Vector3.MoveTowards(_references.avion.velocity, Vector3.down * Maxspeed, Time.deltaTime / accelerationTime);
                }
                if (transform.position.y <= -1)
                {
                    _references.avion.velocity = Vector3.zero;
                }
            }
        }

    }
}

