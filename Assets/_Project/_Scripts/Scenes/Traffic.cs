using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traffic : BaseScene
{
    [SerializeField] float carSpawnCooldown = .25f;
    [SerializeField] float carSpeed = 3f;
    [SerializeField] Color[] carColorPool;

    List<Car> _lCars = new();
    List<Car> _rCars = new();
    Dictionary<List<Car>, Transform[]> _destinations = new();

    Coroutine _carSpawnCoroutine;

    TrafficRefs _references;

    bool spawnLeft = true;

    public override void Init(InputController inputControllerRef)
    {
        _references = FindObjectOfType<TrafficRefs>();
        _destinations.Add(_lCars, _references.lDestinations);
        _destinations.Add(_rCars, _references.rDestinations);
        sceneCamera = _references.gameCamera;

        base.Init(inputControllerRef);
    }

    public override void StartGame()
    {
        base.StartGame();

        _carSpawnCoroutine = StartCoroutine(CarSpawnLoop());
    }

    IEnumerator CarSpawnLoop()
    {
        while(true)
        {
            Transform spawnPoint = spawnLeft ? _references.lCarSpawnPoint : _references.rCarSpawnPoint;

            Car instantiatedCar = Instantiate(_references.carPrefab, spawnPoint.position, Quaternion.identity, parent: transform).GetComponent<Car>();
            instantiatedCar.GetComponentInChildren<Renderer>().materials[0].color = carColorPool[Random.Range(0, carColorPool.Length)];
            instantiatedCar.speed = carSpeed;
            if (spawnLeft)
            {
                if (_lCars.Count + 1 > _references.lDestinations.Length)
                    break;
                _lCars.Add(instantiatedCar);
                instantiatedCar.UpdateDestination(_references.lDestinations[_lCars.Count - 1]);
            }
            else
            {
                if (_rCars.Count + 1 > _references.rDestinations.Length)
                        break;
                _rCars.Add(instantiatedCar);
                Transform destination = _references.lDestinations[_lCars.Count - 1];
                if (destination == null)
                    continue;
                instantiatedCar.UpdateDestination(_references.rDestinations[_rCars.Count - 1]);
            }

            spawnLeft = !spawnLeft;

            yield return new WaitForSeconds(carSpawnCooldown);
        }

        Lose();
    }

    protected override void TimeoutEnded()
    {
        base.TimeoutEnded();

        Win();
    }

    protected override void OnMouseLeftClick()
    {
        base.OnMouseLeftClick();

        if (Physics.Raycast(_references.gameCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 10f, layerMask: LayerMask.GetMask("Clickable")))
            if (hit.transform.TryGetComponent(out Car clickedCar))
            {
                RemoveCarFromRoad(clickedCar);
                SpawnBicycle(_lCars.Contains(clickedCar) ? _references.lBicycleSpawnPoint : _references.rBicycleSpawnPoint);
            }
    }

    public void RemoveCarFromRoad(Car carToRemove)
    {
        List<Car> targetLane = _lCars.Contains(carToRemove) ? _lCars : _rCars;

        targetLane.Remove(carToRemove);
        Destroy(carToRemove.gameObject);
        foreach (var car in targetLane)
            car.UpdateDestination(_destinations[targetLane][targetLane.IndexOf(car)]);
    }

    void SpawnBicycle(Transform spawnPoint)
    {
        Instantiate(_references.bicyclePrefab, spawnPoint.position, Quaternion.identity);
    }

    public override void Win()
    {
        base.Win();

        if (_carSpawnCoroutine != null)
            StopCoroutine(_carSpawnCoroutine);
    }

    protected override void Lose()
    {
        base.Lose();

        if (_carSpawnCoroutine != null)
            StopCoroutine(_carSpawnCoroutine);
    }
}
