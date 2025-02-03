using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

        //ToDo: Make option to skip unnecesary calls, or skip selected ones
        SceneManager.LoadScene("LauncherNew");
    }

    IEnumerator SetTheme()
    {
        string[] args = System.Environment.GetCommandLineArgs();
        //string[] args = new string[2] { "-Theme", "Menu_Christmas" };

        for (int i = 0; i < args.Length; i++)
        {
            if (args[0] != "-Theme") continue;

            if (Theme.TryGet(args[i + 1], out Preload.currentTheme))
            {
                Debug.Log($"Loaded custom theme: {args[i + 1]}");
            }

            break;
        }

        if (Preload.currentTheme != null) yield break;
        UnityWebRequest request = UnityWebRequest.Get(Global.themeLink);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success) Error("Theme Request Error.", request.error);
        else if (Theme.TryGet(request.downloadHandler.text, out Preload.currentTheme))
        {
            Debug.Log($"Loaded Theme: {request.downloadHandler.text}");
        }
        else Error($"Couldn't Load Theme: {request.downloadHandler.text}");
    }

    IEnumerator GetPreviewImages()
    {
        UnityWebRequest request = UnityWebRequest.Get(Global.previewImagesCountLink);
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Error("Couldn't Get Preview Image Count");
            var taskErr = Cache.ChachePreviewImages(0);
            yield return new WaitUntil(() => taskErr.IsCompleted);

            yield break;
        }

        var task = Cache.ChachePreviewImages(int.Parse(request.downloadHandler.text));
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
