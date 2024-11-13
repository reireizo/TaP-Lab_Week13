using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeatherManager : MonoBehaviour 
{
    [SerializeField]
    private string cityName = "Orlando";
    private string jsonApi;

    void Start()
    {
        jsonApi = "http://api.openweathermap.org/data/2.5/weather?q=" + cityName + ",us&mode=json&appid=550a6618486c8002a8c4f81bf99ca84e";
        Debug.Log(jsonApi);
    }

    private IEnumerator CallAPI(string url, Action<string> callback)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError($"network problem: {request.error}");
            }
            else if (request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"response error: {request.responseCode}");
            }
            else
            {
                callback(request.downloadHandler.text);
            }
        }
    }

    public IEnumerator GetWeatherXML(Action<string> callback)
    {
        return CallAPI(jsonApi, callback);
    }

}
