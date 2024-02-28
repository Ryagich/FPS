using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using YG;
using System.Collections;

public class LanguageSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private List<GameObject> _mainCanvas;
    [SerializeField] private float _initialDelay = .0f;

    private void Awake()
    {
        LocalizationSettings.Instance.GetInitializationOperation();
    }

    private void Start()
    {
        if (YandexGame.SDKEnabled)
        {
            CheckLanguage();
        }
        else
        {
            YandexGame.GetDataEvent += CheckLanguage;
//            Debug.Log("GetDataEvent");
        }
    }

    private void CheckLanguage()
    {
        if (YandexGame.savesData.locale == "")
        {
            ShowStartPanel();
        }
        else
        {
            ShowMainCanvas();
            StartCoroutine(WaitSomeTimes());
        }
    }

    private void ShowStartPanel()
    {
        _startPanel.SetActive(true);
    }

    private void ShowMainCanvas()
    {
        foreach (var element in _mainCanvas)
        {
            element.SetActive(true);
        }
    }

    public void SwitchLanguage(string code)
    {
        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
        {
            if (locale.Identifier.Code.Contains(code))
            {
                LocalizationSettings.SelectedLocale = locale;
            }
        }

        YandexGame.savesData.locale = code;
        YandexGame.SaveProgress();
    }

    private void Init()
    {
        // Debug.Log(YandexGame.savesData.locale);

        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
        {
            if (locale.Identifier.Code.Contains(YandexGame.savesData.locale))
            {
                LocalizationSettings.SelectedLocale = locale;
            }
        }
    }
    
    private IEnumerator WaitSomeTimes()
    {
        yield return new WaitForSeconds(_initialDelay);
        Init();
    }
}