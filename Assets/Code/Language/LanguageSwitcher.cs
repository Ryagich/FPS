using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using YG;
using System.Collections;

public class LanguageSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private List<GameObject> _mainCanvas;

    private void Start()
    {
        if (YandexGame.SDKEnabled)
        {
            StartCoroutine(WaitSomeTimes());
            Debug.Log("SDKEnabled");
        }
        else
        {
            YandexGame.GetDataEvent += () => StartCoroutine(WaitSomeTimes());
            Debug.Log("GetDataEvent");
        }
    }

    private IEnumerator WaitSomeTimes()
    {
        yield return new WaitForSeconds(1.5f);
        CheckLanguage();
    }

    private void CheckLanguage()
    {
        if (YandexGame.savesData.locale == "")
        {
            ShowStartPanel();
        }
        else
        {
            Init();
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
        LocalizationSettings.Instance.GetInitializationOperation();
        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
        {
            // Debug.Log($"{locale.Identifier.Code} == {YandexGame.EnvironmentData.language}");
            // Debug.Log(locale.Identifier.Code.Contains(YandexGame.EnvironmentData.language));
            if (locale.Identifier.Code.Contains(YandexGame.savesData.locale))
            {
                LocalizationSettings.SelectedLocale = locale;
                //Debug.Log($"Locale is {LocalizationSettings.SelectedLocale.Identifier.Code}");
            }
        }

        ShowMainCanvas();
    }
}