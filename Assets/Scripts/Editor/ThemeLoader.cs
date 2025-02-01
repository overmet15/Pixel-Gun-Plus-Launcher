using UnityEditor;
using UnityEngine;

public class ThemeLoader : EditorWindow
{
    [MenuItem("Custom/Theme Loader")]
    static void Get() => GetWindow<ThemeLoader>();

    private Theme theme;
    void OnGUI()
    {
        theme = (Theme)EditorGUILayout.ObjectField(theme, typeof(Theme), false);

        if (theme != null)
        {
            if (GUILayout.Button("Set"))
            {
                if (Application.isPlaying) 
                {
                    Debug.LogWarning("Can't set theme outside of play mode");
                    return;
                }
                
                Preload.currentTheme = theme;

                FindAnyObjectByType<AfterPreload>().Awake();
            }
        }
    }
}
