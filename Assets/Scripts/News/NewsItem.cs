using UnityEngine;

public class NewsItem : MonoBehaviour
{
    public NewsData data;
    [SerializeField] private UILabel header, shortDesc, date;

    public void UpdateDisplay()
    {
        header.text = data.shortHeader;
        shortDesc.text = data.shortDescription;
        date.text = $"{data.date.Day}.{data.date.Month}.{data.date.Year}";
    }

    public void OnPressed()
    {
        // Logic here
    }
}
