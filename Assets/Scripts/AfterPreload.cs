using UnityEngine.UI;
using UnityEngine;

public class AfterPreload : MonoBehaviour
{
    [SerializeField] private Image backgroundImage, topImage, progressImage, logoImage, logoImage2;
    void Awake()
    {
        logoImage.sprite = logoImage2.sprite = Preload.currentThemeObject.logoSprite;
        backgroundImage.sprite = Preload.currentThemeObject.backgroundSprite;
        topImage.color = Preload.currentThemeObject.topBarColor;
        progressImage.color = Preload.currentThemeObject.progressBarColor;
    }
}
