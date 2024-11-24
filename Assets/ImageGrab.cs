using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ImageGrab : MonoBehaviour
{
    public const string webImage = "https://pbs.twimg.com/media/FYzmjR2XwAYuzQ6.jpg";
    Texture2D grabbedImage;

    public void Awake()
    {
        GetWebImage(OnWebImageLoad);
    }

    public IEnumerator DownloadImage(Action<Texture2D> callback)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(webImage);
        yield return request.SendWebRequest();
        callback(DownloadHandlerTexture.GetContent(request));
    }

    public void OnWebImageLoad(Texture2D image)
    {
        if (grabbedImage == null)
        {
            grabbedImage = image;
        }
        this.gameObject.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", grabbedImage);
    }

    public void GetWebImage(Action<Texture2D> callback)
    {
        if (grabbedImage == null)
        {
            StartCoroutine(DownloadImage(callback));
        }
        else
        {
            callback(grabbedImage);
        }
    }
}
