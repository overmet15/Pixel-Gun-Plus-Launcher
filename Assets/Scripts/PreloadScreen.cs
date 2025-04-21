using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class PreloadScreen : MonoBehaviour
{
    [SerializeField] private UILabel loadingText;

    IEnumerator Start()
    {
        yield return SetTheme();
        yield return GetPreviewImages();
        yield return GetCurrentVersion();
        yield return GetCurrentVersionLauncher();

        yield return DownloadManager.CheckBuild();
        SceneManager.LoadScene("Launcher");
    }

    IEnumerator SetTheme()
    {
        Theme.TryGet(PrefsManager.theme, out Preload.currentTheme);
        
        UnityWebRequest request = UnityWebRequest.Get(Global.themeLink);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Preload.seasonalTheme = request.downloadHandler.text;
            if (PrefsManager.seasonalTheme) Theme.TryGet(request.downloadHandler.text, out Preload.currentTheme);
            Debug.Log($"Loaded Theme: {request.downloadHandler.text}");
        }
        else Error($"Couldn't Load Theme: {request.downloadHandler.text}");
        if (Preload.currentTheme == null) Theme.TryGet("Menu_North_Pole", out Preload.currentTheme);
    }

    IEnumerator GetPreviewImages()
    {
        UnityWebRequest request = UnityWebRequest.Get(Global.previewImagesCountLink);
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Error("Couldn't Get Preview Image Count");
            var taskErr = Cache.CachePreviewImages(0);
            yield return new WaitUntil(() => taskErr.IsCompleted);

            yield break;
        }

        var task = Cache.CachePreviewImages(int.Parse(request.downloadHandler.text));
        yield return new WaitUntil(() => task.IsCompleted);
    }

    IEnumerator GetCurrentVersion()
    {
        UnityWebRequest request = UnityWebRequest.Get(Global.versionLink);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success) 
        {
            Error("Unable to get current game version.", $"Couldnt get version: {request.error}");
            Preload.GameVersion = Version.Parse("0.0.0");
        }
        else Preload.GameVersion = Version.Parse(request.downloadHandler.text);
    }

    IEnumerator GetCurrentVersionLauncher()
    {
        UnityWebRequest request = UnityWebRequest.Get(Global.versionLink);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Error("Unable to get current launcher version.", $"Couldnt get version: {request.error}");
            Preload.newVersionAviable = false;
            Preload.LauncherVersionAviable = Version.Parse("0.0.0");
            yield break;
        }
        else
        {
            Preload.LauncherVersionAviable = Version.Parse(request.downloadHandler.text);
            Preload.newVersionAviable = Preload.LauncherVersionAviable == Version.Parse(Application.version);
        }


    }

    void Error(string error, string debugOutput)
    {
        Debug.LogError(debugOutput);
        loadingText.text = error;
    }

    void Error(string error)
    {
        Error(error, error);
    }
}
