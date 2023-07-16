using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gotas : MonoBehaviour
{
    public Agua agua;

    private void OnTriggerEnter(Collider other)
    {
        AudioManager.Instance.PlaySound(AudioManager.Instance.waterClip);
        agua.gotasBebidas++;
        Destroy(gameObject);
    }
}
