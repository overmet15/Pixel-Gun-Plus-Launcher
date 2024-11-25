using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Toggle closeOnGameStart, debug;
    [SerializeField] private GameObject debugText;

    void Start()
    {
        closeOnGameStart.isOn = PrefsManager.closeOnGameStart;
        debug.isOn = PrefsManager.debugMode;
        
        debugText.SetActive(PrefsManager.debugMode);

        closeOnGameStart.onValueChanged.AddListener(OnCloseGameOnStartValueChanged);
        debug.onValueChanged.AddListener(OnDebugValueChanged);
    }

    void OnCloseGameOnStartValueChanged(bool val)
    {
        PrefsManager.closeOnGameStart = val;
    }

    void OnDebugValueChanged(bool val)
    {
        PrefsManager.debugMode = val;
        debugText.SetActive(val);
    }
}
