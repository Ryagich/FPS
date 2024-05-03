using UnityEngine;
using YG;

public class UpgradesChecker : MonoBehaviour
{
    [SerializeField] private UpgradesController _upgradesController;
    [SerializeField] private WarningIcon _upgradeButtonWarningIcon;

    private void Awake()
    {
        if (YandexGame.SDKEnabled)
        {
            Init();
        }
        else
        {
            YandexGame.GetDataEvent += Init;
        }
    }

    private void Init()
    {
        _upgradesController.Init();
        CurrencyController.Instanse.CrystalsChanged += CheckPossibilityBuy;
        CheckPossibilityBuy();
    }

    private void CheckPossibilityBuy()
    {
        var have = false;
        foreach (var upgrade in _upgradesController.Upgrades)
        {
            var activity =  upgrade.GetNextLevelIndex() is not -1
                           && upgrade.CheckLastUpgrades()
                           && YandexGame.savesData.UpgradesLevel >= upgrade.MinLevel
                           && YandexGame.savesData.Crystals >= upgrade.GetNextLevel().Cost;
            upgrade.WarningIcon.gameObject.SetActive(activity);
            if (activity)
            {
                have = true;
            }
        }

        _upgradeButtonWarningIcon.gameObject.SetActive(have);
    }
}