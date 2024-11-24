using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Security.Cryptography;

public class Manager : MonoBehaviour
{

    [SerializeField] private Text mainText, verText;
    [SerializeField] private GameObject main, downloading, mainScreen, background;
    [SerializeField] private Animator canvasAnimator;
    [SerializeField] private DownloadManager downloadManager;

    public GameObject unaviableScreen;

    public bool downloadingGame;

    public void Start()
    {
        canvasAnimator.SetBool("UIEnabled", true);
        StartCoroutine(Check(true));
    }

    public IEnumerator Check(bool playAnim)
    {
        verText.text = string.Empty;
        ProcessHandler.unaviableScreen = unaviableScreen;
        ProcessHandler.mainScreen = mainScreen;
        ProcessHandler.backgrnd = background;

        downloading.SetActive(false);
        main.SetActive(true);

        if (Global.buildState == BuildState.updateNeeded || Global.buildState == BuildState.upToDate) verText.text = $"{Preload.GameVersion.major}.{Preload.GameVersion.minor}.{Preload.GameVersion.patch}";

        switch (Global.buildState)
        {
            case BuildState.noBuild: mainText.text = "Download"; break;
            case BuildState.unknownBuild: verText.text = "Unknown Build"; mainText.text = "Play"; break;
            case BuildState.updateNeeded: mainText.text = "Update"; break;
            case BuildState.upToDate: mainText.text = "Play"; break;
        }

        if (playAnim)
        {
            yield return new WaitForSeconds(1f); // finish anim

            canvasAnimator.SetTrigger("C");
        }
    }

    // UGUI things
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
}

public static class Utils
{
    public static Color ColorToUColor(int r, int g, int b, int a = 255)
    {   
        Color color = new Color(r / 255f, g / 255f, b / 255f, a / 255f);
        return color;
    }
}