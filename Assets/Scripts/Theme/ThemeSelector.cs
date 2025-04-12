using UnityEngine;
using System;

public class ThemeSelector : MonoBehaviour
{
    public UIToggle[] toggles;

    public ThemeSelection[] themes;

    private string selectedTheme;

    public AfterPreload afterPreload;

    public static Action onThemeChanged;

    private void Start()
    {
        toggles[0].GetComponent<ButtonHandler>().Clicked += SetSeasonal;
        toggles[0].value = PrefsManager.seasonalTheme;
        for (int i = 1; i < toggles.Length; i++)
        {
            toggles[i].value = !PrefsManager.seasonalTheme? themes[i].themeName == PrefsManager.theme : false;
        }
    }

    public void SetTheme(string themeToSet)
    {
        selectedTheme = themeToSet;
        for (int i = 0; i < toggles.Length; i++)
        {
            if (i == 0) toggles[0].value = false;
            else toggles[i].value = themes[i].themeName == themeToSet;
        }
        PrefsManager.theme = selectedTheme;
        Theme.TryGet(selectedTheme, out Preload.currentTheme);
        PrefsManager.seasonalTheme = false;
        afterPreload.Load(Preload.currentTheme);
        onThemeChanged();
    }

    private void SetSeasonal(object sender, EventArgs e)
    {
        PrefsManager.seasonalTheme = true;
        selectedTheme = Preload.seasonalTheme;
        PrefsManager.theme = selectedTheme;
        Theme.TryGet(selectedTheme, out Preload.currentTheme);
        themes[0].themeName = Preload.seasonalTheme;
        for (int i = 0; i < toggles.Length; i++)
        {
            toggles[i].value = false;
        }
        toggles[0].value = true;
        afterPreload.Load(Preload.currentTheme);
        onThemeChanged();
    }
}
