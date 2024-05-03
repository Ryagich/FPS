using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    [field: SerializeField] public string Name { get; private set; }
    [field: SerializeField] public List<UpgradeInfo> Info { get; private set; }
    [field: SerializeField] public TMP_Text Text { get; private set; }
    [field: SerializeField] public Button Button { get; private set; }
    [field: SerializeField] public EventTrigger Trigger { get; private set; }
    [field: SerializeField] public List<Upgrade> LastUpgrades { get; private set; }
    [field: SerializeField] public int MinLevel { get; private set; }
    [field: SerializeField] public UpgradeType Type { get; private set; }
    [field: SerializeField, Range(0, 4)] public int Level { get; private set; }
    [field: SerializeField, Range(0, 2)] public int Index { get; private set; }
    [field: SerializeField] public WarningIcon WarningIcon { get; private set; }

    private void Awake()
    {
        Text.gameObject.SetActive(Info.Count > 0);
        UpdateText();
    }

    public void UpdateText()
    {
        Text.text = $"{GetCurrentLevelIndex() + 1}/{Info.Count}";
    }

    public bool CheckLastUpgrades()
    {
        foreach (var upgrade in LastUpgrades)
            if (!upgrade.Info[0].Opened)
                return false;
        return true;
    }

    public UpgradeInfo GetNextLevel()
    {
        return Info.FirstOrDefault(i => !i.Opened);
    }

    public int GetNextLevelIndex()
    {
        for (var i = 0; i < Info.Count; i++)
            if (!Info[i].Opened)
                return i;
        return -1;
    }

    public UpgradeInfo GetCurrentLevel()
    {
        for (var i = Info.Count - 1; i >= 0; i--)
            if (Info[i].Opened)
                return Info[i];
        return null;
    }

    public int GetCurrentLevelIndex()
    {
        for (var i = Info.Count - 1; i >= 0; i--)
            if (Info[i].Opened)
                return i;
        return -1;
    }
}

public enum UpgradeType
{
    Expedition,
    Battle,
    Skill,
    Survival,
    Weapon,
    Hero,
}