using UnityEngine;
using UnityEngine.EventSystems;

public class DragWindow : MonoBehaviour, IDragHandler
{
    private Vector2 _deltaValue = Vector2.zero;

    public void OnDrag(PointerEventData data)
    {
#if !UNITY_EDITOR && UNITY_STANDALONE
        _deltaValue += data.delta;
        BorderlessWindow.MoveWindowPos(_deltaValue, Screen.width, Screen.height);
#else
        Debug.Log($"Dragging Window: {data.delta}");
#endif
    }
}