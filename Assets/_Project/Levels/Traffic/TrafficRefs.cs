using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficRefs : MonoBehaviour
{
    public Camera gameCamera;

    [Header("Car Spawning")]
    public GameObject carPrefab;
    public GameObject bicyclePrefab;
    public Transform lCarSpawnPoint;
    public Transform rCarSpawnPoint;
    public Transform[] lDestinations;
    public Transform[] rDestinations;

    [Header("Bicycle Spawning")]
    public Transform lBicycleSpawnPoint;
    public Transform rBicycleSpawnPoint;
}
