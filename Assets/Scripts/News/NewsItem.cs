using UnityEngine;

public class NewsItem : MonoBehaviour
{
    public NewsData data;
    [SerializeField] private UILabel header, shortDesc, date;
    [SerializeField] private UIBasicSprite previewPic;
    [SerializeField] private GameObject isNewIndicator;

    public void UpdateDisplay()
    {
        header.text = data.shortHeader;
        shortDesc.text = data.shortDescription;
        date.text = $"{data.date.Day}.{data.date.Month}.{data.date.Year}";
        previewPic.mainTexture = data.previewPicture;
        isNewIndicator.SetActive(data.isNew);
    }

    public void OnPressed()
    {
        // Logic here
    }
}
