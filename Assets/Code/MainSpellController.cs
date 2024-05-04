using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using YG;

public class MainSpellController : MonoBehaviour
{
    // 0 - Rambo | 1 - Booth | 2 - Shadow
    // Rambo: Hp 210 | Armor 180 | Speed 0.8 | Ammo 1.7 | AddSpell Max 2 | Main Spell Cooldown 120 sec
    // Booth: Hp 150 | Armor 110 | Speed 1.0 | Ammo 1.2 | AddSpell Max 2 | Main Spell Cooldown 60 sec
    // Shadow: Hp 100 | Armor 80 | Speed 1.5 | Ammo 1.0 | AddSpell Max 2 |
    public static MainSpellController Instance;


    public bool IsReady { get; private set; } = true;
    private Image amount;

    private void Awake()
    {
        Instance = this;
    }

    public void SetAmountImage(GameObject holder)
    {
        amount = holder.GetComponent<UIHolder>().MainSpellAmount;
    }

    public void Use(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            Use();
        }
    }

    public void Use()
    {
        if (!IsReady)
        {
            return;
        }

        IsReady = false;

        var talents = YandexGame.savesData.Talents;
        var sc = StatsController.Instance;

        switch (YandexGame.savesData.CharacterIndex)
        {
            case 0:
                if (!InvulnerabilityController.Instance.isInvulnerability)
                {
                    InvulnerabilityController.Instance.ActivateInvulnerability();
                }
                break;
            case 1:
                TimeSpell.Instance.Activate();
                break;
            case 2:
                EnemyController.Instance.AttackEnemy();
                break;
        }

        IsReady = false;
        var time = YandexGame.savesData.CharacterIndex switch
        {
            0 => 120f,
            1 => 60f,
            _ => 40f
        };
        if (talents[50])
        {
            time *= 1.5f;
        }

        StartCoroutine(CoolDown(time));
    }

    private IEnumerator CoolDown(float time)
    {
        for (var i = 0; i <= time; i++)
        {
            yield return new WaitForSeconds(1);
            amount.fillAmount = i / time;
        }

        IsReady = true;
    }
}