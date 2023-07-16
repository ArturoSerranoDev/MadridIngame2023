using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantRotator : MonoBehaviour
{

    public float rotationSpeed = 10f; // Velocidad de rotación

    private void FixedUpdate()
    {
        // Rotar el objeto constantemente sobre el eje Z
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

    }
}
