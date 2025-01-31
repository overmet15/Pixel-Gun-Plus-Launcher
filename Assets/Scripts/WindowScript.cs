using UnityEngine;
using UnityEngine.EventSystems;

public class WindowScript : MonoBehaviour//, IDragHandler
{
    public Vector2Int borderSize;
    bool isDown;

    /*private Vector2 _deltaValue = Vector2.zero;
    private bool _maximized;*/

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Input.mousePosition.y > Screen.height - 60 
            && Input.mousePosition.x < Screen.width - 100) isDown = true;

        if (isDown)
            BorderlessWindow.MoveWindowPos(Input.mousePositionDelta * 1.8f, Screen.width, Screen.height);

        if (Input.GetMouseButtonUp(0)) isDown = false;
    }

    /*public void OnBorderBtnClick()
    {
        if (BorderlessWindow.framed)
            return;

        BorderlessWindow.SetFramedWindow();        
        BorderlessWindow.MoveWindowPos(Vector2Int.zero, Screen.width + borderSize.x, Screen.height + borderSize.y); // Compensating the border offset.
    }

    public void OnNoBorderBtnClick()
    {
        if (!BorderlessWindow.framed)
            return;

        BorderlessWindow.SetFramelessWindow();
        BorderlessWindow.MoveWindowPos(Vector2Int.zero, Screen.width - borderSize.x, Screen.height - borderSize.y);
    }

    public void OnCloseBtnClick()
    {
        EventSystem.current.SetSelectedGameObject(null);
        Application.Quit();
    }

    public void OnMinimizeBtnClick()
    {
        EventSystem.current.SetSelectedGameObject(null);
        BorderlessWindow.MinimizeWindow();
    }

    public void OnMaximizeBtnClick()
    {
        EventSystem.current.SetSelectedGameObject(null);

        if (_maximized)
            BorderlessWindow.RestoreWindow();
        else
            BorderlessWindow.MaximizeWindow();

        _maximized = !_maximized;
    }

    public void OnDrag(PointerEventData data)
    {
        if (BorderlessWindow.framed)
            return;

        _deltaValue += data.delta;
        if (data.dragging)
        {
            BorderlessWindow.MoveWindowPos(_deltaValue, Screen.width, Screen.height);
        }
    }*/
}
