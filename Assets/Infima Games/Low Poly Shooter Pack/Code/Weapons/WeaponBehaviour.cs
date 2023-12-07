//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    public abstract class WeaponBehaviour : MonoBehaviour
    {
        #region UNITY

        /// <summary>
        /// Awake.
        /// </summary>
        protected virtual void Awake(){}

        /// <summary>
        /// Start.
        /// </summary>
        protected virtual void Start(){}

        /// <summary>
        /// Update.
        /// </summary>
        protected virtual void Update(){}

        /// <summary>
        /// Late Update.
        /// </summary>
        protected virtual void LateUpdate(){}

        #endregion

        #region GETTERS
        public abstract Sprite GetSpriteBody();
        public abstract float GetMultiplierMovementSpeed();
        public abstract AudioClip GetAudioClipHolster();
        public abstract AudioClip GetAudioClipUnholster();
        public abstract AudioClip GetAudioClipReload();
        public abstract AudioClip GetAudioClipReloadEmpty();
        public abstract AudioClip GetAudioClipReloadOpen();
        public abstract AudioClip GetAudioClipReloadInsert();
        public abstract AudioClip GetAudioClipReloadClose();
        public abstract AudioClip GetAudioClipFireEmpty();
        public abstract AudioClip GetAudioClipBoltAction();
        public abstract AudioClip GetAudioClipFire();
        public abstract int GetAmmunitionCurrent();
        public abstract int GetAmmunitionTotal();
        public abstract bool HasCycledReload();
        public abstract Animator GetAnimator();
        public abstract bool CanReloadAimed();
        public abstract bool IsAutomatic();
        public abstract bool HasAmmunition();
        public abstract bool IsFull();
        public abstract bool IsBoltAction();
        public abstract bool GetAutomaticallyReloadOnEmpty();
        public abstract float GetAutomaticallyReloadOnEmptyDelay();
        public abstract bool CanReloadWhenFull();
        public abstract float GetRateOfFire();
        public abstract float GetFieldOfViewMultiplierAim();
        public abstract float GetFieldOfViewMultiplierAimWeapon();
        public abstract RuntimeAnimatorController GetAnimatorController();
        public abstract WeaponAttachmentManagerBehaviour GetAttachmentManager();
        #endregion

        #region METHODS
        public abstract void Fire(float spreadMultiplier = 1.0f);
        public abstract void Reload();
        public abstract void FillAmmunition(int amount);
        public abstract void SetSlideBack(int back);
        public abstract void EjectCasing();
        #endregion
    }
}