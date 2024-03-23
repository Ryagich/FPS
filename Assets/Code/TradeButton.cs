using UnityEngine;
using UnityEngine.UI;

public class TradeButton : MonoBehaviour
{
    [field: SerializeField] public Button Button { get; private set; }
    [field: SerializeField] public Image Image { get; private set; }
    [field: SerializeField] public Image BackgroundImage { get; private set; }
    [field: SerializeField]public TradeItemInfo Info { get; private set; }

    public void SetInfo(TradeItemInfo info)
    {
        Info = info;
        BackgroundImage.color = info.BackgroundColor;
        Image.sprite = info.Sprite;
    }
}

public enum TradeButtonType
{
    HealthRestore,
    ArmorRestore,
    AddSpellRestore,
    AmmoRestore,
    HealthAdd,
    ArmorAdd,
    AddSpellAdd,
    AmmoAdd,
    AddDamage,
    AddLive,
}