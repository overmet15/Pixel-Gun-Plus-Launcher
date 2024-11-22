using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PreviewImage : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Image myImage;
    private string link => $"https://pixelgun.plus/~1031/Screenshots/{Random.Range(1, 3)}.png";
    IEnumerator Start()
    {
        image.enabled = myImage.enabled = false;
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(link);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error downloading texture: " + request.error);
            yield break;
        }

        Texture2D texture = DownloadHandlerTexture.GetContent(request);

        // Create a sprite from the texture
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        image.sprite = sprite;
        image.enabled = myImage.enabled = true;
    }
}
