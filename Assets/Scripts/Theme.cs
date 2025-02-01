using UnityEngine;

[CreateAssetMenu(fileName = "Theme", menuName = "Theme")]
public class Theme : ScriptableObject
{
    public Color topBarColor;
    public Color progressBarColor;

    public Texture2D backgroundImage;
    public Texture2D logoImage;

    public static bool TryGet(string name, out Theme theme)
    {
        theme = Resources.Load<Theme>($"Themes/{name}");
        return theme != null;
    }
}
