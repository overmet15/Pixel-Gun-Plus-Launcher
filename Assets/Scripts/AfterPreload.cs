using UnityEngine;

public class AfterPreload : MonoBehaviour
{
    [SerializeField] private UITexture backgroundImage, logoImage, logoImage2, splashImage;
    [SerializeField] private UISprite progressBar, topBar, shadow;
    public void Awake()
    {
        if (Preload.currentTheme == null) return;

        Load(Preload.currentTheme);
    }

    public void Load(Theme theme)
    {
        logoImage.mainTexture = logoImage2.mainTexture = theme.logoImage;
        backgroundImage.mainTexture = theme.backgroundImage;
        splashImage.mainTexture = theme.backgroundImage;
        topBar.color = theme.topBarColor;
        progressBar.color = theme.progressBarColor;
        shadow.color = theme.topBarColor;
    }
}
