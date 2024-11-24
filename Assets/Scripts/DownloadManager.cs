using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public enum DownloadState { notDownloading, inProcess, finished }

public class DownloadManager : MonoBehaviour
{
    [SerializeField] private GameObject playPanel, downloadingPanel;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private Text downloadingText, procentText;
    [SerializeField] private Animator animator;

    [SerializeField] private Manager manager;

    public static DownloadState currentDownloadState;

    public void Download()
    {
        if (currentDownloadState != DownloadState.notDownloading) return;

        Application.runInBackground = true; // make sure nothing breaks.

        playPanel.SetActive(false);
        downloadingPanel.SetActive(true);

        progressSlider.maxValue = 1;
        progressSlider.value = 0;

        //downloadingText.text = $"Downloading {Preload.GameVersion}"; - Disabled due being broken in builds
        downloadingText.text = $"Downloading...";
        procentText.text = "0%";

        currentDownloadState = DownloadState.inProcess;

        StartCoroutine(DownloadGame());
    }

    public static async Task CheckBuild()
    {
        if (!Directory.Exists(Global.GameFolderPath)) Directory.CreateDirectory(Global.GameFolderPath);

        bool exeExists = File.Exists(Global.GameExePath);
        bool versionExists = File.Exists(Global.GameVersionPath);

        if (!exeExists)
        {
            if (versionExists) File.Delete(Global.GameVersionPath);
            Global.buildState = BuildState.noBuild;
            return;
        }

        if (!versionExists) { Global.buildState = BuildState.unknownBuild; return; }

        string rawVer = await File.ReadAllTextAsync(Global.GameVersionPath);
        if (Version.TryParse(rawVer, out Version ver))
        {
            Global.localVersion = ver;

            if (Preload.GameVersion == ver) Global.buildState = BuildState.upToDate;
            else if (Preload.GameVersion > ver) Global.buildState = BuildState.updateNeeded;
            else Global.buildState = BuildState.unknownBuild;
        }
        else Global.buildState = BuildState.unknownBuild;
    }

    public IEnumerator DownloadGame()
{
    UnityWebRequest request = UnityWebRequest.Get(Global.gameDownloadLink);

    request.SendWebRequest();

    while (!request.isDone)
    {
        procentText.text = (request.downloadProgress * 100).ToString("F1") + "%";

        progressSlider.value = request.downloadProgress;

        yield return null;
    }

    if (request.result != UnityWebRequest.Result.Success)
    {
        Debug.LogError(request.error);

        animator.SetBool("UIEnabled", false);

        yield return new WaitForSecondsRealtime(2);

        Application.Quit();
        yield break;
    }

    currentDownloadState = DownloadState.finished;

    animator.SetBool("UIEnabled", false);

    yield return new WaitForSeconds(0.5f); // Let it play.

    using (FileStream fileStream = new FileStream(Global.TempZipPath, FileMode.Create, FileAccess.Write, FileShare.None))
    {
        var writeTask = Task.Run(async () => 
        {
            await fileStream.WriteAsync(request.downloadHandler.data, 0, request.downloadHandler.data.Length);
        });

        // Wait until the task completes.
        yield return new WaitUntil(() => writeTask.IsCompleted);
    }

    yield return new WaitForSecondsRealtime(0.2f); // Small delay to ensure write completion.

    if (!Directory.Exists(Global.GameFolderPath)) 
        Directory.CreateDirectory(Global.GameFolderPath);

    try
    {
        ZipFile.ExtractToDirectory(Global.TempZipPath, Global.GameFolderPath, true);
    }
    catch (Exception ex)
    {
        Debug.LogError($"Error extracting zip: {ex.Message}");
    }

    currentDownloadState = DownloadState.notDownloading;

    yield return File.WriteAllTextAsync(Global.GameVersionPath, Preload.GameVersion.ToString());

    downloadingPanel.SetActive(false);
    playPanel.SetActive(true);

    yield return CheckBuild();

    StartCoroutine(manager.Check(false));

    File.Delete(Global.TempZipPath);

    animator.SetBool("UIEnabled", true);
    }

}
