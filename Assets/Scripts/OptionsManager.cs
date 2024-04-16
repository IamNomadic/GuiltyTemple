using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;

    [SerializeField] private GameObject optionsMenu;

    public AudioClip ButtonHover;
    public AudioClip ButtonPressed;
    public AudioSource Source;

    private void Start()
    {
        optionsMenu.SetActive(false);
        musicSlider.value = 100;
    }

    public void ChangeVolume()
    {
        AudioListener.volume = musicSlider.value;
    }

    public void OnHover()
    {
        Source.PlayOneShot(ButtonHover);
    }

    public void ExitOptions()
    {
        Source.PlayOneShot(ButtonPressed);
        optionsMenu.SetActive(false);
    }
}