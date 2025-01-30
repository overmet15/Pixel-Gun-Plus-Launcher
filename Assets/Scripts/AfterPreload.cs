using UnityEngine;

public class AfterPreload : MonoBehaviour
{
    [SerializeField] private UITexture backgroundImage, logoImage, logoImage2;
    [SerializeField] private UISprite progressBar, topBar;
    public void Awake()
    {
        logoImage.mainTexture = logoImage2.mainTexture = Preload.currentThemeObject.logoImage;
        backgroundImage.mainTexture = Preload.currentThemeObject.backgroundImage;
        topBar.color = Preload.currentThemeObject.topBarColor;
        progressBar.color = Preload.currentThemeObject.progressBarColor;
    }
}
