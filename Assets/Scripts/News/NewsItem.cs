using UnityEngine;

public class NewsItem : MonoBehaviour
{
    public NewsData data;
    public NewsController controller;
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
        if (!data.isNew)
        {
            GetComponent<UISprite>().color = new Color(0.353f, 0.353f, 0.353f);
            GetComponent<UIButton>().defaultColor = new Color(0.353f, 0.353f, 0.353f);
            GetComponent<UIButton>().hover = new Color(0.353f, 0.353f, 0.353f);
            GetComponent<UIButton>().pressed = new Color(0.353f, 0.353f, 0.353f);
        }
    }

    public void OnPressed()
    {
        controller.SetItem(this);
    }
}
