using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using YG;

public class SoundSetting : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private SoundType _setting;

    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        switch (_setting)
        {
            case SoundType.Master:
            {
                slider.value = YandexGame.savesData.MasterVolume;
                break;
            }
            case SoundType.UI:
            {
                slider.value = YandexGame.savesData.UIVolume;
                break;
            }
        }

        UpdateVolume();
    }

    public void UpdateVolume()
    {
        switch (_setting)
        {
            case SoundType.Master:
            {
                YandexGame.savesData.MasterVolume = slider.value;
                _mixer.SetFloat("Master_Volume",
                    YandexGame.savesData.MasterVolume);
                break;
            }
            case SoundType.UI:
            {
                YandexGame.savesData.UIVolume = slider.value;
                _mixer.SetFloat("UI_Volume",
                    YandexGame.savesData.UIVolume);
                break;
            }
        }

        YandexGame.SaveProgress();
        _text.text = (slider.value + 80).ToString();
    }
}

public enum SoundType
{
    Master = 0,
    UI = 1,
}
