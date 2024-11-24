using UnityEngine;

[CreateAssetMenu(fileName = "Theme", menuName = "Theme")]
public class Theme : ScriptableObject
{
    public Color topBarColor;
    public Color progressBarColor;

    public Sprite backgroundSprite;
    public Sprite logoSprite;
}
