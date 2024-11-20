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
    [SerializeField]private string weatherType;
    [SerializeField]private int visibility;
    private WeatherInfo weatherInfo;

    public void Start()
    {
        jsonApi = "http://api.openweathermap.org/data/2.5/weather?q=" + cityName + ",us&mode=json&appid=550a6618486c8002a8c4f81bf99ca84e";
        StartCoroutine(GetWeatherJSON(OnJSONDataLoaded));
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

    public IEnumerator GetWeatherJSON(Action<string> callback)
    {
        return CallAPI(jsonApi, callback);
    }

    public void OnJSONDataLoaded(string data)
    {
        weatherInfo = JsonUtility.FromJson<WeatherInfo>(data);
        Debug.Log(data);
        Debug.Log(weatherInfo.weather[0].main + " " + weatherInfo.visibility);
        weatherType = weatherInfo.weather[0].main;
        visibility = weatherInfo.visibility;
    }

}

[Serializable]
public class WeatherInfo
{
    public Weather[] weather;
    public int visibility;
}

[Serializable]
public class Weather
{
    public string main;
}
