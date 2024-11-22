using UnityEngine;

public static class PrefsManager
{
    public static bool closeOnGameStart
    {
        get => PlayerPrefs.GetInt("CloseOnGameStart", 1) == 1;
        set => PlayerPrefs.SetInt("CloseOnGameStart", boolToInt(value));
    }

    private static int boolToInt(bool bol)
    {
        if (bol) return 1;
        return 0;
    }
}
