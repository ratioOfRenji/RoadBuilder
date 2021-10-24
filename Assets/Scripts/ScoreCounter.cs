
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private RoadBulder _roadBuilder;
    [SerializeField] private int _score;
    [SerializeField] private Text _scoreTextObj;

    private int _scoreText;
    public UnityAction<int> OnScoreChanged;

    private void Start()
    {
        _scoreText = 0;
        UpdateScore();
    }
    private void OnEnable()
    {
        _roadBuilder.OnRoadBuild += AddScorePoint;
    }

    private void OnDisable()
    {
        _roadBuilder.OnRoadBuild -= AddScorePoint;
    }
    private void UpdateScore()
    {
        _scoreTextObj.text = _scoreText.ToString();
    }
    private void AddScorePoint(int score)
    {
       if (_roadBuilder.IsDefaultCollider)
        {
            _score += score;
            OnScoreChanged?.Invoke(_score);
            _scoreText += score;
            UpdateScore();
        }
        if (!_roadBuilder.IsDefaultCollider)
        {
            _score += score;
            OnScoreChanged?.Invoke(_score);
        }
    }
}
