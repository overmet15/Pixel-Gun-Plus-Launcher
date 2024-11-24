using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        string[] args = System.Environment.GetCommandLineArgs();

        bool customTheme = false;
        foreach (string arg in args)
        {
            if (arg.StartsWith("-theme="))
            {
                Preload.currentTheme = arg.Substring(7);
                customTheme = true;
                break;
            }
        }

        if (customTheme)
        {
            if (!ThemeCheck()) yield return StartCoroutine(GetCurrentTheme());
        }
        else yield return StartCoroutine(GetCurrentTheme());
        
        if (!ThemeCheck())
        {
            yield return StartCoroutine(Error("Invalid Theme.", $"Invalid Theme: {Preload.currentTheme}", false));
            Preload.currentTheme = "Menu_Colapsed_City";
        }

        Preload.currentThemeObject = Resources.Load<Theme>("Themes/" + Preload.currentTheme);

        yield return StartCoroutine(GetPreviewImages());
        yield return StartCoroutine(GetCurrentVersion());

        yield return DownloadManager.CheckBuild();
        
        SceneManager.LoadScene("Launcher");
    }

    IEnumerator GetCurrentTheme()
    {
        UnityWebRequest request = UnityWebRequest.Get(Global.themeLink);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            StartCoroutine(Error("Request Error.", request.error));
            yield break;
        }
        Preload.currentTheme = request.downloadHandler.text;
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
            StartCoroutine(Error("Unable to get current version.", $"Couldnt get version: {request.error}"));
            yield break;
        }

        string[] s = request.downloadHandler.text.Split(".");

        Preload.GameVersion = new()
        {
            major = int.Parse(s[0]),
            minor = int.Parse(s[1]),
            patch = int.Parse(s[2])
        };
    }

    bool ThemeCheck()
    {
        return Resources.Load<Theme>("Themes/" + Preload.currentTheme) != null;
    }

    IEnumerator Error(string error, string debugOutput, bool close = true)
    {
        Debug.LogError(debugOutput);

        loadingAnimator.enabled = false;
        loadingText.text = error;
        loadingText.color = Utils.ColorToUColor(255, 0, 0);

        if (close)
        {
            yield return new WaitForSecondsRealtime(5f);

            Application.Quit();
        }
    }
}
