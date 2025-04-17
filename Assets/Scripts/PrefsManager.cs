using UnityEngine;

public static class PrefsManager
{
    public static bool debugMode
    {
        get => GetBool("DebugMode", false);
        set => SetBool("DebugMode", value);
    }

    public static string language
    {
        get => PlayerPrefs.GetString("Language", "english");
        set => PlayerPrefs.SetString("Language", value);
    }

    public static string gamePath
    {
        get => PlayerPrefs.GetString("GamePath", Global.DefaultGameFolderPath);
        set => PlayerPrefs.SetString("GamePath", value);
    }
    public static bool skipIntro
    {
        get => GetBool("skipIntro");
        set => SetBool("skipIntro", value);
    }

    public static bool closeOnPlay
    {
        get => GetBool("closeOnPlay", true);
        set => SetBool("closeOnPlay", value);
    }

    public static bool seasonalTheme
    {
        get => GetBool("seasonalTheme", false);
        set => SetBool("seasonalTheme", value);
    }

    public static string theme
    {
        get => PlayerPrefs.GetString("theme", "Menu_Space");
        set => PlayerPrefs.SetString("theme", value);
    }

    private static bool GetBool(string key, bool defaultValue = false)
    {
        return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
    }

    private static void SetBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
    }
}
