using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PreviewImage : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Image myImage;
    void Start()
    {
        if (Preload.previewImage == null) 
        {
            image.enabled = myImage.enabled = false;
        }
        else image.sprite = Preload.previewImage;
        //StartCoroutine(Loop(0));
    }

    /*IEnumerator Loop(int current) - I need to optimize smth first
    {
        if (current > Preload.previewImages.Count) current = 0;

        image.sprite = Preload.previewImages[current];
        yield return new WaitForSecondsRealtime(15);
        StartCoroutine(Loop(current));
    }*/
}
