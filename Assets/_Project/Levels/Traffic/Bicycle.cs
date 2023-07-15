using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bicycle : MonoBehaviour
{
    private void Start()
    {
        transform.DOMoveY(10f, 3f);
    }
}
