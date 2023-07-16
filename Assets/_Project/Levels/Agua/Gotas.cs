using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gotas : MonoBehaviour
{
    public Agua agua;

    private void OnTriggerEnter(Collider other)
    {
        agua.gotasBebidas++;
        Destroy(gameObject);
    }
}
