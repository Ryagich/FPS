using UnityEngine;
using UnityEngine.UI;

public class SelectedTalent : MonoBehaviour
{
    [field: SerializeField] public Button Button { get; private set; }
    [field: SerializeField] public Image Image { get; private set; }
    [field: SerializeField] public Image BackgroundImage { get; private set; }
    [HideInInspector] public TalentInfo Info;

    public void SetInfo(TalentInfo info)
    {
        Info = info;
        BackgroundImage.color = info.BackgroundColor;
        Image.sprite = info.Sprite;
    }
}