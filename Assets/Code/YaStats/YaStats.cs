using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YG;

public class YaStats : MonoBehaviour
{
    [SerializeField] private string _statText;
    
    public void SendYaEvent(string eventText)
    {
        var triggerValue = $"Scene:{SceneManager.GetActiveScene().name} Stat:{_statText} Event:{eventText}";
        var eventParams = new Dictionary<string, string>
        {
            { "event", triggerValue }
        };
        YandexMetrica.Send("event", eventParams);
    }
}