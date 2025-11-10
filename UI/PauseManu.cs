using TowerDefence;
using UnityEngine;

public class PauseManu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    private bool isPaused;
    public bool IsPaused => isPaused;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (isPaused) Resume();
        else Pause();
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;

        pauseMenu.SetActive(true);
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;

        pauseMenu.SetActive(false);
    }

    public void ExitToMenu()
    {
        Time.timeScale = 1f;

        LevelSequenceController.Instance.ExitToMenu();
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;

        LevelSequenceController.Instance.RestartLevel();
    }
}
