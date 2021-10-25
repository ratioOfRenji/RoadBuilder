using UnityEngine;
using UnityEngine.UI;

public class LevelProgressUI : MonoBehaviour
{
    [Header("UI references :")]
    [SerializeField] private Image _uiFillImage;
    [SerializeField] private Text _uiStartText;
    [SerializeField] private Text _uiEndText;

    [Header("Player & Endline references :")]
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _endLineTransform;

    private Vector3 _endLinePosition;
    private float _fullDistance;

    private void Start()
    {
        _endLinePosition = _endLineTransform.position;
        _fullDistance = GetDistance();
    }

    public void SetLevelTexts(int level)
    {
        _uiStartText.text = level.ToString();
        _uiEndText.text = (level + 1).ToString();
    }

    private float GetDistance() => (_endLinePosition - _playerTransform.position).sqrMagnitude;
    
    private void UpdateProgressFill(float value) => _uiFillImage.fillAmount = value;

    private void Update()
    {
        if (_playerTransform.position.z <= _endLinePosition.z)
        {
            float newDistance = GetDistance();
            float progressValue = Mathf.InverseLerp(_fullDistance, 0f, newDistance);

            UpdateProgressFill(progressValue);
        }
    }
}
