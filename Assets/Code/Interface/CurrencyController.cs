using System;
using TMPro;
using UnityEngine;
using YG;

public class CurrencyController : MonoBehaviour
{
    public static CurrencyController Instanse;
    public Action CrystalsChanged ;
    [SerializeField] private TMP_Text _money;
    [SerializeField] private TMP_Text _crystals;
    [SerializeField] private bool isRun = false;

    private void Awake()
    {
        Instanse = this;
        if (YandexGame.SDKEnabled)
        {
            if (_money)
                UpdateText(CurrencyType.Money);
            if (_crystals)
                UpdateText(CurrencyType.Crystals);
        }
        else
        {
            if (_money)
                YandexGame.GetDataEvent += () => UpdateText(CurrencyType.Money);
            if (_crystals)
                YandexGame.GetDataEvent += () => UpdateText(CurrencyType.Crystals);
        }

        if (isRun && !YandexGame.savesData.IsRun)
        {
            Add(CurrencyType.Money, 100 * (1 + YandexGame.savesData.Upgrades[0][1][0]));
            YandexGame.savesData.IsRun = true;
           // YandexGame.SaveProgress();
        }
    }

    public bool Check(CurrencyType type, int value)
    {
        return type switch
        {
            CurrencyType.Money => YandexGame.savesData.Money >= value,
            CurrencyType.Crystals => YandexGame.savesData.Crystals >= value,
            _ => false
        };
    }

//Метод для игрового процесса, учитывающий улучшения и т.д.
    public void Add(CurrencyType type, int value)
    {
        var talents = YandexGame.savesData.Talents;
        if (talents[9])
        {
            value *= 2;
        }
        switch (type)
        {
            case CurrencyType.Money:
                YandexGame.savesData.Money += value;
                break;
            case CurrencyType.Crystals:
                YandexGame.savesData.Crystals += value;
                break;
        }

        //YandexGame.SaveProgress();
        UpdateText(type);
    }

    public void ChangeAmount(CurrencyType type, int value)
    {
        value += value * (1 + YandexGame.savesData.Upgrades[0][3][1]);
        switch (type)
        {
            case CurrencyType.Money:
                YandexGame.savesData.Money += value;
                break;
            case CurrencyType.Crystals:
                YandexGame.savesData.Crystals += value;
                CrystalsChanged?.Invoke();
                break;
        }

        YandexGame.SaveProgress();
        UpdateText(type);
    }

    public void Reward(int value)
    {
        YandexGame.savesData.Crystals += value;
        YandexGame.SaveProgress();
        
        UpdateText(CurrencyType.Crystals);

    }
    private void UpdateText(CurrencyType type)
    {
        switch (type)
        {
            case CurrencyType.Money:
                _money.text = YandexGame.savesData.Money + "$";
                break;
            case CurrencyType.Crystals:
                _crystals.text = YandexGame.savesData.Crystals.ToString();
                break;
        }
    }
}

public enum CurrencyType
{
    Money,
    Crystals,
}