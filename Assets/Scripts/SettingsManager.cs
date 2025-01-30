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

    void OnDebugValueChanged(bool val)
    {
        PrefsManager.debugMode = val;
        debugText.SetActive(val);
    }
}
