using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour
{

    [SerializeField] private UILabel mainText, verText;
    [SerializeField] private GameObject main, downloading, mainScreen, background;
    [SerializeField] private DownloadManager downloadManager;

    public GameObject unaviableScreen;

    public bool downloadingGame;

    private void Start()
    {
        Check(true);
    }

    public void Check(bool playAnim)
    {
        verText.text = string.Empty;

        downloading.SetActive(false);
        main.SetActive(true);

        if (Global.buildState != BuildState.noBuild && Global.buildState != BuildState.unknownBuild) verText.text = Global.localVersion.ToString();

        switch (Global.buildState)
        {
            case BuildState.noBuild: mainText.text = "DOWNLOAD"; break;
            case BuildState.unknownBuild: verText.text = "UNKNOWN VERSION"; mainText.text = "PLAY"; break;
            case BuildState.updateNeeded: mainText.text = "UPDATE"; break;
            case BuildState.upToDate: mainText.text = "PLAY"; break;
        }
    }

    // UGUI, WRONG!!! NGUI! things
    public void OnMainButton()
    {
        if (Global.buildState == BuildState.upToDate || Global.buildState == BuildState.unknownBuild) 
        {
            StartCoroutine(TryLaunch());
        }
        else downloadManager.Download();
    }

    IEnumerator TryLaunch()
    {
        yield return DownloadManager.CheckBuild();

        if (Global.buildState == BuildState.upToDate || Global.buildState == BuildState.unknownBuild) ProcessHandler.StartMonitoringProcess();
        else Check(false);
    }

    public void OnCreditsButton()
    {
    }

    public void OnSettingsButton()
    {
    }

    public void OnQuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
        //Application.CommitSuicide();
    }

    public void OnMinimizeButton()
    {
        BorderlessWindow.MinimizeWindow();
    }
}