/*using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using Rilisoft.MiniJson;

public class NewsController : MonoBehaviour
{
    IEnumerator Start()
    {
        UnityWebRequest request = UnityWebRequest.Get(Global.newsLink);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            // Show error
            Debug.Log("News request error:" + request.error);
            yield break;
        }

        List<object> response = Json.Deserialize(request.downloadHandler.text) as List<object>;
    }

    public class NewsData
    {
        public ulong date;
        public string URL;
        public string previewpicture;
        public string fullpicture;

        // Localized dicts
        public Dictionary<string, string> short_header;
        public Dictionary<string, string> header;
        public Dictionary<string, string> short_description;
        public Dictionary<string, string> description;
        public Dictionary<string, string> category;
        public float @fixed; //To not break???

        // Stupid news system
        public static NewsData Parse(Dictionary<string, object> dict)
        {
            NewsData result = new();

            result.date = ulong.Parse(dict["date"] as string);
            result.URL = dict["URL"] as string;
            result.previewpicture = dict["previewpicture"] as string;
            result.fullpicture = dict["fullpicture"] as string;

            result.short_header = dict["short_header"] as Dictionary<string, string>;
            result.header = dict["header"] as Dictionary<string, string>;
            result.short_description = dict["short_description"] as Dictionary<string, string>;
            result.description = dict["description"] as Dictionary<string, string>;
            result.category = dict["category"] as Dictionary<string, string>;
            result.@fixed = float.Parse(dict["fixed"] as string);

            return result;
        }
    }
}*/
