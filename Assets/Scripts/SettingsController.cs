using UnityEngine;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private SettingsToggleButtons skipIntro;
    [SerializeField] private SettingsToggleButtons closeOnPlay;

    private void Awake()
    {
        skipIntro.IsChecked = PrefsManager.skipIntro;
        skipIntro.Clicked += delegate (object sender, ToggleButtonEventArgs e)
        {
            PrefsManager.skipIntro = e.IsChecked;
        };
        closeOnPlay.IsChecked = PrefsManager.closeOnPlay;
        closeOnPlay.Clicked += delegate (object sender, ToggleButtonEventArgs e)
        {
            PrefsManager.closeOnPlay = e.IsChecked;
        };
    }
}
