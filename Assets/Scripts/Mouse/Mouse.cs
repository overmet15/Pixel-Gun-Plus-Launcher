using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    public static List<Clickable> clickablesOn = new(); // To not mess up everything.

    public static bool showLoading;

    [SerializeField] private Texture2D defaultMouse, clickIdndicatorMouse;
    [SerializeField] private Texture2D[] loadingAnimation;

    private bool isClickableTex = false; // Dont change cursor every frame

    private float timeBeforeChangingSprite; // i dont wanna use Corotines
    private int currentSprite = 0;

    void Start()
    {
        Cursor.SetCursor(defaultMouse, Vector2.zero, CursorMode.Auto);
        isClickableTex = false;
    }

    void Update()
    {
        if (!Application.isFocused) return;

        if (!showLoading)
        {
            if (clickablesOn.Count == 0 && isClickableTex) { Cursor.SetCursor(defaultMouse, new(16f, 8f), CursorMode.Auto); isClickableTex = false; }
            else if (clickablesOn.Count != 0 && !isClickableTex) SetClickable();
        }
        else
        {
            if (clickablesOn.Count != 0 && !isClickableTex) SetClickable();
            else
            {
                if (timeBeforeChangingSprite <= 0)
                {
                    Cursor.SetCursor(loadingAnimation[currentSprite], new(32f, 32f), CursorMode.Auto);
                    currentSprite++;
                    if (currentSprite > loadingAnimation.Length) currentSprite = 0;
                    timeBeforeChangingSprite = 0.5f;
                }
                timeBeforeChangingSprite -= Time.unscaledDeltaTime;
                isClickableTex = false;
            }
        }
    }

    void SetClickable() 
    {
         Cursor.SetCursor(clickIdndicatorMouse, new(32f, 8f), CursorMode.Auto); isClickableTex = true;
    }
}
