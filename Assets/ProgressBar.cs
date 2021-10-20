using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private ScoreCounter _counter;

    [SerializeField] private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();     
    }
    private void OnEnable()
    {
        _counter.OnScoreChanged += IncrementSliderValue;
    }

    private void OnDisable()
    {
        _counter.OnScoreChanged -= IncrementSliderValue;
    }

    private void IncrementSliderValue(int value)
    {
        StartCoroutine(Fill(value));
    }

    IEnumerator Fill(int value)
    {     
        _slider.DOValue(value, 1f);
        yield return new WaitForSeconds(1f);
    }
}
