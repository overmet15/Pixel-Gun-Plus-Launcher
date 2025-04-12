using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class FastTools : EditorWindow
{
    [MenuItem("Window/FastTools")]

    public static void ShowWindow()
    {
        GetWindow(typeof(FastTools));
    }

    void OnGUI()
    {
        EditorGUILayout.Space(3);
        if (GUILayout.Button("Load Preload"))
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene("Assets/Scenes/Preload.unity");
        }
        if (GUILayout.Button("Load Launcher"))
        {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene("Assets/Scenes/Launcher.unity");
        }
    }
}