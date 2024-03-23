using System;
using System.Collections;
using System.Collections.Generic;
using InfimaGames.LowPolyShooterPack;
using UnityEngine;
using UnityEngine.Serialization;
using YG;
using Random = UnityEngine.Random;

public class BrokenItem : MonoBehaviour
{
    [SerializeField, Min(0)] private int _maxAmmo = 15;
    [SerializeField, Min(0)] private float _armor = 10;
    [SerializeField, Min(0)] private float _health = 10;
    [SerializeField, Min(0)] private int _minCrystals = 1;
    [SerializeField, Min(0)] private int _maxCrystals = 15;

    [SerializeField] private GameObject _addSpellMesh;
    [SerializeField] private GameObject _ammoMesh;
    [SerializeField] private GameObject _armorMesh;
    [SerializeField] private GameObject _crystalMesh;
    [SerializeField] private GameObject _moneyMesh;
    [SerializeField] private GameObject _healthMesh;
    [SerializeField] private GameObject _grenadeMesh;

    private BrokenItemType type;

    public void Activate(BrokenItemType type)
    {
        this.type = type;
        switch (type)
        {
            case BrokenItemType.AddSpell:
                _addSpellMesh.SetActive(true);
                break;
            case BrokenItemType.Ammo:
                _ammoMesh.SetActive(true);
                break;
            case BrokenItemType.Armor:
                _armorMesh.SetActive(true);
                break;
            case BrokenItemType.Crystal:
                _crystalMesh.SetActive(true);
                break;
            case BrokenItemType.Money:
                _moneyMesh.SetActive(true);
                break;
            case BrokenItemType.Health:
                _healthMesh.SetActive(true);
                break;
            case BrokenItemType.Grenade:
                _grenadeMesh.SetActive(true);
                break;
            case BrokenItemType.Empty:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Use()
    {
        var character = Character.Instance;
        var callback = character.GetComponent<CallbackController>();
        var sc = StatsController.Instance;
        var movement = sc.GetComponent<Movement>();
        var talents = YandexGame.savesData.Talents;

        if (talents[2])
        {
            movement.ApplyAddedSpeedForItem();
        }

        switch (type)
        {
            case BrokenItemType.AddSpell:
                character.GetComponent<AddSpellController>().AddSpell(1);
                callback.AddCallBack(new CallbackInfo(CallbackTypes.AddSpell, "1"));
                break;
            case BrokenItemType.Ammo:
                var inventory = (character.GetInventory() as Inventory);
                var ammoCount = Random.Range(0, _maxAmmo);
                if (talents[6])
                {
                    sc.TakeArmor(2);
                    callback.AddCallBack(new CallbackInfo(CallbackTypes.Armor,"2"));
                }
                if (talents[13])
                {
                    var weapon = inventory.GetEquipped() as Weapon;
                    var type = weapon.AmmoType;
                    if (inventory.GetAmmo(type) == inventory.AmmunitionMax[(int)type])
                    {
                        inventory.TakeAmmo(type, ammoCount);
                        callback.AddCallBack(new CallbackInfo(CallbackTypes.Ammo, ammoCount.ToString()), type);
                    }
                    else
                    {
                        var t = (Ammo)Random.Range(0, 5);
                        inventory.TakeAmmo(t, ammoCount);
                        callback.AddCallBack(new CallbackInfo(CallbackTypes.Ammo, ammoCount.ToString()), t);
                    }
                }
                else
                {
                    var t = (Ammo)Random.Range(0, 5);
                    inventory.TakeAmmo(t, ammoCount);
                    callback.AddCallBack(new CallbackInfo(CallbackTypes.Ammo, ammoCount.ToString()), t);
                }
                break;
            case BrokenItemType.Armor:
                var armorCount = Random.Range(1, _armor);
                sc.TakeArmor(armorCount);
                callback.AddCallBack(new CallbackInfo(CallbackTypes.Armor,((int)armorCount).ToString()));
                break;
            case BrokenItemType.Crystal:
                var crystalCount = Random.Range(_minCrystals, _maxCrystals);
                CurrencyController.Instanse.Add(CurrencyType.Crystals, crystalCount);
                callback.AddCallBack(new CallbackInfo(CallbackTypes.Crystal,crystalCount.ToString()));
                if (talents[7])
                {
                    sc.TakeHealth(1);
                    callback.AddCallBack(new CallbackInfo(CallbackTypes.Health,"1"));
                }
                break;
            case BrokenItemType.Money:
                var moneyCount = Random.Range(_minCrystals, _maxCrystals);
                CurrencyController.Instanse.Add(CurrencyType.Money, moneyCount);
                callback.AddCallBack(new CallbackInfo(CallbackTypes.Money,moneyCount.ToString()));
                if (talents[7])
                {
                    sc.TakeHealth(1);
                    callback.AddCallBack(new CallbackInfo(CallbackTypes.Health,"1"));
                }
                break;
            case BrokenItemType.Health:
                var healthCount = Random.Range(1, _health);
                sc.TakeHealth(healthCount);
                callback.AddCallBack(new CallbackInfo(CallbackTypes.Health,((int)healthCount).ToString()));
                break;
            case BrokenItemType.Grenade:
                character.AddGrenade();
                callback.AddCallBack(new CallbackInfo(CallbackTypes.Grenade,"1"));
                break;
            case BrokenItemType.Empty:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

public enum BrokenItemType
{
    Empty,
    AddSpell,
    Ammo,
    Armor,
    Crystal,
    Money,
    Health,
    Grenade,
}