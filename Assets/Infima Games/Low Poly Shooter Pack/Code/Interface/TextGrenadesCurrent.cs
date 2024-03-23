using UnityEngine;
using System.Globalization;

namespace InfimaGames.LowPolyShooterPack.Interface
{
    public class TextGrenadesCurrent : ElementText
    {
        [SerializeField] private bool updateColor = true;
        [SerializeField] private float emptySpeed = 1.5f;
        [SerializeField] private Color emptyColor = Color.red;

        protected override void Tick()
        {
            var current = characterBehaviour.GetGrenadesCurrent();
            var total = characterBehaviour.GetGrenadesTotal();

            textMesh.text = current.ToString(CultureInfo.InvariantCulture);

            if (!updateColor)
                return;
            var colorAlpha = current / total * emptySpeed;
            textMesh.color = Color.Lerp(emptyColor, Color.white, colorAlpha);
        }
    }
}