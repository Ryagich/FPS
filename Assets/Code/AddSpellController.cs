using UnityEngine;
using UnityEngine.InputSystem;
using YG;

public class AddSpellController : MonoBehaviour
{
    public static AddSpellController Instance;
    [field: SerializeField] public int MaxSpell { get; private set; } = 2;

    [HideInInspector] public int SpellCount;

    private void Awake()
    {
        if (YandexGame.savesData.AddSpellMax is not -1)
        {
            MaxSpell = YandexGame.savesData.AddSpellMax;
            SpellCount = YandexGame.savesData.AddSpellCount;
        }
        else
        {
            SpellCount = MaxSpell;
            YandexGame.savesData.AddSpellMax = MaxSpell;
            YandexGame.savesData.AddSpellCount = SpellCount;
            // YandexGame.SaveProgress();
        }

        Instance = this;
    }

    public void Use(InputAction.CallbackContext context)
    {
        if (!context.started)
        {
            return;
        }

        var talents = YandexGame.savesData.Talents;
        var sc = StatsController.Instance;

        if (SpellCount == 0)
        {
            if (talents[51])
            {
                SpellCount++;
                StatsController.Instance.TakeDamage(25);
            }
            else
            {
                return;
            }
        }

        switch (YandexGame.savesData.CharacterIndex)
        {
            case 0:
                sc.TakeHealth(sc.Hp.Max * .1f);
                sc.TakeArmor(sc.Armor.Max * .05f);
                break;
            case 1:
                YandexGame.savesData.BootAddSpellBullets = 4;
                break;
            case 2:
                break;
        }

        if (talents[56])
        {
            sc.TakeArmor(sc.Armor.Max * .05f);
        }

        SpellCount--;
    }

    public void SetMax()
    {
        AddSpell(MaxSpell);
    }

    public void AddSpell(int value)
    {
        SpellCount = Mathf.Clamp(SpellCount + value, 0, MaxSpell);
        YandexGame.savesData.AddSpellCount = SpellCount;
        // YandexGame.SaveProgress();
    }

    public void AddMax()
    {
        MaxSpell++;
        YandexGame.savesData.AddSpellMax = MaxSpell;
        // YandexGame.SaveProgress();
    }
}