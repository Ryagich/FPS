using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
using YG;

public class LanguageSwitcher : MonoBehaviour
{
    private void Start()
    {
        if (YandexGame.SDKEnabled)
        {
            StartCoroutine(WI());
            Debug.Log("YandexGame.SDKEnabled");
        }
        else
        {
            //YandexGame.GetDataEvent += Init;
            YandexGame.GetDataEvent += () => StartCoroutine(WI());
            Debug.Log("NOT YandexGame.SDKEnabled");
        }
    }

    private IEnumerator WI()
    {
        yield return new WaitForSeconds(2);
        Init();
    }
    private void Init()
    {
        LocalizationSettings.Instance.GetInitializationOperation();
        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
        {
//            Debug.Log($"{locale.Identifier.Code} == {YandexGame.EnvironmentData.language}");
  //          Debug.Log(locale.Identifier.Code.Contains(YandexGame.EnvironmentData.language));
            if (locale.Identifier.Code.Contains(YandexGame.EnvironmentData.language))
            {
                LocalizationSettings.SelectedLocale = locale;
                Debug.Log($"Locale is {LocalizationSettings.SelectedLocale.Identifier.Code}");
            }
        }
    }
}
