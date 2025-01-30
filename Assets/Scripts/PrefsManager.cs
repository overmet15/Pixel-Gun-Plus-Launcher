using UnityEngine;

public static class PrefsManager
{

    public static bool debugMode
    {
        get => PlayerPrefs.GetInt("DebugMode", 0) == 1;
        set => PlayerPrefs.SetInt("DebugMode", BoolToInt(value));
    }

    public static string language
    {
        get => PlayerPrefs.GetString("Language", "english");
        set => PlayerPrefs.SetString("Language", value);
    }

    private static int BoolToInt(bool bol)
    {
        if (bol) return 1;
        return 0;
    }
}
