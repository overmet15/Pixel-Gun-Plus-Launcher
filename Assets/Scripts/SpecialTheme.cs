using UnityEngine;

public class SpecialTheme : MonoBehaviour
{
    public UISprite[] sprites;

    public UISprite[] topbarSprites;

    public GameObject tb, tbs, bg, spBg, spsBg;

    private Theme theme;

    private UITexture background;

    private void Awake()
    {
        background = bg.GetComponent<UITexture>();
        ThemeChanged();
        ThemeSelector.onThemeChanged += ThemeChanged;
    }

    private void ThemeChanged()
    {
        theme = Preload.currentTheme;
        tb.SetActive(!theme.isSpecial);
        tbs.SetActive(theme.isSpecial);
        bg.SetActive(theme.isSpecial);
        spBg.SetActive(!theme.isSpecial);
        spsBg.SetActive(theme.isSpecial);

        if (!theme.isSpecial) return;

        background.mainTexture = theme.noiseImage;
        background.color = theme.noiseColor;

        foreach (UISprite sprite in sprites)
        {
            sprite.color = theme.buttonColor;
            if (sprite.GetComponent<UIButton>() != null)
            {
                UIButton button = sprite.GetComponent<UIButton>();
                button.hover = theme.buttonColor;
                button.pressed = theme.buttonColor;
                button.disabledColor = theme.buttonColor;
            }
        }
        foreach (UISprite sprite in topbarSprites)
        {
            sprite.color = theme.topBarColor;
        }
    }
}
