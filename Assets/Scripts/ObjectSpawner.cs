using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private Transform _roadObj;
    private Transform _carObj;
    private Transform _wayPointer;
    protected Transform SpawnRoad()
    {
        Transform newRoad = Instantiate(_roadObj, transform);
        return newRoad;
    }
    protected Transform SpawnCar()
    {
        Transform newCar = Instantiate(_carObj);
        return newCar;
    }
    protected Transform SpawnWayPointer()
    {
        Transform newWayPointer = Instantiate(_wayPointer, transform);
        return newWayPointer;
    }
}
