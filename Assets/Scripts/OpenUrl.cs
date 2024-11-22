using UnityEngine;

public class OpenUrl : MonoBehaviour
{
    [SerializeField] private string url;
    public void Open()
    {
        Application.OpenURL(url);
    }    
}
