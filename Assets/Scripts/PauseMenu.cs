using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused;
    public GameObject PauseUi;


    public void Pause()
    {
        if (GameIsPaused)
            ResumeGame();
        else
            PauseGame();
    }

    private void ResumeGame()
    {
        PauseUi.SetActive(false);
        Time.timeScale = 1;
        GameIsPaused = false;
    }

    private void PauseGame()
    {
        PauseUi.SetActive(true);
        Time.timeScale = 0;
        GameIsPaused = true;
    }
}