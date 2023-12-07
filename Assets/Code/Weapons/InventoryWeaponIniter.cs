using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InventoryWeaponIniter : MonoBehaviour
{
    [SerializeField] private List<InventoryWeapon> _weapons = new();

    private void Awake()
    {
        foreach (var weapon in _weapons)
        {
            weapon.Init();
            weapon.ChosenCurrentAttachments();
        }
    }
}
