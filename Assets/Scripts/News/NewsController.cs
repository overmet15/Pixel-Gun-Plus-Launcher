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
        UnityWebRequest request = UnityWebRequest.Get(Global.newsLink);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning("Couldnt get news");
            yield break;
        }

        JSONNode indexLocal = null;

        if (File.Exists(Global.NewsReadPath))
        {
            var t = File.ReadAllTextAsync(Global.NewsReadPath);
            yield return new WaitUntil(() => t.IsCompleted);

            indexLocal = JSON.Parse(t.Result);
        }

        JSONNode index = JSON.Parse(request.downloadHandler.text);

        foreach (JSONNode node in index)
        {
            NewsData data = new();

            long unixDate = node["date"];
            data.date = DateTimeOffset.FromUnixTimeSeconds(unixDate).UtcDateTime;
                
            data.url = node["URL"];

            var taskPrev = Cache.DownloadOrCache(node["previewpicture"], 
                    $"{Global.NewsPreviewPictureChachePath}/{Path.GetFileName(node["previewpicture"])}");

            var taskFull = Cache.DownloadOrCache(node["fullpicture"], 
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

            data.isNew = IsNew(indexLocal, data);

            NewsItem obj =  NGUITools.AddChild(spawnItemsIn.gameObject, ogItem).GetComponent<NewsItem>();
            obj.data = data;
            obj.UpdateDisplay();
            spawnItemsIn.Reposition();
            scrollView.ResetPosition();
        }

        if (!File.Exists(Global.NewsReadPath))
            using (File.CreateText(Global.NewsReadPath)) { } // Closes the fucking stream

        File.WriteAllText(Global.NewsReadPath, request.downloadHandler.text);
    }

    // Returns if list contains the item
    public bool IsNew(JSONNode list, NewsData newsItem)
    {
        if (list == null) return true;

        try
        {
            foreach (JSONNode n in list)
            {
                int passed = 0;

                if (n["short_header"]["English"] != newsItem.shortHeader) passed++;
                if (n["header"]["English"] != newsItem.header) passed++;

                if (n["short_description"]["English"] != newsItem.shortDescription) passed++;
                if (n["description"]["English"] != newsItem.description) passed++;

                if (n["category"]["English"] != newsItem.category) passed++;

                if (passed == 5) return true;
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Couldn't check if list contains the item" + ex);
        }

        return false;
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
    public bool isNew;
}