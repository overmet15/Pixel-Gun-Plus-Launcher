using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PreloadScreen : MonoBehaviour
{
    [SerializeField] private Animator loadingAnimator;
    [SerializeField] private Text loadingText;

    private string[] supportedThemes = new string[4]{"Menu_Colapsed_City", "Menu_Christmas", "Menu_Mine", "Menu_Heaven"};
    IEnumerator Start()
    {
        string[] args = System.Environment.GetCommandLineArgs();
        //string[] args = new string[1]{"-theme=Menu_Mine"};

        bool set = false;
        foreach (string arg in args)
        {
            if (arg.StartsWith("-theme="))
            {
                Preload.currentTheme = arg.Substring(7);
                set = true;
                break;
            }
        }
        if (set)
        {
            if (!ThemeCheck()) yield return StartCoroutine(GetCurrentTheme());
        }
        else yield return StartCoroutine(GetCurrentTheme());
        
        if (!ThemeCheck())
        {
            yield return StartCoroutine(Error("Invalid Theme.", $"Invalid Theme: {Preload.currentTheme}", false));
            Preload.currentTheme = "Menu_Colapsed_City";
        }

        SetUp();
        Preload.backgroundSprite = Resources.Load<Sprite>(Preload.currentTheme);

        yield return StartCoroutine(GetPreviewImages());
        yield return StartCoroutine(GetCurrentVersion());

        SceneManager.LoadScene("Launcher");
    }

    IEnumerator GetCurrentTheme()
    {
        UnityWebRequest request = UnityWebRequest.Get("https://pixelgun.plus/~1031/pixelgun3d-config/menu/menu_all.txt");

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
        UnityWebRequest request = UnityWebRequestTexture.GetTexture("https://pixelgun.plus/~1031/Screenshots/" + Random.Range(1, 3));
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            Preload.previewImage = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }

    }

    IEnumerator GetCurrentVersion()
    {
        UnityWebRequest request = UnityWebRequest.Get("https://pixelgun.plus/~1031/Downloads/version.txt");

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
        return Preload.currentTheme == "Menu_Colapsed_City" || Preload.currentTheme == "Menu_Christmas" || Preload.currentTheme == "Menu_Mine" || Preload.currentTheme == "Menu_Heaven";
    }

    void SetUp()
    {
        switch (Preload.currentTheme)
        {
            case "Menu_Colapsed_City":
            Preload.topBarColor = Utils.ColorToUColor(156, 82, 0, 130);
            Preload.progressBarColor = Utils.ColorToUColor(237, 112, 0);
            Preload.logoSprite = Resources.Load<Sprite>("logo/normal");
            break;
            
            case "Menu_Christmas":
            Preload.topBarColor = Utils.ColorToUColor(0, 90, 140, 130);
            Preload.progressBarColor = Utils.ColorToUColor(0, 150, 200);
            Preload.logoSprite = Resources.Load<Sprite>("logo/christmas");
            break;
            
            case "Menu_Mine":
            Preload.topBarColor = Utils.ColorToUColor(0, 150, 50, 130);
            Preload.progressBarColor = Utils.ColorToUColor(0, 180, 40);
            Preload.logoSprite = Resources.Load<Sprite>("logo/summer");
            break;
            
            case "Menu_Heaven":
            Preload.topBarColor = Utils.ColorToUColor(255, 205, 75, 130);
            Preload.progressBarColor = Utils.ColorToUColor(255, 205, 75);
            Preload.logoSprite = Resources.Load<Sprite>("logo/spring");
            break;
        }
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
