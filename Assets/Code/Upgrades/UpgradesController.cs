using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class UpgradesController : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private TMP_Text _level;
    [SerializeField] private Button _upgradeButton;
    [SerializeField] private Transform _content;
    [Space] [SerializeField] private string _levelText = "Current level:";
    [Space, Header("Debug SerializeField")] [SerializeField]
    private List<Upgrade> _upgrades = new();

    [SerializeField] private Upgrade _upgrade;

    private void Awake()
    {
        _upgrades = _content.GetComponentsInChildren<Upgrade>().ToList();

        if (YandexGame.SDKEnabled)
        {
            LoadSaves();
        }
        else
        {
            YandexGame.GetDataEvent += LoadSaves;
        }
    }

    private int GetBranchIndex(Upgrade upgrade)
    {
        return upgrade.Type switch
        {
            UpgradeType.Expedition => 0,
            UpgradeType.Battle => 1,
            UpgradeType.Skill => 2,
            UpgradeType.Survival => 3,
            UpgradeType.Weapon => 4,
            UpgradeType.Hero => 5,
            _ => -1
        };
    }

    private void LoadSaves()
    {
        foreach (var u in _upgrades)
        {
            var branch = GetBranchIndex(u);
                
            for (var i = 0; i <= YandexGame.savesData.Upgrades[branch][u.Level][u.Index]; i++)
            {
                u.Info[i].Open();
            }
            u.Button.onClick.AddListener(() => OnClick(u));
            SetUpgradeActivity(u);
            u.UpdateText();
        }

        UpdateLevel();
    }

    private void OnClick(Upgrade upgrade)
    {
        _upgrade = upgrade;
        UpdateInterface();
    }

    private void UpdateInterface()
    {
        var info = _upgrade.GetNextLevel() ?? _upgrade.GetCurrentLevel();

        UpdateDescription(info);
        UpdateUpgradeButton(info);
    }

    [Button]
    private void UpdateLevel()
    {
        YandexGame.savesData.UpgradesLevel++;
        YandexGame.SaveProgress();

        _level.text = $"{_levelText} {YandexGame.savesData.UpgradesLevel.ToString()}";
    }

    private void Upgrade(UpgradeInfo info)
    {
        _upgradeButton.onClick.RemoveAllListeners();
        CurrencyController.Instanse.ChangeAmount(CurrencyType.Crystals, info.Cost);

        info.Open();
        UpdateLevel();
        _upgrade.UpdateText();
        UpdateInterface();
        UpdateUpgrades();

        YandexGame.savesData.Upgrades[GetBranchIndex(_upgrade)][_upgrade.Level][_upgrade.Index] = _upgrade.GetCurrentLevelIndex();
        YandexGame.SaveProgress();
//        Debug.Log(_upgrade.GetCurrentLevelIndex());
    }

    private void UpdateDescription(UpgradeInfo info)
    {
        _name.text = _upgrade.Name == "" ? _upgrade.name : _upgrade.Name;
        _description.text = info.Description == "" ? _upgrade.name : info.Description;
    }

    private void UpdateUpgrades()
    {
        foreach (var upgrade in _upgrades)
        {
            SetUpgradeActivity(upgrade);
        }
    }

    private void SetUpgradeActivity(Upgrade upgrade)
    {
        var activity = upgrade.CheckLastUpgrades() && upgrade.MinLevel <= YandexGame.savesData.UpgradesLevel;
        upgrade.Button.interactable = activity;
        upgrade.Trigger.enabled = activity;
    }

    private void UpdateUpgradeButton(UpgradeInfo info)
    {
        _upgradeButton.interactable = !info.Opened
                                      && _upgrade.CheckLastUpgrades()
                                      && YandexGame.savesData.UpgradesLevel >= _upgrade.MinLevel
                                      && YandexGame.savesData.Crystals >= info.Cost;
        if (!_upgradeButton.interactable)
            return;
        _upgradeButton.onClick.AddListener(() => Upgrade(info));
    }
}