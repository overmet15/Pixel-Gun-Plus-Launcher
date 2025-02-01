using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public static class Chache
{
    public static async Task ChachePreviewImages(int count)
    {
        if (!Directory.Exists(Global.PreviewImagesChachePath))
            Directory.CreateDirectory(Global.PreviewImagesChachePath);

        List<string> files = Directory.GetFiles(Global.PreviewImagesChachePath, "*.png").ToList();

        Preload.previewImages = new Texture2D[count > 0 ? count : files.Count];

        for (int i = 0; i < files.Count; i++)
        {
            try
            {
                byte[] bytes = await File.ReadAllBytesAsync(files[i]);

                Texture2D tex = new(2, 2);
                tex.LoadImage(bytes, false);
                Preload.previewImages[i] = tex;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Couldn't parse file {files[i]}: {ex.Message}");
            }
        }

        for (int i = 1; i <= count; i++)
        {
            if (i <= files.Count) continue;

            UnityWebRequest request = UnityWebRequestTexture.GetTexture($"{Global.previewImagesLink}/{i}");
            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogWarning($"Failed to download texture {i}: {request.error}");
                continue;
            }

            Preload.previewImages[i - 1] = DownloadHandlerTexture.GetContent(request);

            byte[] bytes = Preload.previewImages[i-1].EncodeToPNG();
            
            using (FileStream stream = File.Create($"{Global.PreviewImagesChachePath}/{i}.png"))
            {
                await stream.WriteAsync(bytes, 0, bytes.Length);
            }
        }

        foreach (var v in Preload.previewImages)
        {
            Debug.Log(v == null);
        }
    }

}
