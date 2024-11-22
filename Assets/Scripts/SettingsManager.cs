using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private Toggle closeOnGameStart;

    void Start()
    {
        closeOnGameStart.isOn = PrefsManager.closeOnGameStart;
        closeOnGameStart.onValueChanged.AddListener(OnCloseGameOnStartValueChanged);
    }

    void OnCloseGameOnStartValueChanged(bool val)
    {
        PrefsManager.closeOnGameStart = val;
    }
}
