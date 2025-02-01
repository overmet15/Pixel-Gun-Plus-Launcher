using UnityEngine;


public enum BuildState
{
    noBuild, updateNeeded, unknownBuild, upToDate
}

public static class Global
{
    public const string themeLink = "https://pixelgun.plus/~1031/pixelgun3d-config/menu/menu_all.txt";

    public const string previewImagesCountLink = "https://pixelgun.plus/~1031/Screenshots/image_count.txt";
    public const string previewImagesLink = "https://pixelgun.plus/~1031/Screenshots/";

    public const string newsLink = "https://pixelgun.plus/~1031/pixelgun3d-config/lobbyNews/LobbyNews_all.json";
    public const string versionLink = "https://pixelgun.plus/~1031/Downloads/version.txt";

    public const string gameDownloadLink = "https://pixelgun.plus/~1031/Downloads/Windows/game.zip";
    //public const string gameDownloadLink = "https://pixelgun.plus/~1031/Downloads/Windows/picklegame.zip";

#if !UNITY_EDITOR
    public static string GameFolderPath => Application.streamingAssetsPath + "/../../Game";
    public static string GameExePath => GameFolderPath + "/Pixel Gun Plus.exe";
    public static string GameVersionPath => GameFolderPath + "/version.txt";
    public static string TempZipPath => Application.streamingAssetsPath + "/../../Temp.zip";
    public static string ChachePath => Application.streamingAssetsPath + "/../../Chache";
    public static string PreviewImagesChachePath => ChachePath + "/PreviewImages";
#else
    public static string GameFolderPath => Application.streamingAssetsPath + "/../../_Runtime/Game";
    public static string GameExePath => GameFolderPath + "/Pixel Gun Plus.exe";
    public static string GameVersionPath => GameFolderPath + "/version.txt";
    public static string TempZipPath => Application.streamingAssetsPath + "/../../_Runtime/Temp.zip";
    public static string ChachePath => Application.streamingAssetsPath + "/../../_Runtime/Chache";
    public static string PreviewImagesChachePath => ChachePath + "/PreviewImages";
#endif

    public static bool downloadingGame;
    public static BuildState buildState;
    public static Version localVersion;
}
