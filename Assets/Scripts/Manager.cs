using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Manager : MonoBehaviour
{

    [SerializeField] private Text mainText, verText;
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

    // ChatGPT ahh tool
    public static string ParseNGUIString(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        // Regex to find [RRGGBB]
        string pattern = @"\[(\w{6})\]";
        string replacement = "<color=#$1>";

        // Count the number of color codes [RRGGBB]
        int colorCount = Regex.Matches(input, pattern).Count;

        // Replace [RRGGBB] with <color=#RRGGBB>
        string result = Regex.Replace(input, pattern, replacement);

        // Replace [-] with </color>
        result = result.Replace("[-]", "</color>");

        // Count the number of closing </color> tags
        int closeTagCount = (result.Length - result.Replace("</color>", "").Length) / "</color>".Length;

        // If there are more color opening tags than closing, append necessary </color> tags
        if (colorCount > closeTagCount)
        {
            int missingCloseTags = colorCount - closeTagCount;
            result += new string(' ', missingCloseTags).Replace(" ", "</color>");
        }

        return result;
    }
}