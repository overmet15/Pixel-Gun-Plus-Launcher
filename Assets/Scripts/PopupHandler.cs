using System;
using UnityEngine;

public class PopupHandler : MonoBehaviour
{
    [SerializeField] private GameObject state1, state2;

    [NonSerialized]
    public string path = Global.DefaultGameFolderPath;

    private void HideSelf()
    {
        gameObject.SetActive(false);
    }

    public void ShowPopup()
    {
        state2.SetActive(true);
    }

    public void CopyFolder()
    {
        SettingsManager.ChangeGamePath(path, true);
        HideSelf();
    }

    public void MoveFolder()
    {
        SettingsManager.ChangeGamePath(path, true);
        HideSelf();
    }

    public void DeleteOld()
    {
        SettingsManager.ChangeGamePath(path, false, true);
        HideSelf();
    }

    public void IgnoreOld()
    {
        SettingsManager.ChangeGamePath(path, false, false, true);
        HideSelf();
    }

    public void OpenNewPath()
    {
        SettingsManager.OpenPath(path);
    }

    public void OpenPrevPath()
    {
        SettingsManager.OpenPath(PrefsManager.gamePath);
    }
}
