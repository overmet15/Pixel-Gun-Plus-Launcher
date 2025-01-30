using UnityEngine;


public enum BuildState
{
    noBuild, updateNeeded, unknownBuild, upToDate
}

public static class Global
{
    public const string themeLink = "https://pixelgun.plus/~1031/pixelgun3d-config/menu/menu_all.txt";
    public const string prewievImageLink = "https://pixelgun.plus/~1031/Screenshots/";
    //public const string newsLink = "https://pixelgun.plus/~1031/pixelgun3d-config/lobbyNews/LobbyNews_all.json";
    public const string versionLink = "https://pixelgun.plus/~1031/Downloads/version.txt";
    //public const string gameDownloadLink = "C:/Users/PC/Downloads/test.zip";
    public const string gameDownloadLink = "https://pixelgun.plus/~1031/Downloads/Windows/game.zip";
    
    public static string GameFolderPath => Application.streamingAssetsPath + "/../../Game";
    public static string GameExePath => GameFolderPath + "/Pixel Gun Plus.exe";
    public static string GameVersionPath => GameFolderPath + "/version.txt";
    public static string TempZipPath => Application.streamingAssetsPath + "/../../Temp.zip";
    
    public static bool downloadingGame;
    public static BuildState buildState;
    public static Version localVersion;
}
