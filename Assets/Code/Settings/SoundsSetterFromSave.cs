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
        _mixer.SetFloat("Master_Volume", YandexGame.savesData.MasterVolume);
        _mixer.SetFloat("UI_Volume", YandexGame.savesData.UIVolume);
        _mixer.SetFloat("Effects_Volume", YandexGame.savesData.EffectsVolume);
        _mixer.SetFloat("Shooting_Volume", YandexGame.savesData.ShootingVolume);
        _mixer.SetFloat("Steps_Volume", YandexGame.savesData.StepsVolume);
        _mixer.SetFloat("Music_Volume", YandexGame.savesData.MusicVolume);
    }
}