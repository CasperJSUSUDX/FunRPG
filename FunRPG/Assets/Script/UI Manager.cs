using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject pauseUI;
    public GameObject pauseButton;

    public void OnPausePress()
    {
        pauseUI.SetActive(true);
        pauseButton.SetActive(false);
    }
    public void OnResumePress()
    {
        pauseUI.SetActive(false);
        pauseButton.SetActive(true);
    }
    public void OnRestartPress()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OnExitPress()
    {
        Application.Quit();
    }
}
