using System.Collections.Generic;
using UnityEngine;
using YG;

public class InventoryWeaponIniter : MonoBehaviour
{
    [SerializeField] private List<InventoryWeapon> _weapons = new();

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
        foreach (var weapon in _weapons)
        {
            weapon.Init();
            weapon.ChosenCurrentAttachments();
        }
    }
}
