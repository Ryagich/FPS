﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Main Character Component. This component handles the most important functions of the character, and interfaces
    /// with basically every part of the asset, it is the hub where it all converges.
    /// </summary>
    [RequireComponent(typeof(CharacterKinematics))]
    public sealed class Character : CharacterBehaviour
    {
        [Title(label: "References")] [Tooltip("The character's LowerWeapon component.")] [SerializeField]
        private LowerWeapon lowerWeapon;

        [Title(label: "Inventory")]
        [Tooltip("Determines the index of the weapon to equip when the game starts.")]
        [SerializeField]
        private int weaponIndexEquippedAtStart;

        [Tooltip("Inventory.")] [SerializeField]
        private InventoryBehaviour inventory;

        [Title(label: "Grenade")] [Tooltip("If true, the character's grenades will never run out.")] [SerializeField]
        private bool grenadesUnlimited;

        [Tooltip("Total amount of grenades at start.")] [SerializeField]
        private int grenadeTotal = 10;

        [Tooltip("Grenade spawn offset from the character's camera.")] [SerializeField]
        private float grenadeSpawnOffset = 1.0f;

        [Tooltip("Grenade Prefab. Spawned when throwing a grenade.")] [SerializeField]
        private GameObject grenadePrefab;

        [Title(label: "Knife")] [Tooltip("Knife GameObject.")] [SerializeField]
        private GameObject knife;

        [Title(label: "Cameras")] [Tooltip("Normal Camera.")] [SerializeField]
        private Camera cameraWorld;

        [Tooltip("Weapon-Only Camera. Depth.")] [SerializeField]
        private Camera cameraDepth;

        [Title(label: "Animation")] [Tooltip("Determines how smooth the turning animation is.")] [SerializeField]
        private float dampTimeTurning = 0.4f;

        [Tooltip("Determines how smooth the locomotion blendspace is.")] [SerializeField]
        private float dampTimeLocomotion = 0.15f;

        [Tooltip("How smoothly we play aiming transitions. Beware that this affects lots of things!")] [SerializeField]
        private float dampTimeAiming = 0.3f;

        [Tooltip("Interpolation speed for the running offsets.")] [SerializeField]
        private float runningInterpolationSpeed = 12.0f;

        [Tooltip("Determines how fast the character's weapons are aimed.")] [SerializeField]
        private float aimingSpeedMultiplier = 1.0f;

        [Title(label: "Animation Procedural")] [Tooltip("Character Animator.")] [SerializeField]
        private Animator characterAnimator;

        [Title(label: "Field Of View")] [Tooltip("Normal world field of view.")] [SerializeField]
        private float fieldOfView = 100.0f;

        [Tooltip("Multiplier for the field of view while running.")] [SerializeField]
        private float fieldOfViewRunningMultiplier = 1.05f;

        [Tooltip("Weapon-specific field of view.")] [SerializeField]
        private float fieldOfViewWeapon = 55.0f;

        [Title(label: "Audio Clips")] [Tooltip("Melee Audio Clips.")] [SerializeField]
        private AudioClip[] audioClipsMelee;

        [Tooltip("Grenade Throw Audio Clips.")] [SerializeField]
        private AudioClip[] audioClipsGrenadeThrow;

        [Title(label: "Input Options")]
        [Tooltip("If true, the running input has to be held to be active.")]
        [SerializeField]
        private bool holdToRun = true;

        [Tooltip("If true, the aiming input has to be held to be active.")] [SerializeField]
        private bool holdToAim = true;

        public bool CanPause = true;

        private bool aiming;
        private bool wasAiming;
        private bool running;
        private bool holstered;
        private float lastShotTime;
        private int layerOverlay;
        private int layerHolster;
        private int layerActions;
        private MovementBehaviour movementBehaviour;
        private WeaponBehaviour equippedWeapon;
        private WeaponAttachmentManagerBehaviour weaponAttachmentManager;
        private ScopeBehaviour equippedWeaponScope;
        private MagazineBehaviour equippedWeaponMagazine;
        public bool reloading;
        private bool inspecting;
        private bool throwingGrenade;
        private bool meleeing;
        private bool holstering;
        private float aimingAlpha;
        private float crouchingAlpha;
        private float runningAlpha;
        private Vector2 axisLook;
        private Vector2 axisMovement;
        public bool bolting;
        private int grenadeCount;
        private bool holdingButtonAim;
        private bool holdingButtonRun;
        public bool holdingButtonFire;
        private bool tutorialTextVisible;
        public bool cursorLocked;
        private int shotsFired;
        private bool canSwitchWeaponState = false;

        public static Character Instance;

        protected override void Awake()
        {
            Instance = this;
            cursorLocked = true;
            UpdateCursorState();

            movementBehaviour = GetComponent<MovementBehaviour>();
        }

        public void FillGrenades() => grenadeCount = grenadeTotal;

        protected override void Start()
        {
            FillGrenades();

            //Hide knife. We do this so we don't see a giant knife stabbing through the character's hands all the time!
            if (knife != null)
                knife.SetActive(false);

            layerHolster = characterAnimator.GetLayerIndex("Layer Holster");
            layerActions = characterAnimator.GetLayerIndex("Layer Actions");
            layerOverlay = characterAnimator.GetLayerIndex("Layer Overlay");
        }

        protected override void Run()
        {
            aiming = holdingButtonAim && CanAim();
            running = holdingButtonRun && CanRun();
            switch (aiming)
            {
                case true when !wasAiming:
                    equippedWeaponScope.OnAim();
                    break;
                case false when wasAiming:
                    equippedWeaponScope.OnAimStop();
                    break;
            }

            if (Time.timeScale == 0)
            {
                canSwitchWeaponState = false;
                OnTryFire(new InputAction.CallbackContext());
                holdingButtonFire = false;
            }
            else
                canSwitchWeaponState = true;

            if (holdingButtonFire)
            {
                if (CanPlayAnimationFire() && equippedWeapon.HasAmmunition() && equippedWeapon.IsAutomatic() &&
                    cursorLocked)
                {
                    if (Time.time - lastShotTime > 60.0f / equippedWeapon.GetRateOfFire())
                        Fire();
                }
                else
                {
                    shotsFired = 0;
                }
            }

            UpdateAnimator();

            //Update Aiming Alpha. We need to get this here because we're using the Animator to interpolate the aiming value.
            aimingAlpha = characterAnimator.GetFloat(AHashes.AimingAlpha);

            //Interpolate the crouching alpha. We do this here as a quick and dirty shortcut, but there's definitely better ways to do this.
            crouchingAlpha = Mathf.Lerp(crouchingAlpha, movementBehaviour.IsCrouching() ? 1.0f : 0.0f,
                Time.deltaTime * 12.0f);
            //Interpolate the running alpha. We do this here as a quick and dirty shortcut, but there's definitely better ways to do this.
            runningAlpha = Mathf.Lerp(runningAlpha, running ? 1.0f : 0.0f, Time.deltaTime * runningInterpolationSpeed);

            //Running Field Of View Multiplier.
            var runningFieldOfView = Mathf.Lerp(1.0f, fieldOfViewRunningMultiplier, runningAlpha);

            //Interpolate the world camera's field of view based on whether we are aiming or not.
            cameraWorld.fieldOfView =
                Mathf.Lerp(fieldOfView, fieldOfView * equippedWeapon.GetFieldOfViewMultiplierAim(), aimingAlpha) *
                runningFieldOfView;
            //Interpolate the depth camera's field of view based on whether we are aiming or not.
            cameraDepth.fieldOfView = Mathf.Lerp(fieldOfViewWeapon,
                fieldOfViewWeapon * equippedWeapon.GetFieldOfViewMultiplierAimWeapon(), aimingAlpha);

            wasAiming = aiming;
        }

        #region GETTERS

        public override int GetShotsFired() => shotsFired;

        public override bool IsLowered()
        {
            //Weapons are never lowered if we don't even have a LowerWeapon component.
            if (lowerWeapon == null)
                return false;

            return lowerWeapon.IsLowered();
        }

        public override Camera GetCameraWorld() => cameraWorld;
        public override Camera GetCameraDepth() => cameraDepth;
        public override InventoryBehaviour GetInventory() => inventory;
        public override int GetGrenadesCurrent() => grenadeCount;
        public override int GetGrenadesTotal() => grenadeTotal;
        public override bool IsRunning() => running;
        public override bool IsHolstered() => holstered;
        public override bool IsCrouching() => movementBehaviour.IsCrouching();
        public override bool IsReloading() => reloading;
        public override bool IsThrowingGrenade() => throwingGrenade;
        public override bool IsMeleeing() => meleeing;
        public override bool IsAiming() => aiming;
        public override bool IsCursorLocked() => cursorLocked;
        public override bool IsTutorialTextVisible() => tutorialTextVisible;
        public override Vector2 GetInputMovement() => axisMovement;
        public override Vector2 GetInputLook() => axisLook;
        public override AudioClip[] GetAudioClipsGrenadeThrow() => audioClipsGrenadeThrow;
        public override AudioClip[] GetAudioClipsMelee() => audioClipsMelee;
        public override bool IsInspecting() => inspecting;
        public override bool IsHoldingButtonFire() => holdingButtonFire;

        #endregion

        [SerializeField] private float _motionTime;
        [SerializeField] private float _dampTime;
        private float lastLV;

        private void UpdateAnimator()
        {
            #region Reload Stop

            //Check if we're currently reloading cycled.
            const string boolNameReloading = "Reloading";
            if (characterAnimator.GetBool(boolNameReloading))
            {
                //If we only have one more bullet to reload, then we can change the boolean already.
                if (equippedWeapon.GetAmmunitionTotal() - equippedWeapon.GetAmmunitionCurrent() < 1)
                {
                    //Update the character animator.
                    characterAnimator.SetBool(boolNameReloading, false);
                    //Update the weapon animator.
                    equippedWeapon.GetAnimator().SetBool(boolNameReloading, false);
                }
            }

            #endregion

            _motionTime = Time.fixedDeltaTime * Time.timeScale;
            //Leaning. Affects how much the character should apply of the leaning additive animation.
            var leaningValue = Mathf.Clamp01(axisMovement.y);
            _dampTime = Mathf.Clamp(
                lastLV < leaningValue
                        ? axisMovement.y / 1.5f
                        : axisMovement.y,
                0, 0.5f);

            lastLV = leaningValue;
            characterAnimator.SetFloat(AHashes.LeaningForward, leaningValue, _dampTime, _motionTime);
            //Movement Value. This value affects absolute movement. Aiming movement uses this, as opposed to per-axis movement.
            var movementValue = Mathf.Clamp01(Mathf.Abs(axisMovement.x) + Mathf.Abs(axisMovement.y));
            characterAnimator.SetFloat(AHashes.Movement, movementValue, dampTimeLocomotion, _motionTime);

            //Aiming Speed Multiplier.
            characterAnimator.SetFloat(AHashes.AimingSpeedMultiplier, aimingSpeedMultiplier);

            //Turning Value. This determines how much of the turning animation to play based on our current look rotation.
            characterAnimator.SetFloat(AHashes.Turning, Mathf.Abs(axisLook.x), dampTimeTurning, _motionTime);

            //Horizontal Movement Float.
            characterAnimator.SetFloat(AHashes.Horizontal, axisMovement.x, dampTimeLocomotion, _motionTime);
            //Vertical Movement Float.
            characterAnimator.SetFloat(AHashes.Vertical, axisMovement.y, dampTimeLocomotion, _motionTime);

            //Update the aiming value, but use interpolation. This makes sure that things like firing can transition properly.
            characterAnimator.SetFloat(AHashes.AimingAlpha, Convert.ToSingle(aiming), dampTimeAiming, _motionTime);

            //Set the locomotion play rate. This basically stops movement from happening while in the air.
            const string playRateLocomotionBool = "Play Rate Locomotion";
            characterAnimator.SetFloat(playRateLocomotionBool, movementBehaviour.IsGrounded() ? 1.0f : 0.0f, 0.2f,
                _motionTime);

            #region Movement Play Rates

            //Update Forward Multiplier. This allows us to change the play rate of our animations based on our movement multipliers.
            characterAnimator.SetFloat(AHashes.PlayRateLocomotionForward, movementBehaviour.GetMultiplierForward(),
                0.2f, Time.deltaTime);
            //Update Sideways Multiplier. This allows us to change the play rate of our animations based on our movement multipliers.
            characterAnimator.SetFloat(AHashes.PlayRateLocomotionSideways, movementBehaviour.GetMultiplierSideways(),
                0.2f, Time.deltaTime);
            //Update Backwards Multiplier. This allows us to change the play rate of our animations based on our movement multipliers.
            characterAnimator.SetFloat(AHashes.PlayRateLocomotionBackwards, movementBehaviour.GetMultiplierBackwards(),
                0.2f, Time.deltaTime);

            #endregion

            //Update Animator Aiming.
            characterAnimator.SetBool(AHashes.Aim, aiming);
            //Update Animator Running.
            characterAnimator.SetBool(AHashes.Running, running);
            //Update Animator Crouching.
            characterAnimator.SetBool(AHashes.Crouching, movementBehaviour.IsCrouching());
        }

        private void Inspect()
        {
            //State.
            inspecting = true;
            //Play.
            characterAnimator.CrossFade("Inspect", 0.0f, layerActions, 0);
        }

        private void Fire()
        {
            shotsFired++;

            lastShotTime = Time.time;
            equippedWeapon.Fire(aiming ? equippedWeaponScope.GetMultiplierSpread() : 1.0f);

            const string stateName = "Fire";
            characterAnimator.CrossFade(stateName, 0.05f, layerOverlay, 0);

            if (equippedWeapon.IsBoltAction() && !bolting && equippedWeapon.HasAmmunition())
                UpdateBolt(true);

            if (!equippedWeapon.HasAmmunition() && equippedWeapon.GetAutomaticallyReloadOnEmpty())
                StartCoroutine(nameof(TryReloadAutomatic));
        }

        private void PlayReloadAnimation()
        {
            var i = inventory as Inventory;

            if (!i.CheckAmmo(equippedWeapon.GetComponent<Weapon>().AmmoType))
                return;

            #region Animation

            //Get the name of the animation state to play, which depends on weapon settings, and ammunition!
            var stateName = equippedWeapon.HasCycledReload()
                ? "Reload Open"
                : (equippedWeapon.HasAmmunition() ? "Reload" : "Reload Empty");

            //Play the animation state!
            characterAnimator.Play(stateName, layerActions, 0.0f);

            #endregion

            //Set Reloading Bool. This helps cycled reloads know when they need to stop cycling.
            characterAnimator.SetBool(AHashes.Reloading, reloading = true);

            //Reload.
            equippedWeapon.Reload();
        }

        private IEnumerator TryReloadAutomatic()
        {
            yield return new WaitForSeconds(equippedWeapon.GetAutomaticallyReloadOnEmptyDelay());

            PlayReloadAnimation();
        }

        public IEnumerator Equip(int index = 0)
        {
            if (!holstered)
            {
                SetHolstered(holstering = true);
                yield return new WaitUntil(() => holstering == false);
            }

            SetHolstered(false);
            characterAnimator.Play("Unholster", layerHolster, 0);

            inventory.Equip(index);
            RefreshWeaponSetup();
        }

        public void RefreshWeaponSetup()
        {
            if ((equippedWeapon = inventory.GetEquipped()) == null)
            {
                Debug.LogError(equippedWeapon);
                return;
            }

            characterAnimator.runtimeAnimatorController = equippedWeapon.GetAnimatorController();
            weaponAttachmentManager = equippedWeapon.GetAttachmentManager();
            if (weaponAttachmentManager == null)
                return;
            equippedWeaponScope = weaponAttachmentManager.GetEquippedScope();
            equippedWeaponMagazine = weaponAttachmentManager.GetEquippedMagazine();
        }

        private void FireEmpty()
        {
            lastShotTime = Time.time;
            characterAnimator.CrossFade("Fire Empty", 0.05f, layerOverlay, 0);
        }


        private void PlayGrenadeThrow()
        {
            throwingGrenade = true;
            characterAnimator.CrossFade("Grenade Throw", 0.15f,
                characterAnimator.GetLayerIndex("Layer Actions Arm Left"), 0.0f);
            characterAnimator.CrossFade("Grenade Throw", 0.05f,
                characterAnimator.GetLayerIndex("Layer Actions Arm Right"), 0.0f);
        }

        private void PlayMelee()
        {
            //Start State.
            meleeing = true;

            //Play Normal.
            characterAnimator.CrossFade("Knife Attack", 0.05f,
                characterAnimator.GetLayerIndex("Layer Actions Arm Left"), 0.0f);

            //Play Additive.
            characterAnimator.CrossFade("Knife Attack", 0.05f,
                characterAnimator.GetLayerIndex("Layer Actions Arm Right"), 0.0f);
        }

        private void UpdateBolt(bool value)
        {
            characterAnimator.SetBool(AHashes.Bolt, bolting = value);
        }

        private void SetHolstered(bool value = true)
        {
            holstered = value;

            const string boolName = "Holstered";
            characterAnimator.SetBool(boolName, holstered);
        }

        private IEnumerator PickupWeapon(WeaponBehaviour weapon, GameObject go)
        {
            if (!holstered)
            {
                SetHolstered(holstering = true);
                yield return new WaitUntil(() => holstering == false);
            }

            SetHolstered(false);
            yield return new WaitUntil(() => !holstering);

            if (inventory is Inventory concreteInventory)
            {
                concreteInventory.ChangeWeapon(weapon, go);
            }
            else
                throw new Exception("inventory is not Inventory");
        }

        #region ACTION CHECKS

        private bool CanPlayAnimationFire()
        {
            if (holstered || holstering)
                return false;
            if (meleeing || throwingGrenade)
                return false;
            if (reloading || bolting)
                return false;
            if (inspecting)
                return false;

            return true;
        }

        /// <summary>
        /// Determines if we can play the reload animation.
        /// </summary>
        private bool CanPlayAnimationReload()
        {
            //No reloading!
            if (reloading)
                return false;

            //No meleeing!
            if (meleeing)
                return false;

            //Not actioning a bolt.
            if (bolting)
                return false;

            //Can't reload while throwing a grenade.
            if (throwingGrenade)
                return false;

            //Block while inspecting.
            if (inspecting)
                return false;

            //Block Full Reloading if needed.
            if (!equippedWeapon.CanReloadWhenFull() && equippedWeapon.IsFull())
                return false;

            //Return.
            return true;
        }

        /// <summary>
        /// Returns true if the character is able to throw a grenade.
        /// </summary>
        private bool CanPlayAnimationGrenadeThrow()
        {
            //Block.
            if (holstered || holstering)
                return false;

            //Block.
            if (meleeing || throwingGrenade)
                return false;

            //Block.
            if (reloading || bolting)
                return false;

            //Block.
            if (inspecting)
                return false;

            //We need to have grenades!
            if (!grenadesUnlimited && grenadeCount == 0)
                return false;

            //Return.
            return true;
        }

        /// <summary>
        /// Returns true if the Character is able to melee attack.
        /// </summary>
        private bool CanPlayAnimationMelee()
        {
            //Block.
            if (holstered || holstering)
                return false;

            //Block.
            if (meleeing || throwingGrenade)
                return false;

            //Block.
            if (reloading || bolting)
                return false;

            //Block.
            if (inspecting)
                return false;

            //Return.
            return true;
        }

        /// <summary>
        /// Returns true if the character is able to holster their weapon.
        /// </summary>
        /// <returns></returns>
        private bool CanPlayAnimationHolster()
        {
            //Block.
            if (meleeing || throwingGrenade)
                return false;

            //Block.
            if (reloading || bolting)
                return false;

            //Block.
            if (inspecting)
                return false;

            //Return.
            return true;
        }

        /// <summary>
        /// Returns true if the Character can change their Weapon.
        /// </summary>
        /// <returns></returns>
        private bool CanChangeWeapon()
        {
            //Block.
            if (holstering)
                return false;

            //Block.
            if (meleeing || throwingGrenade)
                return false;

            //Block.
            if (reloading || bolting)
                return false;

            //Block.
            if (inspecting)
                return false;

            //Return.
            return true;
        }

        /// <summary>
        /// Returns true if the Character can play the Inspect animation.
        /// </summary>
        private bool CanPlayAnimationInspect()
        {
            //Block.
            if (holstered || holstering)
                return false;

            //Block.
            if (meleeing || throwingGrenade)
                return false;

            //Block.
            if (reloading || bolting)
                return false;

            //Block.
            if (inspecting)
                return false;

            return true;
        }

        private bool CanAim()
        {
            //Block.
            if (holstered || inspecting)
                return false;

            //Block.
            if (meleeing || throwingGrenade)
                return false;

            //Block.
            if ((!equippedWeapon.CanReloadAimed() && reloading) || holstering)
                return false;

            return true;
        }

        private bool CanRun()
        {
            //Block.
            if (inspecting || bolting)
                return false;

            //No running while crouching.
            if (movementBehaviour.IsCrouching())
                return false;

            //Block.
            if (meleeing || throwingGrenade)
                return false;

            //Block.
            if (reloading || aiming)
                return false;

            //While trying to fire, we don't want to run. We do this just in case we do fire.
            if (holdingButtonFire && equippedWeapon.HasAmmunition())
                return false;

            //This blocks running backwards, or while fully moving sideways.
            if (axisMovement.y <= 0 || Math.Abs(Mathf.Abs(axisMovement.x) - 1) < 0.01f)
                return false;

            return true;
        }

        #endregion

        #region INPUT

        public void OnTryPickupWeapon(WeaponBehaviour weapon, GameObject go)
        {
            StartCoroutine(PickupWeapon(weapon, go));
        }

        public void OnTryFire(InputAction.CallbackContext context)
        {
            if (!cursorLocked || !canSwitchWeaponState)
                return;
            switch (context)
            {
                case { phase: InputActionPhase.Started }:
                    holdingButtonFire = true;
                    shotsFired = 0;
                    break;

                case { phase: InputActionPhase.Performed }:
                    if (!CanPlayAnimationFire())
                        break;
                    if (equippedWeapon.HasAmmunition())
                    {
                        if (equippedWeapon.IsAutomatic())
                        {
                            shotsFired = 0;
                            break;
                        }

                        if (Time.time - lastShotTime > 60.0f / equippedWeapon.GetRateOfFire())
                            Fire();
                    }
                    else
                        FireEmpty();

                    break;

                case { phase: InputActionPhase.Canceled }:
                    holdingButtonFire = false;
                    shotsFired = 0;
                    break;
            }
        }

        public void OnTryPlayReload(InputAction.CallbackContext context)
        {
            if (!cursorLocked)
                return;

            if (!CanPlayAnimationReload())
                return;

            switch (context)
            {
                case { phase: InputActionPhase.Performed }:
                    PlayReloadAnimation();
                    break;
            }
        }

        public void OnTryInspect(InputAction.CallbackContext context)
        {
            if (!cursorLocked)
                return;

            if (!CanPlayAnimationInspect())
                return;

            switch (context)
            {
                case { phase: InputActionPhase.Performed }:
                    Inspect();
                    break;
            }
        }

        public void OnTryAiming(InputAction.CallbackContext context)
        {
            if (!cursorLocked)
                return;

            switch (context.phase)
            {
                case InputActionPhase.Started:
                    if (holdToAim)
                        holdingButtonAim = true;
                    break;
                case InputActionPhase.Performed:
                    if (!holdToAim)
                        holdingButtonAim = !holdingButtonAim;
                    break;
                case InputActionPhase.Canceled:
                    if (holdToAim)
                        holdingButtonAim = false;
                    break;
            }
        }

        public void OnTryHolster(InputAction.CallbackContext context)
        {
            if (!cursorLocked)
                return;

            if (!CanPlayAnimationHolster())
                return;

            switch (context.phase)
            {
                case InputActionPhase.Started:
                    if (holstered)
                    {
                        SetHolstered(false);
                        holstering = true;
                    }

                    break;
                case InputActionPhase.Performed:
                    SetHolstered(!holstered);
                    holstering = true;
                    break;
            }
        }

        public void OnTryThrowGrenade(InputAction.CallbackContext context)
        {
            if (!cursorLocked)
                return;

            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    if (CanPlayAnimationGrenadeThrow())
                        PlayGrenadeThrow();
                    break;
            }
        }

        public void OnTryMelee(InputAction.CallbackContext context)
        {
            if (!cursorLocked)
                return;

            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    if (CanPlayAnimationMelee())
                        PlayMelee();
                    break;
            }
        }

        public void OnTryRun(InputAction.CallbackContext context)
        {
            if (!cursorLocked)
                return;

            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    if (!holdToRun)
                        holdingButtonRun = !holdingButtonRun;
                    break;
                case InputActionPhase.Started:
                    if (holdToRun)
                        holdingButtonRun = true;
                    break;
                case InputActionPhase.Canceled:
                    if (holdToRun)
                        holdingButtonRun = false;
                    break;
            }
        }

        public void OnTryJump(InputAction.CallbackContext context)
        {
            if (!cursorLocked)
                return;

            switch (context.phase)
            {
                case InputActionPhase.Performed:
                    movementBehaviour.Jump();
                    break;
            }
        }

        public void OnTryInventoryNext(InputAction.CallbackContext context)
        {
            if (!cursorLocked)
                return;

            if (inventory == null)
                return;

            switch (context)
            {
                case { phase: InputActionPhase.Performed }:
                    //Get the index increment direction for our inventory using the scroll wheel direction. If we're not
                    //actually using one, then just increment by one.
                    var scrollValue = context.valueType.IsEquivalentTo(typeof(Vector2))
                        ? Mathf.Sign(context.ReadValue<Vector2>().y)
                        : 1.0f;

                    //Get the next index to switch to.
                    var indexNext = scrollValue > 0 ? inventory.GetNextIndex() : inventory.GetLastIndex();
                    //Get the current weapon's index.
                    var indexCurrent = inventory.GetEquippedIndex();

                    //Make sure we're allowed to change, and also that we're not using the same index, otherwise weird things happen!
                    if (CanChangeWeapon() && (indexCurrent != indexNext))
                        StartCoroutine(nameof(Equip), indexNext);
                    break;
            }
        }

        private void UpdateCursorState()
        {
            Cursor.visible = !cursorLocked;
            Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                Time.timeScale = 1;
                if (AudioManager.Instance)
                    AudioManager.Instance.UnPause();
            }
        }

        public void OnLockCursor(InputAction.CallbackContext context)
        {
            if (!CanPause)
                return;
            switch (context)
            {
                case { phase: InputActionPhase.Performed }:
                    cursorLocked = !cursorLocked;
                    UpdateCursorState();
                    break;
            }
        }


        public void OnLockCursor()
        {
            if (!CanPause)
                return;
            cursorLocked = !cursorLocked;
            UpdateCursorState();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            axisMovement = cursorLocked ? context.ReadValue<Vector2>() : default;
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            axisLook = cursorLocked ? context.ReadValue<Vector2>() : default;

            //Make sure that we have a weapon.
            if (equippedWeapon == null)
                return;

            //Make sure that we have a scope.
            if (equippedWeaponScope == null)
                return;

            //If we're aiming, multiply by the mouse sensitivity multiplier of the equipped weapon's scope!
            axisLook *= aiming ? equippedWeaponScope.GetMultiplierMouseSensitivity() : 1.0f;
        }

        public void OnUpdateTutorial(InputAction.CallbackContext context)
        {
            //Switch.
            tutorialTextVisible = context switch
            {
                //Started. Show the tutorial.
                { phase: InputActionPhase.Started } => true,
                //Canceled. Hide the tutorial.
                { phase: InputActionPhase.Canceled } => false,
                //Default.
                _ => tutorialTextVisible
            };
        }

        #endregion

        #region ANIMATION EVENTS

        public override void EjectCasing()
        {
            if (equippedWeapon != null)
                equippedWeapon.EjectCasing();
        }

        public override void FillAmmunition(int amount)
        {
            //Notify the weapon to fill the ammunition by the amount.
            if (equippedWeapon != null)
            {
                equippedWeapon.FillAmmunition(amount);
                characterAnimator.SetBool(AHashes.Reloading,
                    reloading = ((Inventory)inventory).CheckAmmo(((Weapon)equippedWeapon).AmmoType));
            }
        }

        public override void Grenade()
        {
            //Make sure that the grenade is valid, otherwise we'll get errors.
            if (grenadePrefab == null)
                return;

            if (cameraWorld == null)
                return;

            if (!grenadesUnlimited)
                grenadeCount--;

            var cTransform = cameraWorld.transform;
            var position = cTransform.position;
            position += cTransform.forward * grenadeSpawnOffset;
            Instantiate(grenadePrefab, position, cTransform.rotation);
        }

        public override void SetActiveMagazine(int active)
        {
            //Set magazine gameObject active.
            equippedWeaponMagazine.gameObject.SetActive(active != 0);
        }

        public override void AnimationEndedBolt()
        {
            UpdateBolt(false);
        }

        public override void AnimationEndedReload()
        {
            reloading = false;
        }

        public override void AnimationEndedGrenadeThrow()
        {
            throwingGrenade = false;
        }

        public override void AnimationEndedMelee()
        {
            meleeing = false;
        }

        public override void AnimationEndedInspect()
        {
            inspecting = false;
        }

        public override void AnimationEndedHolster()
        {
            holstering = false;
        }

        public override void SetSlideBack(int back)
        {
            if (equippedWeapon != null)
                equippedWeapon.SetSlideBack(back);
        }

        public override void SetActiveKnife(int active)
        {
            knife.SetActive(active != 0);
        }

        #endregion
    }
}