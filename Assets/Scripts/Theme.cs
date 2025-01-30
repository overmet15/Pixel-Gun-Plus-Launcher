using UnityEngine;

[CreateAssetMenu(fileName = "Theme", menuName = "Theme")]
public class Theme : ScriptableObject
{
    public Color topBarColor;
    public Color progressBarColor;

    public Texture2D backgroundImage;
    public Texture2D logoImage;
}
