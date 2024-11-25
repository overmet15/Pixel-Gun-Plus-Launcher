using UnityEngine;
using UnityEngine.UI;

public class DebugText : MonoBehaviour
{
    [SerializeField] private Text text;
    void Update()
    {
        text.text = $"Launcher Ver: {Application.version}, Current Theme: {Preload.currentTheme}, FPS: {Mathf.RoundToInt(1 / Time.unscaledDeltaTime)}";
    }
}
