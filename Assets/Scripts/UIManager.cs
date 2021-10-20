using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _panelWin;
    [SerializeField] private GameObject _panelLose;

    public void Win() => _panelWin.SetActive(true);
    public void Lose() => _panelLose.SetActive(true);

    public void TryAgain()
    {
        SceneManager.LoadScene(0);
    }
}
