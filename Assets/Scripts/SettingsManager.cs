using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    void Start()
    {

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
        string resultPath;

        if (Directory.GetFiles(changeTo).Contains(Path.Combine(changeTo, Global.gameExecutableName))) resultPath = changeTo;
        else resultPath = Path.Combine(changeTo, Global.subDirName);

        if (Directory.Exists(PrefsManager.gamePath))
        {
            if (!Path.GetFullPath(changeTo).StartsWith(Path.GetFullPath(PrefsManager.gamePath), StringComparison.OrdinalIgnoreCase) || changeTo != PrefsManager.gamePath)
            {
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
            }
            else Debug.LogWarning("Cannot move into child directory of current directory or same directory.");
        }
        
        PrefsManager.gamePath = resultPath;

        Manager.instance.Check(true);
    }
}
