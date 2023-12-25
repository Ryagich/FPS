//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    public abstract class CharacterBehaviour : MonoCache
    {
        #region UNITY
        protected virtual void Awake(){}
        protected virtual void Start(){}
//        protected override void Run() { }
        #endregion
        
        #region GETTERS
        /// <summary>
        /// This function should return the amount of shots that the character has fired in succession.
        /// Using this value for applying recoil, and for modifying spread is what we have this function for.
        /// </summary>
        /// <returns></returns>
        public abstract int GetShotsFired();
        public abstract bool IsLowered();
        public abstract Camera GetCameraWorld();
        public abstract Camera GetCameraDepth();
        public abstract InventoryBehaviour GetInventory();
        public abstract int GetGrenadesCurrent();
        public abstract int GetGrenadesTotal();
        public abstract bool IsRunning();
        public abstract bool IsHolstered();
        public abstract bool IsCrouching();
        public abstract bool IsReloading();
        public abstract bool IsThrowingGrenade();
        public abstract bool IsMeleeing();
        public abstract bool IsAiming();
        public abstract bool IsCursorLocked();
        public abstract bool IsTutorialTextVisible();
        public abstract Vector2 GetInputMovement();
        public abstract Vector2 GetInputLook();
        public abstract AudioClip[] GetAudioClipsGrenadeThrow();
        public abstract AudioClip[] GetAudioClipsMelee();
        public abstract bool IsInspecting();
        public abstract bool IsHoldingButtonFire();
        #endregion

        #region ANIMATION

        /// <summary>
        /// Ejects a casing from the equipped weapon.
        /// </summary>
        public abstract void EjectCasing();
        /// <summary>
        /// Fills the character's equipped weapon's ammunition by a certain amount, or fully if set to -1.
        /// </summary>
        public abstract void FillAmmunition(int amount);

        /// <summary>
        /// Throws a grenade.
        /// </summary>
        public abstract void Grenade();
        /// <summary>
        /// Sets the equipped weapon's magazine to be active or inactive!
        /// </summary>
        public abstract void SetActiveMagazine(int active);
        
        /// <summary>
        /// Bolt Animation Ended.
        /// </summary>
        public abstract void AnimationEndedBolt();
        /// <summary>
        /// Reload Animation Ended.
        /// </summary>
        public abstract void AnimationEndedReload();

        /// <summary>
        /// Grenade Throw Animation Ended.
        /// </summary>
        public abstract void AnimationEndedGrenadeThrow();
        /// <summary>
        /// Melee Animation Ended.
        /// </summary>
        public abstract void AnimationEndedMelee();

        /// <summary>
        /// Inspect Animation Ended.
        /// </summary>
        public abstract void AnimationEndedInspect();
        /// <summary>
        /// Holster Animation Ended.
        /// </summary>
        public abstract void AnimationEndedHolster();

        /// <summary>
        /// Sets the equipped weapon's slide back pose.
        /// </summary>
        public abstract void SetSlideBack(int back);

        public abstract void SetActiveKnife(int active);

        #endregion
    }
}