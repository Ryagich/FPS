using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class GameStarter : MonoBehaviour
{
    [SerializeField] private Button _continueButton;

    public void StartNewGame()
    {
        YandexGame.savesData.Stage = 0;
        YandexGame.savesData.Money = 0;
        YandexGame.savesData.MainSpellMax = -1;
        YandexGame.savesData.MainSpellCount = -1;
        YandexGame.savesData.AddSpellMax = -1;
        YandexGame.savesData.AddSpellCount = -1;
        YandexGame.savesData.ArmorMax = 0;
        YandexGame.savesData.Armor = -1;
        YandexGame.savesData.HealthMax = 0;
        YandexGame.savesData.Health = -1;
        YandexGame.savesData.DamageFactor = .0f;
        YandexGame.savesData.AmmunitionMax = new[] { -1, -1, -1, -1, -1, -1, -1 };
        YandexGame.savesData.Ammunition = new[] { -1, -1, -1, -1, -1, -1, -1 };
        YandexGame.savesData.AddLive = 0;
        YandexGame.savesData.IsRun = false;
        YandexGame.savesData.FreePurchases = -1;
        YandexGame.savesData.Talents = new bool[65];

        YandexGame.savesData.speedWalking = -1f;
        YandexGame.savesData.speedAiming = -1f;
        YandexGame.savesData.speedCrouching = -1f;
        YandexGame.savesData.speedRunning = -1f;

        YandexGame.SaveProgress();
    }

    public void CheckContinueGame()
    {
        _continueButton.interactable = YandexGame.savesData.Stage > 0;
    }
}