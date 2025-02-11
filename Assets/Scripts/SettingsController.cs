using UnityEngine;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private SettingsToggleButtons skipIntro;

    private void Awake()
    {
        skipIntro.IsChecked = PrefsManager.skipIntro;
        skipIntro.Clicked += delegate (object sender, ToggleButtonEventArgs e)
        {
            PrefsManager.skipIntro = e.IsChecked;
        };
    }
}
