using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lookat : MonoBehaviour
{
    public string targetObjectName; // Nombre del objeto objetivo a buscar

    private Transform target;

    private void Start()
    {
        // Buscar el objeto objetivo por su nombre en la escena
        GameObject targetObject = GameObject.Find(targetObjectName);

        if (targetObject != null)
        {
            target = targetObject.transform;
        }
        else
        {
            Debug.LogError("No se encontró el objeto objetivo con el nombre: " + targetObjectName);
        }
    }

    private void Update()
    {
        // Rotar el objeto actual para mirar hacia el objeto objetivo en el eje Z
        if (target != null)
        {
            Vector3 directionToTarget = target.position - transform.position;
            float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
