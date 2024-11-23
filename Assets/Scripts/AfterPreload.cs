using UnityEngine.UI;
using UnityEngine;

public class AfterPreload : MonoBehaviour
{
    [SerializeField] private Image backgroundImage, topImage, progressImage, logoImage, logoImage2;
    void Awake()
    {
        logoImage.sprite = logoImage2.sprite = Preload.logoSprite;
        backgroundImage.sprite = Preload.backgroundSprite;
        topImage.color = Preload.topBarColor;
        progressImage.color = Preload.progressBarColor;
    }
}
