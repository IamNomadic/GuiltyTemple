using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused;
    public GameObject PauseUi;
    public AudioSource Source;
    public AudioClip ButtonHover;
    public AudioClip ButtonPressed;


    public void Pause()
    {
        if (GameIsPaused)
        {
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
    public void OnHover()
    {
        Source.PlayOneShot(ButtonHover);
    }
    public void OnPlayButton()
    {
        Source.PlayOneShot(ButtonPressed);
        ResumeGame();
    }
    public void OnRestartButton()
    {
        Source.PlayOneShot(ButtonPressed);
        StartCoroutine(RestartGame());
    }
    public void OnExitButton()
    {
        Source.PlayOneShot(ButtonPressed);
        StartCoroutine(ExitGame());
    }
    IEnumerator ExitGame()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Exited Game");
        Application.Quit();
    }
    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}