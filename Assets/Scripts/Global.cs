using System.IO;
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
    public static string DefaultGameFolderPath => Path.Combine(Application.dataPath, "..", "PixelGunPlus");
    public static string TempZipPath => Path.Combine(Application.dataPath, "..", "GameDownload_DoNotTouch.zip");
    public static string CachePath => Path.Combine(Application.dataPath, "..", "Cache");
#else
    public static string DefaultGameFolderPath => Path.Combine(Application.dataPath, "..", "_Runtime", subDirName);
    public static string TempZipPath => Path.Combine(Application.dataPath, "..", "_Runtime", "GameDownload_DoNotTouch.zip");
    public static string CachePath => Path.Combine(Application.dataPath, "..", "_Runtime", "Cache");
#endif

#if UNITY_STANDALONE_LINUX
    public static string GameExecutablePath => Path.Combine(PrefsManager.gamePath, "Pixel Gun Plus.x86_64");
#else
    public static string GameExecutablePath => Path.Combine(PrefsManager.gamePath, "Pixel Gun Plus.exe");
#endif

    public static string GameVersionPath => Path.Combine(PrefsManager.gamePath, "version.txt");
    public static string NewsReadPath => Path.Combine(CachePath, "News_DoNotDelete.json");


    public static bool downloadingGame;
    public static BuildState buildState;
    public static bool installed;
    public static Version localVersion;
}
