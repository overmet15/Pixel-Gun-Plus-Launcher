using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PreloadScreen : MonoBehaviour
{
    [SerializeField] private Animator loadingAnimator;
    [SerializeField] private Text loadingText;
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

            Theme.TryGet(args[i + 1], out Preload.currentTheme);

            break;
        }

        if (Preload.currentTheme != null) yield break;

        UnityWebRequest request = UnityWebRequest.Get(Global.themeLink);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success) Error("Theme Request Error.", request.error);
        else if (Theme.TryGet(request.downloadHandler.text, out Theme theme)) Preload.currentTheme = theme;
    }

    IEnumerator GetPreviewImages()
    {
        /*List<Sprite> sprites = new();

        for (int i = 1; i <= 3; i++)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture("https://pixelgun.plus/~1031/Screenshots/" + i);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                sprites.Add(Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)));
            }
        }
        Preload.previewImages = sprites.Count != 0 ? sprites : null;*/
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(Global.prewievImageLink + Random.Range(1, 3));
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            Preload.previewImage = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
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
            yield break;
        }
        else Preload.newVersionAviable = Version.Parse(request.downloadHandler.text) == Version.Parse(Application.version);
    }

    void Error(string error, string debugOutput)
    {
        Debug.LogError(debugOutput);

        loadingAnimator.enabled = false;
        loadingText.text = error;
        loadingText.color = Utils.ColorToUColor(255, 0, 0);
    }
}
