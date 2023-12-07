using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using YG;

public class MoneyController : MonoBehaviour
{
    public static MoneyController Instanse { get; private set; }

    [SerializeField] private TMP_Text _text;
    
    private void Awake()
    {
        Instanse = this;
        UpdateText();
    }

    public bool Check(int value)
    {
        return YandexGame.savesData.Money >= value;
    }
    
    public void ChangeAmount(int value)
    {
        YandexGame.savesData.Money += value;
        YandexGame.SaveProgress();
        UpdateText();
    }

    private void UpdateText()
    {
        _text.text = YandexGame.savesData.Money.ToString() +'$';
    }
}