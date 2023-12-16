//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;

namespace InfimaGames.LowPolyShooterPack.Interface
{
    /// <summary>
    /// CanvasAlpha. Changes the canvas alpha based on certain things that can happen in-game.
    /// </summary>
    public class CanvasAlpha : Element
    {
        #region FIELDS SERIALIZED
        
        [Title(label: "References")]

        [Tooltip("Canvas group to update the alpha for.")]
        [SerializeField, NotNull]
        private CanvasGroup canvasGroup;

        [Title(label: "Settings")]

        [Tooltip("Speed of interpolation.")]
        [Range(0.0f, 25.0f)]
        [SerializeField]
        private float interpolationSpeed = 12.0f;
        
        [Tooltip("Alpha of the canvasGroup while the cursor is unlocked (pause menu is open).")]
        [Range(0.0f, 1.0f)]
        [SerializeField]
        private float cursorUnlockedAlpha = 0.6f;
        
        #endregion
        
        #region METHODS
        
        protected override void Tick()
        {
            base.Tick();

            if (canvasGroup == null)
            {
                Log.ReferenceError(this, gameObject);
                
                return;
            }
            if (!(characterBehaviour as Character).CanPause)
                return;
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha,
                characterBehaviour.IsCursorLocked() ? 1.0f : cursorUnlockedAlpha, 
                Time.deltaTime * interpolationSpeed);
            //Update Alpha.
           // canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, characterBehaviour.IsCursorLocked() ? 1.0f : cursorUnlockedAlpha, Time.deltaTime * interpolationSpeed);
        }
        
        #endregion
    }
}