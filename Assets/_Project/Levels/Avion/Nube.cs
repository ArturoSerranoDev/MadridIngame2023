using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nube : MonoBehaviour
{
    public float cloudSpeed = 5;
    Avion avion;
    private void Start()
    {
        avion = FindObjectOfType<Avion>();
    }
    private void LateUpdate()
    {
        gameObject.transform.position += new Vector3(-cloudSpeed * Time.deltaTime, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        avion.nubesRecogidas++;
        Destroy(gameObject);
        
    }
}
