using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour
{
    public static Manager instance { get; private set; }
    [SerializeField] private UILabel mainText;
    public UILabel verText;
    [SerializeField] private GameObject main, downloading, mainScreen, background, outdated;
    [SerializeField] private DownloadManager downloadManager;
    [SerializeField] private LauncherUI ui;

    public PopupHandler popup;

    public GameObject unaviableScreen;

    public GameObject splashScreen;

    public bool downloadingGame;

    private void Awake()
    {
        instance = this;
        splashScreen.SetActive(!PrefsManager.skipIntro);
    }

    private void Start()
    {
        Check(true);
    }

    public void Check(bool playAnim)
    {
        StartCoroutine(CheckCoroutine(playAnim));
    }

    IEnumerator CheckCoroutine(bool playAnim)
    {
        var task = DownloadManager.CheckBuild();

        while (!task.IsCompleted) yield return null;

        verText.text = "NOT INSTALLED";

        downloading.SetActive(false);
        main.SetActive(true);
        ui.manageGameInstalledPanel.SetActive(false);
        ui.manageGamePanel.SetActive(false);

        if (Global.buildState != BuildState.noBuild && Global.buildState != BuildState.unknownBuild) verText.text = Global.localVersion.ToString();
        switch (Global.buildState)
        {
            case BuildState.noBuild: mainText.text = "DOWNLOAD"; Global.installed = false; break;
            case BuildState.unknownBuild: verText.text = "UNKNOWN VERSION"; mainText.text = "PLAY"; Global.installed = true; break;
            case BuildState.updateNeeded: outdated.SetActive(true); mainText.text = "UPDATE"; Global.installed = true; break;
            case BuildState.upToDate: mainText.text = "PLAY"; Global.installed = true; break;
        }
    }

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
        ui.creditsPanel.SetActive(!ui.creditsPanel.activeSelf);
        ui.newsPanel.SetActive(false);
        ui.settingsPanel.SetActive(false);
    }

    public void OnNewsButton()
    {
        ui.newsPanel.SetActive(!ui.newsPanel.activeSelf);
        ui.creditsPanel.SetActive(false);
        ui.settingsPanel.SetActive(false);
    }

    public void OnSettingsButton()
    {
        ui.settingsPanel.SetActive(!ui.settingsPanel.activeSelf);
        ui.creditsPanel.SetActive(false);
        ui.newsPanel.SetActive(false);
    }

    public void OnManageButton()
    {
        if (Global.installed)
        {
            ui.manageGameInstalledPanel.SetActive(!ui.manageGameInstalledPanel.activeSelf);
        }
        else
        {
            ui.manageGamePanel.SetActive(!ui.manageGamePanel.activeSelf);
        }
    }

    public void OnOpenPathButton()
    {
        SettingsManager.OpenPath(PrefsManager.gamePath);
    }

    public void OnChangePathButton()
    {
        if (Global.installed)
        {
            SettingsManager.OpenGamePathPicking(true, true);
        }
        else
        {
            SettingsManager.OpenGamePathPicking(true, false);
        }
    }

    public void ResetLauncher()
    {
        PlayerPrefs.DeleteAll();
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