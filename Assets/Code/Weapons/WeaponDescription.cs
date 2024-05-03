using System;
using System.Collections;
using System.Collections.Generic;
using InfimaGames.LowPolyShooterPack;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponDescription : MonoBehaviour
{
    [SerializeField] private GameObject _description;
    [SerializeField] private TMP_Text _ammoTypeText;
    [SerializeField] private TMP_Text _shootingTypeText;
    [SerializeField] private TMP_Text _movementFactorText;
    [SerializeField] private TMP_Text _damageText;
    [SerializeField] private TMP_Text _shootingSpeedText;
    [SerializeField] private TMP_Text _spreadText;
    [SerializeField] private TMP_Text _magazineText;

    [SerializeField] private string _ammoType;
    [SerializeField] private string _shootingType;
    [SerializeField] private string _movementFactor;
    [SerializeField] private string _damage;
    [SerializeField] private string _shootingSpeed;
    [SerializeField] private string _spread;
    [SerializeField] private string _magazine;

    [Space] [SerializeField] private string automatic = "automatic";
    [SerializeField] private string single = "single";

    public void Show(GameObject weaponObj)
    {
        var weapon = weaponObj.GetComponent<InventoryWeapon>().Weapon;

        _description.SetActive(true);
        var ammoType = "";
        switch (weapon.AmmoType)
        {
            case Ammo.Eleven:
                ammoType = $"11mm";
                break;
            case Ammo.Five:
                ammoType = $"5,56mm";
                break;
            case Ammo.Nine:
                ammoType = $"9mm";
                break;
            case Ammo.Seven:
                ammoType = $"7,62mm";
                break;
            case Ammo.Twelve:
                ammoType = $"12mm";
                break;
        }

        var shootingType = weapon.IsAutomatic() ? automatic : single;
        _ammoTypeText.text = $"{_ammoType} {ammoType}";
        _shootingTypeText.text = $"{_shootingType} {shootingType}";
        _movementFactorText.text = $"{_movementFactor} x{weapon.GetMultiplierMovementSpeed()}";
        _damageText.text = $"{_damage} {weapon.Damage}";
        _shootingSpeedText.text = $"{_shootingSpeed} {weapon.GetRateOfFire()}";
        _spreadText.text = $"{_spread} {weapon.Spread}";
        _magazineText.text = $"{_magazine} {weaponObj.GetComponent<WeaponAttachmentManager>().GetEquippedMagazine().GetAmmunitionTotal()}";
    }

    public void Hide()
    {
        _description.SetActive(false);
    }
}