using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Networking;

public class WeatherManager : MonoBehaviour 
{
    public Light sun;

    [SerializeField]
    private string cityName = "Orlando";
    private string jsonApi;
    [SerializeField]private string weatherType;
    private WeatherInfo weatherInfo;

    [SerializeField]
    private Material[] skyboxes;


    public void Awake()
    {
        jsonApi = "http://api.openweathermap.org/data/2.5/weather?q=" + cityName + ",us&mode=json&appid=550a6618486c8002a8c4f81bf99ca84e";
        StartCoroutine(GetWeatherJSON(OnJSONDataLoaded));
    }

    private void Start()
    {
        if (weatherType == "Rain")
        {
            RenderSettings.skybox = skyboxes[0];
            sun.color = new Color((159f/255f), (228f/255f), (214f/255f), 1);
        }
        else if (weatherType == "Clouds")
        {
            RenderSettings.skybox = skyboxes[1];
            sun.color = new Color((214f/255f), 1, (250f/255f), 1);
        }
        else if (weatherType == "Clear")
        {
            RenderSettings.skybox = skyboxes[2];
        }
        else if (weatherType == "Snow")
        {
            RenderSettings.skybox= skyboxes[3];
            sun.intensity = 3;
        }
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
