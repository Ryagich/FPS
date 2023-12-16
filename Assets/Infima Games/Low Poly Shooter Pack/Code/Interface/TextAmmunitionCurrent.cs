//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;
using System.Globalization;

namespace InfimaGames.LowPolyShooterPack.Interface
{
    /// <summary>
    /// Current Ammunition Text.
    /// </summary>
    public class TextAmmunitionCurrent : ElementText
    {
        #region FIELDS SERIALIZED
        
        [Title(label: "Colors")]
        
        [Tooltip("Determines if the color of the text should changes as ammunition is fired.")]
        [SerializeField]
        private bool updateColor = true;
        
        [Tooltip("Determines how fast the color changes as the ammunition is fired.")]
        [SerializeField]
        private float emptySpeed = 1.5f;
        
        [Tooltip("Color used on this text when the player character has no ammunition.")]
        [SerializeField]
        private Color emptyColor = Color.red;
        
        #endregion
        
        #region METHODS
        
        protected override void Tick()
        {
            float current = equippedWeaponBehaviour.GetAmmunitionCurrent();
            float total = equippedWeaponBehaviour.GetAmmunitionTotal();
            
            textMesh.text = current.ToString(CultureInfo.InvariantCulture);

            if (updateColor)
            {
                var colorAlpha = (current / total) * emptySpeed;
                textMesh.color = Color.Lerp(emptyColor, Color.white, colorAlpha);   
            }
        }
        
        #endregion
    }
}