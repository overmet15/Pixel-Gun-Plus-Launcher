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

    //public const string gameDownloadLink = "https://pixelgun.plus/~1031/Downloads/Windows/game.zip";
    public const string gameDownloadLink = "https://pixelgun.plus/~1031/Downloads/Windows/picklegame.zip";
    public const string launcherDownloadLink = "https://pixelgun.plus/~1031/Downloads/Windows/picklegame.zip";
    public const string gameExecutableName = "Pixel Gun Plus.exe";
    public const string subDirName = "PixelGunPlus";

#if !UNITY_EDITOR
    public static string DefaultGameFolderPath => Application.dataPath + "/../Game/PixelGunPlus";
    public static string TempZipPath => Application.dataPath + "/../Temp.zip";
    public static string ChachePath => Application.dataPath + "/../Chache";
#else
    public static string DefaultGameFolderPath => Application.dataPath + "/../_Runtime/Game/PixelGunPlus";
    public static string TempZipPath => Application.dataPath + "/../_Runtime/Temp.zip";
    public static string ChachePath => Application.dataPath + "/../_Runtime/Chache";
#endif

#if UNITY_STANDALONE_LINUX
    public static string GameExecutablePath => PrefsManager.gamePath + "/Pixel Gun Plus.x86_64";
#else
    public static string GameExecutablePath => PrefsManager.gamePath + "/Pixel Gun Plus.exe";

#endif
    public static string GameVersionPath => PrefsManager.gamePath + "/version.txt";
    public static string PreviewImagesChachePath => ChachePath + "/PreviewImages";
    public static string NewsPreviewPictureChachePath => ChachePath + "/News/PreviewImages";
    public static string NewsFullpictureChachePath => ChachePath + "/News/Fullpictures";
    public static string NewsReadPath => ChachePath + "/News/ReadedNews.json";

    public static bool downloadingGame;
    public static BuildState buildState;
    public static Version localVersion;
}
