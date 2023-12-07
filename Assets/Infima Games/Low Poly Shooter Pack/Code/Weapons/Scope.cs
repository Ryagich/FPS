using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    public class Scope : ScopeBehaviour
    {
        #region FIELDS SERIALIZED
        [Title(label: "Multipliers")]
        [Tooltip("Amount to multiply the mouse sensitivity by while aiming through this scope.")]
        [SerializeField]
        private float multiplierMouseSensitivity = 0.8f;

        [Tooltip("Value multiplied by the weapon's spread while aiming through this scope.")] [SerializeField]
        private float multiplierSpread = 0.1f;

        [Title(label: "Interface")] [Tooltip("Interface Sprite.")] [SerializeField]
        private Sprite sprite;

        [Title(label: "Sway")]
        [Tooltip("The value to multiply the weapon sway by while aiming through this scope.")]
        [SerializeField]
        private float swayMultiplier = 1.0f;

        [Title(label: "Aiming Offset")] [Tooltip("Weapon bone location offset while aiming.")] [SerializeField]
        private Vector3 offsetAimingLocation;

        [Tooltip("Weapon bone rotation offset while aiming.")] [SerializeField]
        private Vector3 offsetAimingRotation;

        [Title(label: "Field Of View")] [Tooltip("Field Of View Multiplier Aim.")] [SerializeField]
        private float fieldOfViewMultiplierAim = 0.9f;

        [Tooltip("Field Of View Multiplier Aim Weapon.")] [SerializeField]
        private float fieldOfViewMultiplierAimWeapon = 0.7f;

        [Title(label: "Materials")]
        [Tooltip("The index of the scope material that gets hidden when we don't aim.")]
        [SerializeField]
        private int materialIndex = 3;

        [Tooltip("Material to block the scope while not aiming through it.")] [SerializeField]
        private Material materialHidden;
        #endregion

        #region FIELDS
        private MeshRenderer meshRenderer;

        /// <summary>
        /// Default scope material. We store it so we can re-apply it at any time, since it is
        /// usually changed at runtime.
        /// </summary>
        private Material materialDefault;
        private Camera camera;
        #endregion

        #region UNITY
        private void Awake()
        {
            //Cache Scope Renderer.
            meshRenderer = GetComponentInChildren<MeshRenderer>();

            //Make sure that the index can exist.
            if (!HasMaterialIndex())
                return;

            //Cache default material.
            materialDefault = meshRenderer.materials[materialIndex];
            camera = GetComponentInChildren<Camera>();
        }

        private void Start()
        {
            //Start at the default state.
            OnAimStop();
        }
        #endregion

        #region GETTERS
        public override float GetMultiplierMouseSensitivity() => multiplierMouseSensitivity;
        public override float GetMultiplierSpread() => multiplierSpread;
        public override Vector3 GetOffsetAimingLocation() => offsetAimingLocation;
        public override Vector3 GetOffsetAimingRotation() => offsetAimingRotation;
        public override float GetFieldOfViewMultiplierAim() => fieldOfViewMultiplierAim;
        public override float GetFieldOfViewMultiplierAimWeapon() => fieldOfViewMultiplierAimWeapon;
        public override Sprite GetSprite() => sprite;
        public override float GetSwayMultiplier() => swayMultiplier;

        private bool HasMaterialIndex()
        {
            if (meshRenderer == null)
                return false;

            return materialIndex < meshRenderer.materials.Length && materialIndex >= 0;
        }
        #endregion

        #region METHODS
        public override void OnAim()
        {
            if (!HasMaterialIndex())
                return;
            
            camera.gameObject.SetActive(true);
            Material[] materials = meshRenderer.materials;
            materials[materialIndex] = materialDefault;
            meshRenderer.materials = materials;
        }

        public override void OnAimStop()
        {
            //Make sure that the index can exist.
            if (!HasMaterialIndex())
                return;
            
            camera.gameObject.SetActive(false);
            Material[] materials = meshRenderer.materials;
            materials[materialIndex] = materialHidden;
            meshRenderer.materials = materials;
        }
        #endregion
    }
}