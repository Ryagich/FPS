using System;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using YG;

public class SoundsSetterFromSave : MonoBehaviour
{
    [SerializeField] private AudioMixer _mixer;

    private void Awake()
    {
        if (YandexGame.SDKEnabled)
        {
            SetVolume();
        }
        else
        {
            YandexGame.GetDataEvent += SetVolume;
        }
    }

    private void SetVolume()
    {
        _mixer.SetFloat("Master_Volume",
            YandexGame.savesData.MasterVolume);
        _mixer.SetFloat("UI_Volume",
            YandexGame.savesData.UIVolume);
    }
}