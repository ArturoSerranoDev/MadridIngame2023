using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentiladorGiro : MonoBehaviour
{
    public float minSpeed = 10f;
    public float maxSpeed = 30f;
    public float minTime = 2f;
    public float maxTime = 5f;

    private float rotationSpeed;
    private float rotationTime;
    private float rotationTimer;
    private int rotationDirection = 1;

    public bool start = false;

    private void Start()
    {
        // Generar velocidad y tiempo aleatorios
        rotationSpeed = Random.Range(minSpeed, maxSpeed);
        rotationTime = Random.Range(minTime, maxTime);
    }

    private void FixedUpdate()
    {
        if (!start)
            return;

        // Rotar el objeto
        transform.Rotate(Vector3.forward * rotationSpeed * rotationDirection * Time.deltaTime);

        // Actualizar el temporizador
        rotationTimer += Time.deltaTime;

        // Comprobar si se alcanzó el tiempo de rotación
        if (rotationTimer >= rotationTime)
        {
            // Cambiar la dirección de rotación
            rotationDirection *= -1;

            // Generar nuevos valores aleatorios para velocidad y tiempo
            rotationSpeed = Random.Range(minSpeed, maxSpeed);
            rotationTime = Random.Range(minTime, maxTime);

            // Reiniciar el temporizador
            rotationTimer = 0f;
        }
        float currentRotation = transform.rotation.eulerAngles.y;
        if (currentRotation > 40f && currentRotation < 180f)
        {
            currentRotation = 40f;
            transform.rotation = Quaternion.Euler(0f, currentRotation, 0f);
        }
        else if (currentRotation < 320f && currentRotation >= 180f)
        {
            currentRotation = 320f;
            transform.rotation = Quaternion.Euler(0f, currentRotation, 0f);
        }
    }
}
