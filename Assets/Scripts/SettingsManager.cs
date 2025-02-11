using System;
using System.IO;
using System.Linq;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static void OpenPath(string path)
    {
        Application.OpenURL("file://" + path);
    }

    public static void OpenGamePathPicking(bool deleteOld = false, bool openPopup = false)
    {
        string[] test = SFB.StandaloneFileBrowser.OpenFolderPanel("Select folder.", PrefsManager.gamePath, false);
        if (test.Length > 0) 
        {
            if (!openPopup)
            {
                ChangeGamePath(test[0], deleteOld);
            }
            else if (test[0] != PrefsManager.gamePath)
            {
                Manager.instance.popup.gameObject.SetActive(true);
                Manager.instance.popup.path = test[0];
                Manager.instance.popup.ShowPopup();
            }
        }
    }

    public static void ChangeGamePath(string changeTo, bool moveOld = false, bool deleteOld = false, bool ignoreOld = false)
    {
        string resultPath;

        if (Directory.GetFiles(changeTo).Contains(Path.Combine(changeTo, Global.gameExecutableName))) resultPath = changeTo;
        else resultPath = Path.Combine(changeTo, Global.subDirName);

        if (Directory.Exists(PrefsManager.gamePath) && !ignoreOld)
        {
            if (!Path.GetFullPath(changeTo).StartsWith(Path.GetFullPath(PrefsManager.gamePath), StringComparison.OrdinalIgnoreCase) || changeTo != PrefsManager.gamePath)
            {
                if (deleteOld) Directory.Delete(PrefsManager.gamePath);
                else if (moveOld) Directory.Move(PrefsManager.gamePath, resultPath);
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
