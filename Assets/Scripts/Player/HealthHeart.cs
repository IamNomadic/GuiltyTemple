using UnityEngine;
using UnityEngine.UI;

public class HealthHeart : MonoBehaviour
{
    public Sprite Heartfull, Heartempty, Hearthalf;
    private Image heartImage;


    private void Awake()
    {
        heartImage = GetComponent<Image>();
    }

    public void SetHeartImage(HeartStatus status)
    {
        switch (status)
        {
            case HeartStatus.Empty:
                heartImage.sprite = Heartempty;
                break;
            case HeartStatus.Full:
                heartImage.sprite = Heartfull;
                break;
            case HeartStatus.Half:
                heartImage.sprite = Hearthalf;
                break;
        }
    }
}

public enum HeartStatus
{
    Empty = 0,
    Half = 1,
    Full = 2
}