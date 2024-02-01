using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class YaStatsLoaded : MonoBehaviour
{
    private void Start()
    {
        if (YandexGame.SDKEnabled)
        {
            SendLoadedYaEvent();
        }
        else
        {
            YandexGame.GetDataEvent += SendLoadedYaEvent;
        }
    }

    private void SendLoadedYaEvent()
    {
        var triggerValue = $"Scene:{SceneManager.GetActiveScene().name} ";
        var eventParams = new Dictionary<string, string>
        {
            { "loaded", triggerValue }
        };
        YandexMetrica.Send("loaded", eventParams);
    }
}