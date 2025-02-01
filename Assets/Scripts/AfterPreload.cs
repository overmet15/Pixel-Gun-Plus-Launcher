using UnityEngine;

public class AfterPreload : MonoBehaviour
{
    [SerializeField] private UITexture backgroundImage, logoImage, logoImage2;
    [SerializeField] private UISprite progressBar, topBar;
    public void Awake()
    {
        if (Preload.currentTheme == null) return;

        logoImage.mainTexture = logoImage2.mainTexture = Preload.currentTheme.logoImage;
        backgroundImage.mainTexture = Preload.currentTheme.backgroundImage;
        topBar.color = Preload.currentTheme.topBarColor;
        progressBar.color = Preload.currentTheme.progressBarColor;
    }
}
