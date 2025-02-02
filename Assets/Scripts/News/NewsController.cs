using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using SimpleJSON;
using System;
using System.IO;
using System.Threading.Tasks;

public class NewsController : MonoBehaviour
{
    [SerializeField] private UIGrid spawnItemsIn;
    [SerializeField] private UIScrollView scrollView;
    [SerializeField] private GameObject ogItem;

    IEnumerator Start()
    {
        //yield break;

        UnityWebRequest request = UnityWebRequest.Get(Global.newsLink);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning("Couldnt get news");
            yield break;
        }

        JSONNode index = JSON.Parse(request.downloadHandler.text);

        foreach (JSONNode node in index)
        {
            NewsData data = new();

            long unixDate = node["date"];
            data.date = DateTimeOffset.FromUnixTimeSeconds(unixDate).UtcDateTime;
                
            data.url = node["URL"];

            var taskPrev = Chache.DownloadOrChache(node["previewpicture"], 
                    $"{Global.NewsPreviewPictureChachePath}/{Path.GetFileName(node["previewpicture"])}");

            var taskFull = Chache.DownloadOrChache(node["fullpicture"], 
                    $"{Global.NewsFullpictureChachePath}/{Path.GetFileName(node["fullpicture"])}");

            yield return new WaitUntil(() => taskPrev.IsCompleted);
            yield return new WaitUntil(() => taskFull.IsCompleted);

            data.previewPicture = taskPrev.Result;
            data.fullpicture = taskFull.Result;


            data.shortHeader = node["short_header"]["English"];
            data.header = node["header"]["English"];

            data.shortDescription = node["short_description"]["English"];
            data.description = node["description"]["English"];

            data.category = node["category"]["English"];

            //NewsItem obj = Instantiate(ogItem, spawnItemsIn.transform).GetComponent<NewsItem>();
            NewsItem obj =  NGUITools.AddChild(spawnItemsIn.gameObject, ogItem).GetComponent<NewsItem>();
            obj.data = data;
            obj.UpdateDisplay();
            spawnItemsIn.Reposition();
            scrollView.ResetPosition();
        }
    }
}

#if UNITY_EDITOR
[System.Serializable] // Serialize in editor
#endif
public struct NewsData
{
    public DateTime date;
    public string url;
    public string shortHeader, header, shortDescription, description, category;
    public Texture2D previewPicture, fullpicture;
}