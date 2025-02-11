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

    private static bool GetBool(string key, bool defaultValue = false)
    {
        return PlayerPrefs.GetInt(key, defaultValue ? 1 : 0) == 1;
    }

    private static void SetBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
    }
}
