﻿using UnityEngine;
using YG;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Camera Look. Handles the rotation of the camera.
    /// </summary>
    public class CameraLook : MonoBehaviour
    {
        #region FIELDS SERIALIZED

        [Title(label: "Settings")] [Tooltip("Sensitivity when looking around.")] [SerializeField]
        private Vector2 sensitivity = new Vector2(1, 1);

        [Tooltip("Minimum and maximum up/down rotation angle the camera can have.")] [SerializeField]
        private Vector2 yClamp = new Vector2(-60, 60);

        [Title(label: "Interpolation")] [Tooltip("Should the look rotation be interpolated?")] [SerializeField]
        private bool smooth;

        [Tooltip("The speed at which the look rotation is interpolated.")] [SerializeField]
        private float interpolationSpeed = 25.0f;

        #endregion

        #region FIELDS

        private CharacterBehaviour playerCharacter;
        private Rigidbody playerCharacterRigidbody;
        private Quaternion rotationCharacter;
        private Quaternion rotationCamera;

        #endregion

        #region UNITY

        private void Start()
        {
            //Get Player Character.
            playerCharacter = ServiceLocator.Current.Get<IGameModeService>().GetPlayerCharacter();

            //Cache the character's initial rotation.
            rotationCharacter = playerCharacter.transform.localRotation;
            //Cache the camera's initial rotation.
            rotationCamera = transform.localRotation;


                //if (YandexGame.SDKEnabled)
                // {
                //     SetSensitivity(YandexGame.savesData.SensitivityX,
                //         YandexGame.savesData.SensitivityY);
                //  }
                // else
                //  {
                //     YandexGame.GetDataEvent += () =>
                //          SetSensitivity(YandexGame.savesData.SensitivityX,
                //              YandexGame.savesData.SensitivityY);
                //  }
        }

        public void SetSensitivity(float x, float y)
        {
            sensitivity = new Vector2(x, y);
        }

        private void LateUpdate()
        {
            SetSensitivity(YandexGame.savesData.SensitivityX,
                YandexGame.savesData.SensitivityY);
            //Frame Input. The Input to add this frame!
            var frameInput = playerCharacter.IsCursorLocked() ? playerCharacter.GetInputLook() : default;
            //Sensitivity.
            frameInput *= sensitivity;

            var rotationYaw = Quaternion.Euler(0.0f, frameInput.x, 0.0f);
            var rotationPitch = Quaternion.Euler(-frameInput.y, 0.0f, 0.0f);

            //Save rotation. We use this for smooth rotation.
            rotationCamera *= rotationPitch;
            rotationCamera = Clamp(rotationCamera);
            rotationCharacter *= rotationYaw;

            var localRotation = transform.localRotation;

            if (smooth)
            {
                // Interpolate local rotation.
                localRotation = Quaternion.Slerp(localRotation, rotationCamera, Time.deltaTime * interpolationSpeed);
                //Clamp.
                localRotation = Clamp(localRotation);
                //Interpolate character rotation.
                playerCharacter.transform.rotation = Quaternion.Slerp(playerCharacter.transform.rotation,
                    rotationCharacter, Time.deltaTime * interpolationSpeed);
            }
            else
            {
                //Rotate local.
                localRotation *= rotationPitch;
                //Clamp.
                localRotation = Clamp(localRotation);

                //Rotate character.
                playerCharacter.transform.rotation *= rotationYaw;
            }

            //Set.
            transform.localRotation = localRotation;
        }

        #endregion

        #region FUNCTIONS

        /// <summary>
        /// Clamps the pitch of a quaternion according to our clamps.
        /// </summary>
        private Quaternion Clamp(Quaternion rotation)
        {
            rotation.x /= rotation.w;
            rotation.y /= rotation.w;
            rotation.z /= rotation.w;
            rotation.w = 1.0f;

            //Pitch.
            var pitch = 2.0f * Mathf.Rad2Deg * Mathf.Atan(rotation.x);

            //Clamp.
            pitch = Mathf.Clamp(pitch, yClamp.x, yClamp.y);
            rotation.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * pitch);

            //Return.
            return rotation;
        }

        #endregion
    }
}