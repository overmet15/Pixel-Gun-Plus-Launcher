using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public enum DownloadState { notDownloading, inProcess, finished }

public class DownloadManager : MonoBehaviour
{
    [SerializeField] private GameObject playPanel, downloadingPanel;
    [SerializeField] private UISlider progressSlider;
    [SerializeField] private UILabel downloadingText, procentText, progressText, speedText;
    [SerializeField] private Manager manager;

    public static DownloadState currentDownloadState;

    public static UnityEvent<DownloadState> onDownloadStatusChange = new();

    public void Download(bool isGame)
    {
        if (currentDownloadState != DownloadState.notDownloading) return;

        playPanel.SetActive(false);
        downloadingPanel.SetActive(true);

        progressSlider.value = 0;

        manager.verText.text = string.Empty;
        string toApply = isGame ? Preload.GameVersion.ToString() : Preload.LauncherVersionAviable.ToString();
        downloadingText.text = $"DOWNLOADING {toApply}";
        procentText.text = "0%";

        currentDownloadState = DownloadState.inProcess;
        onDownloadStatusChange.Invoke(DownloadState.inProcess);

        StartCoroutine(DownloadCoroutine(isGame));
    }

    public static async Task CheckBuild()
    {
        if (!Directory.Exists(Global.GameFolderPath)) Directory.CreateDirectory(Global.GameFolderPath);

        bool exeExists = File.Exists(Global.GameExecutablePath);
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

    void CheckDownloadSpeed(double downloadedBytes, DateTime startTime)
    {
        double speed = downloadedBytes / 1024f / 1024f / (DateTime.Now - startTime).TotalSeconds;
        speed = Math.Round(speed * 10) * 0.1;
        speedText.text = speed + " MB/s";
    }

    IEnumerator DownloadCoroutine(bool isGame)
    {
        // check if there is build
        File.Delete(Global.TempZipPath);

        UnityWebRequest request = UnityWebRequest.Get(Global.gameDownloadLink);

        DateTime startTime = DateTime.Now;
        float checkSpeedTimer = 0f;

        request.SendWebRequest();

        while (!request.isDone)
        {
            string sizeString = request.GetResponseHeader("Content-Length");
            long size = Convert.ToInt64(sizeString) / 1024 / 1024;
            double bytesDownloaded = request.downloadedBytes / 1024f / 1024f;
            bytesDownloaded = Math.Round(bytesDownloaded * 10) * 0.1;
            procentText.text = (request.downloadProgress * 100).ToString("F1") + "%";
            progressSlider.value = request.downloadProgress;
            progressText.text = bytesDownloaded.ToString() + " MB/" + size + " MB";

            checkSpeedTimer += Time.deltaTime;
            if (checkSpeedTimer >= 0.75f)
            {
                CheckDownloadSpeed(request.downloadedBytes, startTime);
                checkSpeedTimer = 0f;
            }
            yield return null;
        }

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);

            yield return new WaitForSecondsRealtime(2);

            Application.Quit();
            yield break;
        }

        currentDownloadState = DownloadState.finished;
        onDownloadStatusChange.Invoke(DownloadState.finished);

        yield return new WaitForSeconds(0.5f); // Let it play.

        using (FileStream fileStream = File.Create(Global.TempZipPath))
        {
            var writeTask = fileStream.WriteAsync(request.downloadHandler.data, 0, request.downloadHandler.data.Length);
            yield return new WaitUntil(() => writeTask.IsCompleted);
            Debug.Log(writeTask.IsCompleted);
        }

        yield return new WaitForSecondsRealtime(1.15f);

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
        onDownloadStatusChange.Invoke(DownloadState.notDownloading);

        yield return File.WriteAllTextAsync(Global.GameVersionPath, Preload.GameVersion.ToString());

        downloadingPanel.SetActive(false);
        playPanel.SetActive(true);

        yield return CheckBuild();

        manager.Check(false);

        File.Delete(Global.TempZipPath);
    }
}