using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Toggle debug;
    [SerializeField] private GameObject debugText;

    void Start()
    {
        debug.isOn = PrefsManager.debugMode;
        
        debugText.SetActive(PrefsManager.debugMode);

        debug.onValueChanged.AddListener(OnDebugValueChanged);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            OpenGamePathPicking();
        }
    }

    public static void OpenGamePathPicking(bool deleteOld = false)
    {
        string[] test = SFB.StandaloneFileBrowser.OpenFolderPanel("Select folder.", PrefsManager.gamePath, false);
        if (test.Length > 0) ChangeGamePath(test[0], deleteOld);
    }

    public static void ChangeGamePath(string changeTo, bool deleteOld = false)
    {
        if (Path.GetFullPath(changeTo).StartsWith(Path.GetFullPath(PrefsManager.gamePath), StringComparison.OrdinalIgnoreCase) || changeTo == PrefsManager.gamePath)
        {
            Debug.LogWarning("Cannot move into child directory of current directory or same directory.");
            return;
        }

        string resultPath = Path.Combine(changeTo, Global.subDirName);
        if (!Directory.Exists(resultPath)) Directory.CreateDirectory(resultPath);

        if (deleteOld) Directory.Move(PrefsManager.gamePath, resultPath);
        else
        {

            foreach (string s in Directory.GetFiles(PrefsManager.gamePath))
            {
                File.Copy(s, Path.Combine(resultPath, Path.GetFileName(s)));
            }

            foreach (string s in Directory.GetDirectories(PrefsManager.gamePath))
            {
                Utils.CopyDirectory(s, Path.Combine(resultPath, Path.GetFileName(s)));
            }
        }
        
        PrefsManager.gamePath = resultPath;
    }

    void OnDebugValueChanged(bool val)
    {
        PrefsManager.debugMode = val;
        debugText.SetActive(val);
    }
}
