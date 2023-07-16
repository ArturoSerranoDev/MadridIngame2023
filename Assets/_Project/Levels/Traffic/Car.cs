using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Car : MonoBehaviour
{
    public float speed;

    public void UpdateDestination(Transform newDestination)
    {
        transform.DOMove(newDestination.position, 1f / speed);
    }
}
