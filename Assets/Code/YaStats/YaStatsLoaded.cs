using System.Collections.Generic;
using UnityEngine;
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
        var eventParams = new Dictionary<string, string>
        {
            { "loaded", "loaded" }
        };
        YandexMetrica.Send("loaded", eventParams);
    }
}
