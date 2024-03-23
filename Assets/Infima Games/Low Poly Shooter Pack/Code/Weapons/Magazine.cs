using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    public class Magazine : MagazineBehaviour
    {
        [Title(label: "Settings")]
        
        [Tooltip("Total Ammunition.")]
        [SerializeField]
        private int ammunitionTotal = 10;

        [Title(label: "Interface")]

        [Tooltip("Interface Sprite."), SerializeField] private Sprite sprite;

        public override int GetAmmunitionTotal() => ammunitionTotal;
        public override Sprite GetSprite() => sprite;
    }
}