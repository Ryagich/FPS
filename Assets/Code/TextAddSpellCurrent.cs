using UnityEngine;
using System.Globalization;

namespace InfimaGames.LowPolyShooterPack.Interface
{
    public class TextAddSpellCurrent : ElementText
    {
        [Title(label: "Colors")]
        [SerializeField] private bool updateColor = true;
        [SerializeField] private float emptySpeed = 1.5f;
        [SerializeField] private Color emptyColor = Color.red;

        protected override void Tick()
        {
            var addSpellC = Character.Instance.GetComponent<AddSpellController>();

            textMesh.text = addSpellC.SpellCount.ToString(CultureInfo.InvariantCulture);

            if (!updateColor)
                return;
            var colorAlpha = (float)addSpellC.SpellCount / addSpellC.MaxSpell * emptySpeed;
            textMesh.color = Color.Lerp(emptyColor, Color.white, colorAlpha);
        }
    }
}
