using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.VFX;

public enum DownloadState { notDownloading, inProcess, finished }

public class DownloadManager : MonoBehaviour
{
    [SerializeField] private GameObject playPanel, downloadingPanel;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private Text downloadingText, procentText;
    [SerializeField] private Animator animator;

    [SerializeField] private Manager manager;

    public static DownloadState currentDownloadState;

    public static UnityEvent<DownloadState> onDownloadStatusChange = new();

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
        onDownloadStatusChange.Invoke(DownloadState.inProcess);

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

    IEnumerator DownloadGame()
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
            LogError(request.error);

            animator.SetBool("UIEnabled", false);

            yield return new WaitForSecondsRealtime(2);

            Application.Quit();
            yield break;
        }

        currentDownloadState = DownloadState.finished;
        onDownloadStatusChange.Invoke(DownloadState.finished);

        animator.SetBool("UIEnabled", false);

        yield return new WaitForSeconds(0.5f); // Let it play.

        using (FileStream fileStream = new(Global.TempZipPath, FileMode.Create, FileAccess.Write, FileShare.Read))
        {
            Log(request.downloadedBytes.ToString());
            yield return fileStream.WriteAsync(request.downloadHandler.data, 0, request.downloadHandler.data.Length);
            yield return fileStream.DisposeAsync();
        }

        yield return new WaitForSecondsRealtime(0.15f);

        if (!Directory.Exists(Global.GameFolderPath)) 
            Directory.CreateDirectory(Global.GameFolderPath);

        Process unzipProgress = Utils.Unzip(Global.TempZipPath, Global.GameFolderPath);

        while (!unzipProgress.HasExited) yield return null;

        if (unzipProgress.ExitCode != 0) LogWarning($"Unzip process error: {unzipProgress.ExitCode}");

        currentDownloadState = DownloadState.notDownloading;
        onDownloadStatusChange.Invoke(DownloadState.notDownloading);

        if (unzipProgress.ExitCode != 2) yield return File.WriteAllTextAsync(Global.GameVersionPath, Preload.GameVersion.ToString());

        downloadingPanel.SetActive(false);
        playPanel.SetActive(true);

        yield return CheckBuild();

        StartCoroutine(manager.Check(false));

        File.Delete(Global.TempZipPath);

        animator.SetBool("UIEnabled", true);
    }

    static void Log(string log)
    {
        UnityEngine.Debug.Log(log);
    }

    static void LogWarning(string war)
    {
        UnityEngine.Debug.LogWarning(war);
    }
    static void LogError(string war)
    {
        UnityEngine.Debug.LogWarning(war);
    }
}