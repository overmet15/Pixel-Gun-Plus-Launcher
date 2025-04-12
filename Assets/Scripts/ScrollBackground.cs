using UnityEngine;

public class ScrollBackground : MonoBehaviour
{
    private UITexture background;

    private Rect rect = new Rect();

    private void Awake()
    {
        background = GetComponent<UITexture>();
        ThemeChanged();
    }
    
    private void OnEnable()
    {
        ThemeSelector.onThemeChanged += ThemeChanged;
    }

    private void OnDisable()
    {
        ThemeSelector.onThemeChanged -= ThemeChanged;
    }
    
    private void Update()
    {
        rect.x += Time.deltaTime * 0.005f;
        rect.y -= Time.deltaTime * 0.005f;
        if (rect.x >= 2f) rect.x = 0f;
        if (rect.y <= -2f) rect.y = 0f;
        background.uvRect = rect;
    }

    private void ThemeChanged()
    {
        rect.width = Preload.currentTheme.rectSize;
        rect.height = Preload.currentTheme.rectSize;
    }
}
