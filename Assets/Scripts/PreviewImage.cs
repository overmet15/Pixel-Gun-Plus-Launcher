using UnityEngine;

public class PreviewImage : MonoBehaviour
{
    public UITexture image;
    public UISprite frame;

    public int currentIndex = 0;

    private void Awake()
    {
        frame.color = Preload.currentTheme.frameColor;
        image.mainTexture = Preload.previewImages[currentIndex];
        ThemeSelector.onThemeChanged += ThemeChanged;
    }

    private void ThemeChanged()
    {
        frame.color = Preload.currentTheme.frameColor;
    }
}
