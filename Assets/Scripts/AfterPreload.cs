using UnityEngine;

public class AfterPreload : MonoBehaviour
{
    [SerializeField] private UITexture backgroundImage, logoImage, logoImage2;
    [SerializeField] private UISprite progressBar, topBar;
    public void Awake()
    {
        if (Preload.currentTheme == null) return;

        Load(Preload.currentTheme);
    }

    public void Load(Theme theme)
    {
        logoImage.mainTexture = logoImage2.mainTexture = theme.logoImage;
        backgroundImage.mainTexture = theme.backgroundImage;
        topBar.color = theme.topBarColor;
        progressBar.color = theme.progressBarColor;
    }
}
