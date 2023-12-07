//Copyright 2022, Infima Games. All Rights Reserved.

using System.Globalization;
using UnityEngine;

namespace InfimaGames.LowPolyShooterPack.Interface
{
    public class TextAmmunitionTotal : ElementText
    {
        private Color emptyColor = Color.red;
        private float emptySpeed = 0.35f;

        #region METHODS

        protected override void Tick()
        {
            var inventory = inventoryBehaviour as Inventory;
            var current = inventory.GetCurrentAmmo();
            float total = inventory.GetCurrentMaxAmmo();
            float colorAlpha = current / total;
            
            textMesh.color = Color.Lerp(emptyColor, Color.white, colorAlpha);   

            textMesh.text = current.ToString(CultureInfo.InvariantCulture);
        }

        #endregion
    }
}