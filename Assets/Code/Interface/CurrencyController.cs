using TMPro;
using UnityEngine;
using YG;

public class CurrencyController : MonoBehaviour
{
    public static CurrencyController Instanse;
    [SerializeField] private TMP_Text _money;
    [SerializeField] private TMP_Text _crystals;

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
    }

    public bool Check(CurrencyType type, int value)
    {
        switch (type)
        {
            case CurrencyType.Money:
                return YandexGame.savesData.Money >= value;
            case CurrencyType.Crystals:
                return YandexGame.savesData.Crystals >= value;
            default:
                return false;
        }
    }

    public void ChangeAmount(CurrencyType type, int value)
    {
        switch (type)
        {
            case CurrencyType.Money:
                YandexGame.savesData.Money += value;
                break;
            case CurrencyType.Crystals:
                YandexGame.savesData.Crystals += value;
                break;
        }

        YandexGame.SaveProgress();
        UpdateText(type);
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