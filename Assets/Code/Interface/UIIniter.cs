using UnityEngine;

[RequireComponent(typeof(BeatingHeart))]
[RequireComponent(typeof(StatsController))]
[RequireComponent(typeof(HpBarColorController))]
[RequireComponent(typeof(BloodScreen))]
public class UIIniter : MonoBehaviour
{
    [SerializeField, Min(.0f)] private float _fillSpeed = 2f;

    public void Init(GameObject canvas)
    {
        var hpColor = GetComponent<HpBarColorController>();
        var sc = GetComponent<StatsController>();
        var heart = GetComponent<BeatingHeart>();
        var blood = GetComponent<BloodScreen>();

        var bh = canvas.GetComponent<UIHolder>();

        sc.Init();

        bh.HP.fillAmount = sc.Hp.Value / sc.Hp.Max;
        bh.Armor.fillAmount = sc.Armor.Value / sc.Armor.Max;

        var hpFiller = new StatFiller(bh.HP, sc.Hp, this, _fillSpeed);
        var armorFiller = new StatFiller(bh.Armor, sc.Armor, this, _fillSpeed);

        heart.Init(bh.Heart, sc.Hp, hpFiller);
        blood.Init(bh.BloodScreen, sc.Hp, hpFiller,heart);
        hpColor.Init(hpFiller, bh.HP);
        hpColor.ChangeColor(sc.Hp.Value / sc.Hp.Max);
    }
}