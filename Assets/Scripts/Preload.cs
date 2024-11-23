using System.Collections.Generic;
using UnityEngine;

public struct Version
{
    public int major, minor, patch;
}

public static class Preload
{
    public static string currentTheme = string.Empty;

    public static Sprite backgroundSprite, logoSprite;

    public static Sprite previewImage;

    public static Color progressBarColor, topBarColor;

    public static Version GameVersion;
}
