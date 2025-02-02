using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class Chache
{
    public static List<string> chachedPaths = new();

    public static async Task ChachePreviewImages(int count)
    {
        //if (!Directory.Exists(Global.PreviewImagesChachePath))
        //    Directory.CreateDirectory(Global.PreviewImagesChachePath);

        Preload.previewImages = new Texture2D[count];

        for (int i = 1; i <= count; i++)
        {
            Texture2D tex = await DownloadOrChache($"{Global.previewImagesLink}/{i}", $"{Global.PreviewImagesChachePath}/{i}.png");
            Preload.previewImages[i - 1] = tex;
        }
    }

    public static async Task<Texture2D> DownloadOrChache(string url, string path, FilterMode filterMode = FilterMode.Point)
    {
        if(!Directory.Exists(Path.GetDirectoryName(path)))
            Directory.CreateDirectory(Path.GetDirectoryName(path));
        
        chachedPaths.Add(path);

        if (File.Exists(path))
        {
            try
            {
                byte[] bytes = await File.ReadAllBytesAsync(path);

                Texture2D tex = new(2, 2);
                tex.LoadImage(bytes, false);

                return tex;
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"Could't load image from file even tho it exists?\n{ex}");
            }
        }

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        await request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning($"Failed to download texture: {url}");
            return null;
        }

        byte[] bytesWeb = DownloadHandlerTexture.GetContent(request).EncodeToPNG();

        using (FileStream stream = File.Create(path))
        {
            await stream.WriteAsync(bytesWeb, 0, bytesWeb.Length);
        }

        Texture2D texWeb = new(2, 2);
        texWeb.LoadImage(bytesWeb);
        texWeb.filterMode = filterMode;

        return texWeb;
    }
}
