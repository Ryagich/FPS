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
        var talents = YandexGame.savesData.Talents;
        var sc = StatsController.Instance;
        if (!context.started)
        {
            return;
        }

        if (SpellCount == 0)
        {
            if (talents[51])
            {
                if (talents[56])
                {
                    sc.TakeArmor(sc.Armor.Max * .05f);
                }

                StatsController.Instance.TakeDamage(25);
            }

            return;
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