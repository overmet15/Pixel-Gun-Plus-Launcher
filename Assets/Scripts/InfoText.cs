using UnityEngine;
using UnityEngine.UI;

public class InfoText : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private Clickable clickable;
    [SerializeField] private DownloadManager downloadManager;

    void Start()
    {
        DownloadManager.onDownloadStatusChange.AddListener(OnDownloadStatusChange);
    }

    void Update()
    {
        if (!Application.isFocused || DownloadManager.currentDownloadState != DownloadState.notDownloading) return;

        if (Global.buildState == BuildState.unknownBuild)
        {
            text.text = "Click here to download latest game version.";
        }
        else if (Preload.newVersionAviable)
        {
            text.text = "Click here to download latest launcher update.";
        }

        clickable.enabled = ClickableShouldBeEnabled();
    }

    public void OnClick()
    {   
        if (Global.buildState == BuildState.unknownBuild)
        {
            downloadManager.Download();
        }
        else if (Preload.newVersionAviable)
        {
            Application.OpenURL("https://github.com/overmet15/Pixel-Gun-Plus-Launcher/releases/latest");
        }
    }

    void OnDownloadStatusChange(DownloadState state)
    {
        switch (state)
        {
            case DownloadState.inProcess: text.text = "Download in process, please wait.";  break;
            case DownloadState.finished: text.text = "Unpacking...";  break;
            
        }
    }

    bool ClickableShouldBeEnabled()
    {
        return (Global.buildState == BuildState.unknownBuild || Preload.newVersionAviable) && DownloadManager.currentDownloadState == DownloadState.notDownloading;
    }
}