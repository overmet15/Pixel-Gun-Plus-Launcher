using UnityEngine;

public class NewsItem : MonoBehaviour
{
    public NewsData data;
    [SerializeField] private UILabel header, shortDesc, date;
    [SerializeField] private UIBasicSprite previewPic;

    public void UpdateDisplay()
    {
        header.text = data.shortHeader;
        shortDesc.text = data.shortDescription;
        date.text = $"{data.date.Day}.{data.date.Month}.{data.date.Year}";
        previewPic.mainTexture = data.previewPicture;
    }

    public void OnPressed()
    {
        // Logic here
    }
}
