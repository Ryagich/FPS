using System;
using System.Collections;
using UnityEngine;
using YG;

public class StatsController : MonoBehaviour
{
    public static StatsController Instance;
    public bool IsDead { get; private set; }
    public event Action Died;
    public bool IsHpMax => Hp.Max == Hp.Value;
    public bool IsArmorMax => Armor.Max == Armor.Value;
    public float NeedHealth => Hp.Max - Hp.Value;
    public float NeedArmor => Armor.Max - Armor.Value;
    public bool IsInit { get; private set; } = false;
    [SerializeField, Range(.0f, 1f)] private float _ratio = .75f;
    [SerializeField] private InvulnerabilityController InvulnerabilityC;
    [SerializeField] private string _secondChange = "Second Change";
    public Stat Hp { get; private set; }
    public Stat Armor { get; private set; }

    public bool TookDamageInLast5Seconds;

    private Coroutine damageInLast5Seconds;
    private int armorPlate;
    private bool haveArmorCharge = true;

    private void Awake()
    {
        Instance = this;

        StartCoroutine(RecoveryArmorPlate());
        StartCoroutine(Regen());
        StartCoroutine(RegenArmor());
        StartCoroutine(ReloadArmorCharge());
        StartCoroutine(RegenArmorForHealth());
    }

    public void Init(float maxHp, float minHp, float hp,
        float maxArmor, float minArmor, float armor)
    {
        if (IsInit)
            return;
        Hp = new Stat(maxHp, minHp, hp);
        Armor = new Stat(maxArmor, minArmor, armor);
        IsInit = true;

        YandexGame.savesData.Armor = Armor.Value;
        YandexGame.savesData.ArmorMax = Armor.Max;
        YandexGame.savesData.HealthMax = Armor.Max;
        YandexGame.savesData.Health = Hp.Value;
        //YandexGame.SaveProgress();
    }

    public void TakeArmor(float value)
    {
        Armor.AddValue(value);
        YandexGame.savesData.Armor = Armor.Value;
       // YandexGame.SaveProgress();
    }

    public void TakeDamage(float value)
    {
        if (InvulnerabilityC.isInvulnerability)
            return;
        var talents = YandexGame.savesData.Talents;
        value -= value * GetDamageResistCoefficient();
        if (talents[22] && armorPlate > 0)
        {
            CallbackController.Instance.AddCallBack(
                new CallbackInfo(CallbackTypes.Text,
                    TalentsController.Instance.GetTalentName(22)));
            armorPlate--;
            return;
        }

        if (talents[57])
        {
            value = Mathf.Clamp(value, 0, Hp.Max * .1f);
        }

        var ad = value * _ratio;
        if (ad > Armor.Value)
        {
            var wasArmor = ad > 0;
            var ld = ad - Armor.Value;
            var hd = value * (1 - _ratio) + ld;

            Armor.AddValue(-ad);
            Hp.AddValue(-hd);

            if (talents[61] && wasArmor && haveArmorCharge)
            {
                CallbackController.Instance.AddCallBack(
                    new CallbackInfo(CallbackTypes.Text,
                        TalentsController.Instance.GetTalentName(61)));
                TakeArmor(Armor.Max * .1f);
                haveArmorCharge = false;
            }
        }
        else
        {
            Armor.AddValue(-ad);
            Hp.AddValue(-value * (1 - _ratio));
        }

        if (Hp.Value <= 0)
        {
            var mainSpellC = MainSpellController.Instance;
            if (YandexGame.savesData.CharacterIndex is 0 && mainSpellC.IsReady)
            {
                mainSpellC.Use();
            }
            else if (YandexGame.savesData.AddLive > 0)
            {
                YandexGame.savesData.AddLive--;
                Hp.AddValue(Hp.Max);
                Armor.AddValue(Armor.Max);
                CallbackController.Instance.AddCallBack(new CallbackInfo(CallbackTypes.Text,_secondChange));
            }
            else if (talents[34])
            {
                Hp.AddValue(Hp.Max * .1f);
                YandexGame.savesData.Talents[34] = false;
                CallbackController.Instance.AddCallBack(
                    new CallbackInfo(CallbackTypes.Text,
                        TalentsController.Instance.GetTalentName(34)));
            }
            else if (talents[35])
            {
                Hp.AddValue(Hp.Max * .1f);
                YandexGame.savesData.Talents[35] = false;
                
                InvulnerabilityController.Instance.ActivateInvulnerability();
                CallbackController.Instance.AddCallBack(
                    new CallbackInfo(CallbackTypes.Text,
                        TalentsController.Instance.GetTalentName(35)));
                StartCoroutine(WaitHeal());
            }
            else
            {
                StopAllCoroutines();
                IsDead = true;
                Died?.Invoke();
            }
        }

        TakeDamageLast5Seconds();

        YandexGame.savesData.Armor = Armor.Value;
        YandexGame.savesData.Health = Hp.Value;
       // YandexGame.SaveProgress();
    }

    private float GetDamageResistCoefficient()
    {
        var talents = YandexGame.savesData.Talents;
        var coefficient = .0f;
        coefficient += .05f * (1 + YandexGame.savesData.Upgrades[3][4][0]);
        if (talents[41])
        {
            coefficient -= .1f;
        }

        if (talents[49])
        {
            coefficient += Armor.Value > 0 ? .15f : -.15f;
        }

        if (talents[57])
        {
            coefficient -= 1f;
        }

        if (talents[62])
        {
            coefficient += .1f;
        }

        return coefficient;
    }

    public void TakeHealth(float value)
    {
        value += value * .1f * (YandexGame.savesData.Upgrades[3][2][0] + 1);
        if (YandexGame.savesData.Talents[14])
        {
            value *= 2;
        }

        Hp.AddValue(value);
        YandexGame.savesData.Health = Hp.Value;
        //YandexGame.SaveProgress();
    }

    private IEnumerator WaitHeal()
    {
        yield return new WaitForSeconds(5);
        if (Hp.Value <= 0)
        {
            StopAllCoroutines();
            IsDead = true;
            Died?.Invoke();
        }
    }
    public void SetNewMaxHealth(float newMax)
    {
        Hp.ChangeMax(newMax);
        Hp.ChangeValue(Hp.Value);

        YandexGame.savesData.HealthMax = Hp.Max;
        YandexGame.savesData.Health = Hp.Value;
        //YandexGame.SaveProgress();
    }

    public void SetNewMaxArmor(float newMax)
    {
        Armor.ChangeMax(newMax);
        Armor.ChangeValue(Armor.Value);

        YandexGame.savesData.ArmorMax = Armor.Max;
        YandexGame.savesData.Armor = Armor.Value;
        //      YandexGame.SaveProgress();
    }

    private void TakeDamageLast5Seconds()
    {
        if (damageInLast5Seconds is not null)
        {
            StopCoroutine(damageInLast5Seconds);
        }

        damageInLast5Seconds = StartCoroutine(HoldDamageLast5Second());
    }

    private IEnumerator HoldDamageLast5Second()
    {
        TookDamageInLast5Seconds = true;

        for (var i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(1);
            if (YandexGame.savesData.Talents[31])
            {
                TakeHealth(3);
            }
        }

        TookDamageInLast5Seconds = false;
    }

    private IEnumerator ReloadArmorCharge()
    {
        while (true)
        {
            yield return new WaitForSeconds(45);
            haveArmorCharge = true;
        }
    }

    private IEnumerator RecoveryArmorPlate()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);
            if (armorPlate + 1 <= 3)
                armorPlate++;
        }
    }

    private IEnumerator Regen()
    {
        var regen = YandexGame.savesData.Upgrades[3][3][0] switch
        {
            0 => .5f,
            1 => 1f,
            2 => 2f,
            _ => .0f
        };
        if (regen == .0f)
            yield break;
        while (true)
        {
            yield return new WaitForSeconds(1f);
            TakeHealth(regen);
        }
    }

    private IEnumerator RegenArmor()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (YandexGame.savesData.Talents[64])
            {
                TakeArmor(1);
            }
        }
    }

    private IEnumerator RegenArmorForHealth()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (YandexGame.savesData.Talents[37] && Armor.Value < Armor.Max)
            {
                var value = Hp.Max * .04f;
                if (Hp.Value - value >= Hp.Max * .2f)
                {
                    TakeArmor(value);
                    TakeHealth(-value);
                }
            }
        }
    }
}