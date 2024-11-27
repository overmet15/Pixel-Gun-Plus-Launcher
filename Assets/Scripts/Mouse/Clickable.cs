using UnityEngine;
using UnityEngine.EventSystems;

public class Clickable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (enabled)
        Mouse.clickablesOn.Add(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Mouse.clickablesOn.Contains(this))
        Mouse.clickablesOn.Remove(this);
    }

    private void OnDisable()
    {
        if (Mouse.clickablesOn.Contains(this)) Mouse.clickablesOn.Remove(this);
    }
}
