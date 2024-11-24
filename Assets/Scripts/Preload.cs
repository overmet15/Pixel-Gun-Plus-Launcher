using System.Collections.Generic;
using UnityEngine;

public struct Version
{
    public int major, minor, patch;
}

public static class Preload
{
    public static string currentTheme = string.Empty;

    public static Theme currentThemeObject;

    public static Sprite previewImage;

    public static Version GameVersion;
}
