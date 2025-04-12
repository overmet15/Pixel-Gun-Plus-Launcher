using UnityEngine;

[CreateAssetMenu(fileName = "Theme", menuName = "Theme")]
public class Theme : ScriptableObject
{
    public Color topBarColor;
    public Color progressBarColor;
    public Color frameColor;

    public Texture2D backgroundImage;
    public Texture2D logoImage;

    [Header("Special Theme")]
    public bool isSpecial;
    public Texture2D noiseImage;
    public Color noiseColor = Color.white;
    public float scrollSpeed = 0.005f;
    public float rectSize = 0.15f;
    public Color buttonColor = Color.white;

    public static bool TryGet(string name, out Theme theme)
    {
        theme = Resources.Load<Theme>($"Themes/{name}");
        return theme != null;
    }
}
