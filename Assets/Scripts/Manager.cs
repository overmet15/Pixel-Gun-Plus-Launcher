using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using System.IO.Compression;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public static string GamePath => Application.streamingAssetsPath + "/../../Game";
    public static string TempZipPath => Application.streamingAssetsPath + "/../../Temp.zip";

    [SerializeField] private Text mainText, verText, downloadingText, procentText;
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject main, downloading;
    [SerializeField] private Animator canvasAnimator;

    private int currentRelease, currentUpdate, currentPatch;

    private bool needsDownload;
    private bool hasBuild;

    public GameObject unaviableScreen;

    public bool downloadingGame;

    // To Do: redo the whole fucking loading system
    IEnumerator Start()
    {
        ProcessHandler.unaviableScreen = unaviableScreen;
        yield return StartCoroutine(GetCurrentVersion());

        if (File.Exists(TempZipPath))
        {
            StartCoroutine(AfterDownload());
            yield break;
        }

        ProcessHandler.hWnd = ProcessHandler.GetForegroundWindow();
        Application.runInBackground = true;

        downloading.SetActive(false);
        main.SetActive(true);

        if (!Directory.Exists(GamePath)) Directory.CreateDirectory(GamePath);
        if (File.Exists(GamePath + "/version.txt"))
        {
            string s = File.ReadAllText(GamePath + "/version.txt");
            string[] ss = s.Split('.');

            if (currentPatch > int.Parse(ss[2])) needsDownload = true;
            else if (currentUpdate > int.Parse(ss[1])) needsDownload = true;
            else if (currentRelease > int.Parse(ss[0])) needsDownload = true;
            hasBuild = true;
        }
        else
        {
            hasBuild = false;
            needsDownload = true;
        }

        if (needsDownload && hasBuild)
        {
            verText.text = "Update Required";
            mainText.text = "Update";
        }
        else if (needsDownload)
        {
            verText.text = string.Empty;
            mainText.text = "Download";
        }
        else
        {
            verText.text = $"{currentRelease}.{currentUpdate}.{currentPatch}";
            mainText.text = "Play";
        }

        yield return new WaitForSeconds(1f); // finish anim

        canvasAnimator.SetTrigger("C");
    }
    
    IEnumerator GetCurrentVersion()
    {
        UnityWebRequest request = UnityWebRequest.Get("https://pixelgun.plus/~1031/Downloads/version.txt");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success) Application.Quit();

        string[] s = request.downloadHandler.text.Split(".");

        currentRelease = int.Parse(s[0]);
        currentUpdate = int.Parse(s[1]);
        currentPatch = int.Parse(s[2]);
    }

    IEnumerator Download()
    {
        downloadingGame = true;

        downloadingText.text = $"Downloading {currentRelease}.{currentUpdate}.{currentPatch}";
        main.SetActive(false);
        downloading.SetActive(true);
        UnityWebRequest request = UnityWebRequest.Get("https://pixelgun.plus/download.php?id=10.3.1-win-game-x86_64");

        request.SendWebRequest();
        slider.maxValue = 1;

        while (!request.isDone)
        {
            int downloadedMB = Mathf.RoundToInt(request.downloadedBytes / (1024f * 1024f));
            procentText.text = Mathf.RoundToInt(request.downloadProgress * 100) + "%";

            slider.value = request.downloadProgress;

            yield return null;
        }

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
            Application.Quit();
        }

        using (FileStream fileStream = new FileStream(TempZipPath, FileMode.Create, FileAccess.Write))
        {
            fileStream.Write(request.downloadHandler.data, 0, request.downloadHandler.data.Length);
        }

        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(0);
    }

    IEnumerator AfterDownload()
    {
        downloadingText.text = "Unzipping...";
        yield return new WaitForSeconds(1f);

        try
        {
            Utils.UnzipFile(TempZipPath, GamePath + "/");
        }
        catch
        {
            Debug.LogError("Unable to unzip game files");
            Application.Quit();
            yield break;
        }

        yield return new WaitForSeconds(1f);
        yield return File.WriteAllTextAsync(GamePath + "/version.txt", $"{currentRelease}.{currentUpdate}.{currentPatch}");

        File.Delete(TempZipPath);
        yield return null;
        downloadingGame = true;
        SceneManager.LoadScene("Launcher");
    }

    // UGUI things
    public void OnMainButton()
    {
        if (needsDownload)
        {
            StartCoroutine(Download());
        }
        else
        {
            ProcessHandler.StartMonitoringProcess();
        }
    }

    public void OnCreditsButton()
    {
        canvasAnimator.SetTrigger("Credits");
    }

    public void OnSettingsButton()
    {
        canvasAnimator.SetTrigger("Options");
    }

    public void OnQuitButton()
    {
        if (downloadingGame) StopAllCoroutines();
        Application.Quit();
    }
}

public static class Utils
{

    public static void UnzipFile(string zipFilePath, string extractPath)
    {
        try
        {
            // Check if the zip file exists
            if (File.Exists(zipFilePath))
            {
                Directory.CreateDirectory(extractPath);

                ZipFile.ExtractToDirectory(zipFilePath, extractPath, true);
                Debug.Log("Files extracted to: " + extractPath);
            }
            else
            {
                Debug.LogWarning("Zip file does not exist: " + zipFilePath);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error extracting zip file: " + e.Message);
        }
    }
}