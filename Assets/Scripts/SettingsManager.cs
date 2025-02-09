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
            string[] test = SFB.StandaloneFileBrowser.OpenFolderPanel("", PrefsManager.gamePath, false);
            if (test.Length > 0) ChangeGamePath(test[0]);
        }
    }

    public void ChangeGamePath(string changeTo)
    {
        if (Path.GetFullPath(changeTo).StartsWith(Path.GetFullPath(PrefsManager.gamePath), StringComparison.OrdinalIgnoreCase) || changeTo == PrefsManager.gamePath)
        {
            Debug.LogWarning("Cannot move into child directory of current directory");
            return;
        }

        if (File.Exists(Global.GameExecutablePath))
        {
            // ToDo: ask to move

            Directory.Move(PrefsManager.gamePath, Path.Combine(changeTo, Global.subDirName));
            /*foreach (string s in Directory.GetFiles(PrefsManager.gamePath))
            {
                Debug.LogWarning($"{changeTo}\\{Path.GetFileName(s)}");
                File.Move(s, $"{changeTo}\\{Path.GetFileName(s)}");
            }

            foreach (string s in Directory.GetDirectories(PrefsManager.gamePath))
            {
                Debug.LogWarning($"{changeTo}\\{Path.GetFileName(s)}");

                Directory.Move(s, $"{changeTo}\\{Path.GetFileName(s)}");
            }*/
        }

        PrefsManager.gamePath = Path.Combine(changeTo, Global.subDirName);
    }

    void OnDebugValueChanged(bool val)
    {
        PrefsManager.debugMode = val;
        debugText.SetActive(val);
    }
}
