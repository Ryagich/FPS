using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using YG;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Movement. This is our main, and base, component that handles the character's movement.
    /// It contains all of the logic relating to moving, running, crouching, jumping...etc
    /// </summary>
    public class Movement : MovementBehaviour
    {
        #region FIELDS SERIALIZED

        [Title(label: "Acceleration")] [Tooltip("How fast the character's speed increases.")] [SerializeField]
        private float acceleration = 9.0f;

        [Tooltip("Acceleration value used when the character is in the air. This means either jumping, or falling.")]
        [SerializeField]
        private float accelerationInAir = 3.0f;

        [Tooltip("How fast the character's speed decreases.")] [SerializeField]
        private float deceleration = 11.0f;

        [Title(label: "Speeds")] [SerializeField]
        private float _speedWalking = 4.0f;

        [SerializeField] private float _speedAiming = 3.2f;
        [SerializeField] private float _speedCrouching = 3.5f;
        [SerializeField] private float _speedRunning = 6.8f;

        [Title(label: "Walking Multipliers")]
        [Tooltip("Value to multiply the walking speed by when the character is moving forward."), SerializeField]
        [Range(0.0f, 1.0f)]
        private float _walkingMultiplierForward = 1.0f;

        [Tooltip("Value to multiply the walking speed by when the character is moving sideways.")]
        [Range(0.0f, 1.0f)]
        [SerializeField]
        private float _walkingMultiplierSideways = 1.0f;

        [Tooltip("Value to multiply the walking speed by when the character is moving backwards.")]
        [Range(0.0f, 1.0f)]
        [SerializeField]
        private float _walkingMultiplierBackwards = 1.0f;

        [Title(label: "Air")]
        [Tooltip("How much control the player has over changes in direction while the character is in the air.")]
        [Range(0.0f, 1.0f)]
        [SerializeField]
        private float airControl = 0.8f;

        [Tooltip("The value of the character's gravity. Basically, defines how fast the character falls.")]
        [SerializeField]
        private float gravity = 1.1f;

        [Tooltip("The value of the character's gravity while jumping.")] [SerializeField]
        private float jumpGravity = 1.0f;

        [Tooltip("The force of the jump.")] [SerializeField]
        private float jumpForce = 100.0f;

        [Tooltip("Force applied to keep the character from flying away while descending slopes.")] [SerializeField]
        private float stickToGroundForce = 0.03f;

        [Title(label: "Crouching")]
        [Tooltip("Setting this to false will always block the character from crouching.")]
        [SerializeField]
        private bool canCrouch = true;

        [Tooltip("If true, the character will be able to crouch/un-crouch while falling, which can lead to " +
                 "some slightly interesting results.")]
        [SerializeField, ShowIf(nameof(canCrouch), true)]
        private bool canCrouchWhileFalling = false;

        [Tooltip("If true, the character will be able to jump while crouched too!")]
        [SerializeField, ShowIf(nameof(canCrouch), true)]
        private bool canJumpWhileCrouching = true;

        [Tooltip("Height of the character while crouching.")] [SerializeField, ShowIf(nameof(canCrouch), true)]
        private float crouchHeight = 1.0f;

        [Tooltip("Mask of possible layers that can cause overlaps when trying to un-crouch. Very important!")]
        [SerializeField, ShowIf(nameof(canCrouch), true)]
        private LayerMask crouchOverlapsMask;

        [Title(label: "Rigidbody Push")]
        [Tooltip(
            "Force applied to other rigidbodies when walking into them. This force is multiplied by the character's " +
            "velocity, so it is never applied by itself, that's important to note.")]
        [SerializeField]
        private float rigidbodyPushForce = 1.0f;

        #endregion

        private CharacterController controller;
        private CharacterBehaviour playerCharacter;
        private WeaponBehaviour equippedWeapon;
        private float standingHeight;
        private Vector3 velocity;
        private bool isGrounded;


        private bool wasGrounded;
        private bool jumping;
        private bool crouching;
        private float lastJumpTime;

        #region UNITY FUNCTIONS

        protected override void Awake()
        {
            playerCharacter = ServiceLocator.Current.Get<IGameModeService>().GetPlayerCharacter();
            if (YandexGame.savesData.speedWalking is -1)
            {
                switch (YandexGame.savesData.CharacterIndex)
                {
                    case 0:
                        SetMovement(.8f);
                        break;
                    case 1:
                        SetMovement(1f);
                        break;
                    case 2:
                        SetMovement(1.5f);
                        break;
                }
                ApplySpeedFactor(1);
            }
        }

        protected override void Start()
        {
            controller = GetComponent<CharacterController>();
            standingHeight = controller.height;
        }

        public void SetMovement(float value)
        {
            _speedWalking *= value;
            _speedAiming *= value;
            _speedCrouching *= value;
            _speedRunning *= value;
        }

        /// Moves the camera to the character, processes jumping and plays sounds every frame.
        protected override void Update()
        {
            //Get the equipped weapon!
            equippedWeapon = playerCharacter.GetInventory().GetEquipped();

            //Get this frame's grounded value.
            isGrounded = IsGrounded();
            //Check if it has changed from last frame.
            if (isGrounded && !wasGrounded)
            {
                //Set jumping.
                jumping = false;
                //Set lastJumpTime.
                lastJumpTime = 0.0f;
            }
            else if (wasGrounded && !isGrounded)
                lastJumpTime = Time.time;

            MoveCharacter();
            //Save the grounded value to check for difference next frame.
            wasGrounded = isGrounded;
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            //Zero out the upward velocity if the character hits the ceiling.
            if (hit.moveDirection.y > 0.0f && velocity.y > 0.0f)
                velocity.y = 0.0f;

            //We need a rigidbody to push the object we just hit.
            var hitRigidbody = hit.rigidbody;
            if (hitRigidbody == null)
                return;

            //AddForce.
            var force = (hit.moveDirection + Vector3.up * 0.35f) * velocity.magnitude * rigidbodyPushForce;
            hitRigidbody.AddForceAtPosition(force, hit.point);
        }

        #endregion

        #region METHODS

        public void ApplySpeedFactor(float factor)
        {
            YandexGame.savesData.speedWalking += factor * _speedWalking;
            YandexGame.savesData.speedAiming += factor * _speedAiming;
            YandexGame.savesData.speedCrouching += factor * _speedCrouching;
            YandexGame.savesData.speedRunning += factor * _speedRunning;
            //??
            //YandexGame.SaveProgress();
        }

        private Coroutine addedItemSpeedCor;
        
        public void ApplyAddedSpeedForItem()
        {
            if (addedItemSpeedCor is not null)
            {
                StopCoroutine(addedItemSpeedCor);
                ApplySpeedFactor(-.2f);
            }
            addedItemSpeedCor = StartCoroutine(HoldAddedItemSpeed());
        }

        private IEnumerator HoldAddedItemSpeed()
        {
            ApplySpeedFactor(.2f);
            yield return new WaitForSeconds(2);
            ApplySpeedFactor(-.2f);
        }
        private void MoveCharacter()
        {
            var speedWalking = YandexGame.savesData.speedWalking;
            var speedAiming = YandexGame.savesData.speedAiming;
            var speedCrouching = YandexGame.savesData.speedCrouching;
            var speedRunning =  YandexGame.savesData.speedRunning;
            
            var talents = YandexGame.savesData.Talents;
            var sc = StatsController.Instance;
            if (talents[5] && sc.Armor.Value is 0)
            {
                speedWalking +=  .5f * _speedWalking;
                speedAiming += .5f * _speedAiming;
                speedCrouching += .5f * _speedCrouching;
                speedRunning += .5f * _speedRunning;
            }
            if (talents[12])
            {
                var m = (int)(10*(sc.Armor.Value / sc.Armor.Max)) * 2;
                speedWalking += (m / 100) * _speedWalking;
                speedAiming += (m / 100) * _speedAiming;
                speedCrouching += (m / 100) * _speedCrouching;
                speedRunning += (m / 100) * _speedRunning;
            }
            
            var frameInput = Vector3.ClampMagnitude(playerCharacter.GetInputMovement(), 1.0f);
            //Calculate local-space direction by using the player's input.
            var desiredDirection = new Vector3(frameInput.x, 0.0f, frameInput.y);

            //Running speed calculation.
            if (playerCharacter.IsRunning())
                desiredDirection *= speedRunning;
            else
            {
                if (crouching)
                    desiredDirection *= speedCrouching;
                else
                {
                    //Aiming speed calculation.
                    if (playerCharacter.IsAiming())
                        desiredDirection *= speedAiming;
                    else
                    {
                        //Multiply by the normal walking speed.
                        desiredDirection *= speedWalking;
                        //Multiply by the sideways multiplier, to get better feeling sideways movement.
                        desiredDirection.x *= _walkingMultiplierSideways;
                        //Multiply by the forwards and backwards multiplier.
                        desiredDirection.z *=
                            (frameInput.y > 0 ? _walkingMultiplierForward : _walkingMultiplierBackwards);
                    }
                }
            }

            //World space velocity calculation.
            desiredDirection = transform.TransformDirection(desiredDirection);
            //Multiply by the weapon movement speed multiplier. This helps us modify speeds based on the weapon!
            if (equippedWeapon != null)
                desiredDirection *= equippedWeapon.GetMultiplierMovementSpeed();

            //Apply gravity!
            if (isGrounded == false)
            {
                //Get rid of any upward velocity.
                if (wasGrounded && !jumping)
                    velocity.y = 0.0f;

                //Movement.
                velocity += desiredDirection * (accelerationInAir * airControl * Time.deltaTime);
                //Gravity.
                velocity.y -= (velocity.y >= 0 ? jumpGravity : gravity) * Time.deltaTime;
            }
            //Normal Movement On Ground.
            else if (!jumping)
            {
                //Update velocity with movement on the ground values.
                velocity = Vector3.Lerp(velocity, new Vector3(desiredDirection.x, velocity.y, desiredDirection.z),
                    Time.deltaTime * (desiredDirection.sqrMagnitude > 0.0f ? acceleration : deceleration));
            }

            //Velocity Applied.
            var applied = velocity * Time.deltaTime;
            //Stick To Ground Force. Helps with making the character walk down slopes without floating.
            if (controller.isGrounded && !jumping)
                applied.y = -stickToGroundForce;

            controller.Move(applied);
        }

        public override bool WasGrounded() => wasGrounded;
        public override bool IsJumping() => jumping;

        public override bool CanCrouch(bool newCrouching)
        {
            //Always block crouching if we need to.
            if (canCrouch == false)
                return false;

            //If we're in the air, and we cannot crouch while in the air, then we can ignore this execution!
            if (isGrounded == false && canCrouchWhileFalling == false)
                return false;

            //The controller can always crouch, the issue is un-crouching!
            if (newCrouching)
                return true;

            //Overlap check location.
            var sphereLocation = transform.position + Vector3.up * standingHeight;
            //Check for any overlaps.
            return (Physics.OverlapSphere(sphereLocation, controller.radius, crouchOverlapsMask).Length == 0);
        }

        public override bool IsCrouching() => crouching;

        public override void Jump()
        {
            //We can ignore this if we're crouching and we're not allowed to do crouch-jumps.
            if (crouching && !canJumpWhileCrouching)
                return;

            //Block jumping when we're not grounded. This avoids us double jumping.
            if (!isGrounded)
                return;

            //Jump.
            jumping = true;
            //Apply Jump Velocity.
            velocity = new Vector3(velocity.x, Mathf.Sqrt(2.0f * jumpForce * jumpGravity), velocity.z);

            //Save lastJumpTime.
            lastJumpTime = Time.time;
        }

        /// <summary>
        /// Changes the controller's capsule height.
        /// </summary>
        public override void Crouch(bool newCrouching)
        {
            //Set the new crouching value.
            crouching = newCrouching;

            //Update the capsule's height.
            controller.height = crouching ? crouchHeight : standingHeight;
            //Update the capsule's center.
            controller.center = controller.height / 2.0f * Vector3.up;
        }

        public override void TryCrouch(bool value)
        {
            if (value && CanCrouch(true))
                Crouch(true);
            //Coroutine Un-Crouch.
            else if (!value)
                StartCoroutine(nameof(TryUncrouch));
        }

        public override void TryToggleCrouch() => TryCrouch(!crouching);

        /// <summary>
        /// Tries to un-crouch the character.
        /// </summary>
        private IEnumerator TryUncrouch()
        {
            //If the movementBehaviour says that we can't go into whatever crouching state is the opposite, then
            //the character will have to forget about it, no way around it bois!
            yield return new WaitUntil(() => CanCrouch(false));

            //Un-Crouch.
            Crouch(false);
        }

        #endregion

        #region GETTERS

        public override float GetLastJumpTime() => lastJumpTime;
        public override float GetMultiplierForward() => _walkingMultiplierForward;
        public override float GetMultiplierSideways() => _walkingMultiplierSideways;
        public override float GetMultiplierBackwards() => _walkingMultiplierBackwards;
        public override Vector3 GetVelocity() => controller.velocity;
        public override bool IsGrounded() => controller.isGrounded;

        #endregion
    }
}