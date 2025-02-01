using System.Collections;
using UnityEngine;

public class PreviewImage : MonoBehaviour
{
    [SerializeField] private UIBasicSprite image1, image2;

    IEnumerator Start()
    {
        if (Preload.previewImages == null || Preload.previewImages.Length == 0)
        {
            gameObject.SetActive(false);
            yield break;
        }

        image1.mainTexture = Preload.previewImages[0];
        image1.color = new(1, 1, 1, 1);
        image2.color = new(1, 1, 1, 0);

        int currentIndex = 0;

        float time = 1;
        float elapsed = 0;

        while (true)
        {
            currentIndex = currentIndex >= Preload.previewImages.Length ? 0 : currentIndex;

            image2.mainTexture = Preload.previewImages[currentIndex];

            while (elapsed < time)
            {
                elapsed += Time.deltaTime;
                image2.color = new(1, 1, 1, elapsed);
                yield return null;
            }

            elapsed = 0;
            image1.mainTexture = Preload.previewImages[currentIndex];
            image1.color = new(1, 1, 1, 1);
            image2.color = new(1, 1, 1, 0);

            currentIndex++;

            yield return new WaitForSeconds(10);
        }
    }
}
