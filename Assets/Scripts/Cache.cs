using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class Cache
{
    public static Dictionary<string, Texture2D> textureCache = new();
    public static Dictionary<string, Task<Texture2D>> currentRequests = new();

    public static async Task CachePreviewImages(int count)
    {
        Preload.previewImages = new Texture2D[count];

        for (int i = 1; i <= count; i++)
        {
            Texture2D tex = await DownloadOrCache($"{Global.previewImagesLink}/{i}");
            Preload.previewImages[i - 1] = tex;
        }
    }

    public static async Task<Texture2D> DownloadOrCache(string url, FilterMode filterMode = FilterMode.Point)
    {
        string hash = Hash.ComputeSHA256(url);

        if (textureCache.TryGetValue(hash, out Texture2D val)) return val;

        if(!Directory.Exists(Global.CachePath))
            Directory.CreateDirectory(Global.CachePath);

        string hashPath = Path.Combine(Global.CachePath, hash);

        if (File.Exists(hashPath))
        {
            try
            {
                byte[] bytes = await File.ReadAllBytesAsync(hashPath);

                Texture2D tex = new(2, 2);
                tex.LoadImage(bytes, false);
                tex.filterMode = filterMode;

                textureCache.Add(hash, tex);
                return tex;
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"Could't load image from file even tho it exists?\n{ex}");
            }
        }

        if (currentRequests.TryGetValue(url, out Task<Texture2D> t))
        {
            await t;

            Debug.Log("Test");

            return t.Result;

        }
        else
        {
            Task<Texture2D> request = Download(url, filterMode);

            currentRequests.Add(url, request);

            await request;

            byte[] bytes = request.Result.EncodeToPNG();

            using (FileStream stream = File.Create(hashPath))
            {
                await stream.WriteAsync(bytes, 0, bytes.Length);
            }

            await Task.Yield();

            currentRequests.Remove(url);
        }

        return null;
    }

    static async Task<Texture2D> Download(string url, FilterMode filterMode)
    {
        using UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        await request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning($"Failed to download texture: {url}");
            return null;
        }

        byte[] bytesWeb = DownloadHandlerTexture.GetContent(request).EncodeToPNG();

        Texture2D texWeb = new(2, 2);
        texWeb.LoadImage(bytesWeb);
        texWeb.filterMode = filterMode;

        return texWeb;
    }
}
