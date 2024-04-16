using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StartMenu : MonoBehaviour
{
    public AudioSource Source;
    public AudioClip StartSFX;
    public AudioClip ButtonHover;
    public AudioClip ButtonPressed;
    public void OnPlayButton()
    {
        Source.PlayOneShot(ButtonPressed);
        Source.PlayOneShot(StartSFX);
        StartCoroutine(StartScene());
    }
    public void OnHover()
    {
        Source.PlayOneShot(ButtonHover);
    }
    public void OnOptionsButton()
    {
        Source.PlayOneShot(ButtonPressed);
    }
    public void OnExitButton()
    {
        Source.PlayOneShot(ButtonPressed);
        StartCoroutine(ExitGame());
    }
    //functions
    IEnumerator StartScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("SampleScene");
    }
    IEnumerator OpenOptions()
    {
        yield return new WaitForSeconds(.5f);
    }
    IEnumerator ExitGame()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Exited Game");
        Application.Quit();
    }
}
