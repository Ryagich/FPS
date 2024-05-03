using System.Collections;
using System.Collections.Generic;
using InfimaGames.LowPolyShooterPack;
using UnityEngine;
using YG;

public class CharacterSetter : MonoBehaviour
{
    // 0 - Rambo | 1 - Booth | 2 - Shadow
    // Rambo: Hp 210 | Armor 180 | Speed 0.8 | Ammo 1.7
    // Booth: Hp 150 | Armor 110 | Speed 1 | Ammo 1.2
    // Shadow: Hp 100 | Armor 80 | Speed 1.5 | Ammo 1

    private float hpFactor;

    public void SetCharacterValues(GameObject character)
    {
        var statsC = character.GetComponent<StatsController>();
        var inventory = character.GetComponent<InventoryIniter>()._inventory;

        var addHp = GetAddHp();
        var addArmor = GetAddArmor();
        if (YandexGame.savesData.IsRun)
        {
            statsC.Init(YandexGame.savesData.HealthMax, 0, YandexGame.savesData.Health,
                              YandexGame.savesData.ArmorMax, 0, YandexGame.savesData.Armor);
        }
        else
        {
            switch (YandexGame.savesData.CharacterIndex)
            {
                case 0:
                    statsC.Init(210 + addHp, 0, 210 + addHp,
                        180 + addArmor, 0, 180 + addArmor);
                    inventory.ApplyMaxAmmoFactor(1.7f);
                    inventory.ApplyAmmoFactor(1.7f);
                    break;
                case 1:
                    statsC.Init(150 + addHp, 0, 150 + addHp,
                        110 + addArmor, 0, 110 + addArmor);
                    inventory.ApplyMaxAmmoFactor(1.2f);
                    inventory.ApplyAmmoFactor(1.2f);
                    break;
                case 2:
                    statsC.Init(100 + addHp, 0, 100 + addHp,
                        80 + addArmor, 0, 80 + addArmor);
                    inventory.ApplyMaxAmmoFactor(1f);
                    inventory.ApplyAmmoFactor(1f);
                    break;
            }

            inventory.ApplyMaxAmmoFactor(GetAddAmmoFactor());
            inventory.ApplyAmmoFactor(GetAddAmmoFactor());
        }

        inventory.LoadSaves();
    }

    private float GetAddAmmoFactor()
    {
        return .05f * (1 + YandexGame.savesData.Upgrades[1][1][0]);
    }

    private int GetAddHp() =>
        YandexGame.savesData.Upgrades[3][0][0] == -1 ? 0 : 10 * YandexGame.savesData.Upgrades[3][0][0];

    private int GetAddArmor()
    {
        return YandexGame.savesData.Upgrades[3][2][1] switch
        {
            0 => 15,
            1 => 30,
            2 => 50,
            _ => 0
        };
    }
}