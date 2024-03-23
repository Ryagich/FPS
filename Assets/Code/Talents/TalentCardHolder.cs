using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TalentCardHolder : MonoBehaviour
{
    [field: SerializeField] public TMP_Text Name { get; private set; }
    [field: SerializeField] public TMP_Text Description { get; private set; }
    [field: SerializeField] public Image Icon { get; private set; }
    [field: SerializeField] public Image IconBackground { get; private set; }
    [field: SerializeField] public Button Button { get; private set; }

    public void SetColor(Color color)
    {
        IconBackground.color = color;
    }

    public void SetIcon(Sprite sprite)
    {
        Icon.sprite = sprite;
    }

    public void SetText(string name, string description)
    {
        Name.text = name;
        Description.text = description;
    }
}