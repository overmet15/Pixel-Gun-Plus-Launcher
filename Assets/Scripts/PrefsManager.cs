using UnityEngine;

public static class PrefsManager
{
    public static bool closeOnGameStart
    {
        get => PlayerPrefs.GetInt("CloseOnGameStart", 1) == 1;
        set => PlayerPrefs.SetInt("CloseOnGameStart", boolToInt(value));
    }

    public static bool debugMode
    {
        get => PlayerPrefs.GetInt("DebugMode", 0) == 1;
        set => PlayerPrefs.SetInt("DebugMode", boolToInt(value));
    }

    private static int boolToInt(bool bol)
    {
        if (bol) return 1;
        return 0;
    }
}
