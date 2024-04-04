using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject PauseUi;
        
    
    public void Pause()
    {
        if (GameIsPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
    void ResumeGame()
    {
        PauseUi.SetActive(false);
        Time.timeScale = 1;
        GameIsPaused = false;
    }
    void PauseGame()
    {
        PauseUi.SetActive(true);
        Time.timeScale = 0;
        GameIsPaused = true;
    }
}

