using MilkShake;
using UnityEngine;

public class CameraSheker : MonoBehaviour
{
    [SerializeField] private Shaker _shaker;
    [SerializeField] private ShakePreset _shakePreset;
    [SerializeField] private RoadBulder _roadBuilder;

    private void OnEnable()
    {
        _roadBuilder.OnRoadBuild += ShakeByBuild;
    }  
    private void OnDisable()
    {
        _roadBuilder.OnRoadBuild -= ShakeByBuild;
    }

    private void ShakeByBuild(int score)
    {
        
        _shaker.Shake(_shakePreset);
    }
}
