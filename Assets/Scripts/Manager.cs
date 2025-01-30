using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour
{

    [SerializeField] private UILabel mainText, verText;
    [SerializeField] private GameObject main, downloading, mainScreen, background;
    [SerializeField] private Animator canvasAnimator;
    [SerializeField] private DownloadManager downloadManager;

    public GameObject unaviableScreen;

    public bool downloadingGame;

    private string verDisp, playDisp; // temporary fix, something else is setting text for no reason.
    private void Start()
    {
        canvasAnimator.SetBool("UIEnabled", true);
        StartCoroutine(Check(true));
    }

    public IEnumerator Check(bool playAnim)
    {
        yield return null;

        verText.text = string.Empty;
        ProcessHandler.unaviableScreen = unaviableScreen;
        ProcessHandler.mainScreen = mainScreen;
        ProcessHandler.backgrnd = background;

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

        if (playAnim)
        {
            yield return new WaitForSeconds(1f); // finish anim

            canvasAnimator.SetTrigger("C");
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
        else
        {
            StartCoroutine(Check(false));
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
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
        //Application.CommitSuicide();
    }
}