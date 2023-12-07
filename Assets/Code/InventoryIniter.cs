using System;
using System.Collections;
using System.Collections.Generic;
using InfimaGames.LowPolyShooterPack;
using UnityEngine;
using YG;

public class InventoryIniter : MonoBehaviour
{
    [field:SerializeField]  public Inventory _inventory { get; private set; }
    [SerializeField] private Character _character;

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
        var weapons = _inventory.InitWeapons();
        var index = 0;

        for (int i = 0; i < YandexGame.savesData.ChosenWeapons.Length; i++)
        for (var j = 0; j < YandexGame.savesData.ChosenWeapons[i].Length; j++)
        {
            if (!YandexGame.savesData.ChosenWeapons[i][j])
            {
                Destroy(weapons[index].gameObject);
                weapons[index] = null;
            }

            index++;
        }

        var newWeapons = new List<WeaponBehaviour>();
        foreach (var weapon in weapons)
        {
            if (weapon)
                newWeapons.Add(weapon);
        }

        InitAttachments(newWeapons.ToArray());

        _inventory.weapons = newWeapons.ToArray();
        _inventory.Init();
        _character.RefreshWeaponSetup();

        YandexGame.GetDataEvent -= Init;
    }

    private void InitAttachments(WeaponBehaviour[] weapons)
    {
        foreach (var weapon in weapons)
        {
            var manager = weapon.GetComponent<InventoryWeapon>();
            manager.Init();
            manager.ChosenCurrentAttachments();
        }
    }
}