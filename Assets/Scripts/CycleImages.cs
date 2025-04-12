using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleImages : MonoBehaviour
{
    private int currentIndex;

    public MyCenterOnChild center;

    public GameObject previewObject;

    private List<GameObject> imageList = new List<GameObject>();

    private float time = 5f;

    private float elapsed = 0f;

    private IEnumerator Start()
    {
        if (Preload.previewImages == null || Preload.previewImages.Length == 0)
        {
            gameObject.SetActive(false);
            yield break;
        }

        for (int i = 0; i < Preload.previewImages.Length; i++)
        {
            AddImage(i);
        }
    }

    public void ChangeIndex(int index)
    {
        currentIndex = index;
        elapsed = 0f;
    }

    private void AddImage(int index)
    {
        GameObject obj = Instantiate(previewObject);
        obj.transform.parent = center.transform;
        obj.transform.localScale = new Vector3(1f, 1f, 1f);
        obj.GetComponent<PreviewImage>().currentIndex = index;
        imageList.Add(obj);
        obj.SetActive(true);
    }

    private void Update()
    {
        if (elapsed < time)
        {
            elapsed += Time.deltaTime;
        }
        else
        {
            elapsed = 0f;
            currentIndex++;
            if (currentIndex > imageList.Count - 1) currentIndex = 0;
            center.CenterOn(imageList[currentIndex].transform);
        }
    }
}