using UnityEngine;
using UnityEngine.Events;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private RoadBulder _roadBuilder;
    [SerializeField] private int _score;

    public UnityAction<int> OnScoreChanged;

    private void OnEnable()
    {
        _roadBuilder.OnRoadBuild += AddScorePoint;
    }

    private void OnDisable()
    {
        _roadBuilder.OnRoadBuild -= AddScorePoint;
    }

    private void AddScorePoint(int score)
    {
       if (_roadBuilder.IsDefaultCollider)
        {
            _score += score;
            OnScoreChanged?.Invoke(_score);
        }
        if (!_roadBuilder.IsDefaultCollider)
        {
            _score += score;
            OnScoreChanged?.Invoke(_score);
        }
    }
}
