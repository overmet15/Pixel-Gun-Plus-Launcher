using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using SimpleJSON;
using System;

public class NewsController : MonoBehaviour
{
    [SerializeField] private UIGrid spawnItemsIn;
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

        try
        {
            JSONNode index = JSON.Parse(request.downloadHandler.text);

            foreach (JSONNode node in index)
            {
                NewsData data = new();

                long unixDate = node["date"];
                data.date = DateTimeOffset.FromUnixTimeSeconds(unixDate).UtcDateTime;
                
                data.url = node["URL"];

                data.previewPicture = node["previewpicture"];
                data.fullpicture = node["fullpicture"];

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
            }
        }
        catch
        {
            Debug.LogWarning("Exception on news parsing");
            yield break;
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
    public string previewPicture, fullpicture;
    public string shortHeader, header, shortDescription, description, category;
}