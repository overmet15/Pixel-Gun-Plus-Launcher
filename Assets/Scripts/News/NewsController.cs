using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using SimpleJSON;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

public class NewsController : MonoBehaviour
{
    [SerializeField] private UIGrid spawnItemsIn;
    [SerializeField] private UIScrollView scrollView;
    [SerializeField] private GameObject ogItem;

    public JSONNode currentNewsNode;
    private List<NewsItem> newsItems = new();

    IEnumerator Start()
    {
        currentNewsNode = new JSONArray();
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

        int curID = 0;
        foreach (JSONNode node in index)
        {
            NewsData data = new()
            {
                id = curID,
                url = node["URL"],
                shortHeader = node["short_header"]["English"],
                header = node["header"]["English"],
                shortDescription = node["short_description"]["English"],
                description = node["description"]["English"],
                category = node["category"]["English"]
            };

            long unixDate = node["date"];
            data.date = DateTimeOffset.FromUnixTimeSeconds(unixDate).UtcDateTime;

            var taskPrev = Cache.DownloadOrCache(node["previewpicture"], 
                    $"{Global.NewsPreviewPictureChachePath}/{Path.GetFileName(node["previewpicture"])}");

            var taskFull = Cache.DownloadOrCache(node["fullpicture"], 
                    $"{Global.NewsFullpictureChachePath}/{Path.GetFileName(node["fullpicture"])}");

            yield return new WaitUntil(() => taskPrev.IsCompleted);
            yield return new WaitUntil(() => taskFull.IsCompleted);

            data.previewPicture = taskPrev.Result;
            data.fullpicture = taskFull.Result;

            if (TryGetSame(indexLocal, data, out var v))
            {
                if (v == null) data.isNew = true;
                else if (v["isnew"] == null || v["isnew"] == true) data.isNew = true;
            }
            else data.isNew = true;

            NewsItem obj =  NGUITools.AddChild(spawnItemsIn.gameObject, ogItem).GetComponent<NewsItem>();
            obj.data = data;
            obj.UpdateDisplay();
            spawnItemsIn.Reposition();
            scrollView.ResetPosition();
            curID++;

            node["isnews"] = false;

            newsItems.Add(obj);
            currentNewsNode.Add(node);
        }

        if (!File.Exists(Global.NewsReadPath))
            using (File.CreateText(Global.NewsReadPath)) { } // Closes the fucking stream


    }

    // Returns if list contains the item
    public JSONNode TryGetSame(JSONNode list, NewsData newsItem, out JSONNode result)
    {
        result = null;

        if (list == null)
        {
            Debug.Log("The local news list was null, returning false");
            return false;
        }

        try
        {
            foreach (JSONNode n in list)
            {
                int passed = 0;

                if (n["short_header"]["English"] == newsItem.shortHeader) passed++;
                if (n["header"]["English"] == newsItem.header) passed++;

                if (n["short_description"]["English"] == newsItem.shortDescription) passed++;
                if (n["description"]["English"] == newsItem.description) passed++;

                if (n["category"]["English"] == newsItem.category) passed++;

                result = n;

                if (passed == 5) return true;
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Couldn't check if list contains the item" + ex);
        }

        return false;
    }


    public void MarkAsOld(int id)
    {
        currentNewsNode[id]["isnew"] = false;
        File.WriteAllText(Global.NewsReadPath, currentNewsNode.ToString());
        newsItems[id].data.isNew = false;
        newsItems[id].UpdateDisplay();
    }
}

#if UNITY_EDITOR
[System.Serializable] // Serialize in editor
#endif
public struct NewsData
{
    public int id;
    public DateTime date;
    public string url;
    public string shortHeader, header, shortDescription, description, category;
    public Texture2D previewPicture, fullpicture;
    public bool isNew;
}