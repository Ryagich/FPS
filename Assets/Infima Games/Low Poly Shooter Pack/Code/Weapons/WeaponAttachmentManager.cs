//Copyright 2022, Infima Games. All Rights Reserved.

using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Weapon Attachment Manager. Handles equipping and storing a Weapon's Attachments.
    /// </summary>
    public class WeaponAttachmentManager : WeaponAttachmentManagerBehaviour
    {
        #region FIELDS SERIALIZED

        [Title(label: "Scope")]
        [Tooltip("Determines if the ironsights should be shown on the weapon model.")]
        [SerializeField]
        private bool scopeDefaultShow = true;

        [Tooltip("Default Scope!")] [SerializeField]
        private ScopeBehaviour scopeDefaultBehaviour;

        [Tooltip(
            "Selected Scope Index. If you set this to a negative number, ironsights will be selected as the enabled scope.")]
        [SerializeField]
        private int scopeIndex = -1;
        [Tooltip("First scope index when using random scopes.")] [SerializeField]
        private int scopeIndexFirst = -1;
        [Tooltip("Should we pick a random index when starting the game?")] [SerializeField]
        private bool scopeIndexRandom;
        [Tooltip("All possible Scope Attachments that this Weapon can use!")] [SerializeField]
        private ScopeBehaviour[] scopeArray;
        public ScopeBehaviour[] GetScopes => scopeArray;
        

        [Title(label: "Muzzle")] [Tooltip("Selected Muzzle Index.")] [SerializeField]
        private int muzzleIndex;

        [Tooltip("Should we pick a random index when starting the game?")] [SerializeField]
        private bool muzzleIndexRandom = true;

        [Tooltip("All possible Muzzle Attachments that this Weapon can use!")] [SerializeField]
        private MuzzleBehaviour[] muzzleArray;
        public MuzzleBehaviour[] GetMuzzle => muzzleArray;
        

        [Title(label: "Laser")] [Tooltip("Selected Laser Index.")] [SerializeField]
        private int laserIndex = -1;

        [Tooltip("Should we pick a random index when starting the game?")] [SerializeField]
        private bool laserIndexRandom = true;

        [Tooltip("All possible Laser Attachments that this Weapon can use!")] [SerializeField]
        private LaserBehaviour[] laserArray;
        public LaserBehaviour[] GetLaser => laserArray;
        

        [Title(label: "Grip")] [Tooltip("Selected Grip Index.")] [SerializeField]
        private int gripIndex = -1;

        [Tooltip("Should we pick a random index when starting the game?")] [SerializeField]
        private bool gripIndexRandom = true;

        [Tooltip("All possible Grip Attachments that this Weapon can use!")] [SerializeField]
        private GripBehaviour[] gripArray;
        public GripBehaviour[] GetGrip => gripArray;
        

        [Title(label: "Magazine")] [Tooltip("Selected Magazine Index.")] [SerializeField]
        private int magazineIndex;

        [Tooltip("Should we pick a random index when starting the game?")] [SerializeField]
        private bool magazineIndexRandom = true;

        [Tooltip("All possible Magazine Attachments that this Weapon can use!")] [SerializeField]
        private Magazine[] magazineArray;
        #endregion

        #region FIELDS
        private ScopeBehaviour scopeBehaviour;
        private MuzzleBehaviour muzzleBehaviour;
        private LaserBehaviour laserBehaviour;
        private GripBehaviour gripBehaviour;
        private MagazineBehaviour magazineBehaviour;
        #endregion

        #region UNITY FUNCTIONS
        public int GetScopeIndex => scopeIndex;
        public int GetMuzzleIndex => muzzleIndex;
        public int GetLaserIndex => laserIndex;
        public int GetGripIndex => gripIndex;

        public void SetScope(int index)
        {
            scopeIndex = index;
            scopeBehaviour = scopeArray.SelectAndSetActive(scopeIndex);
            if (scopeBehaviour == null && scopeIndex == -1)
            {
                //Select Default Scope.
                scopeBehaviour = scopeDefaultBehaviour;
                //Set Active.
                scopeBehaviour.gameObject.SetActive(true);
            }
            else
            {
                scopeDefaultBehaviour.gameObject.SetActive(false);
            }
        }

        public void SetMuzzle(int index)
        {
            muzzleIndex = index;
            muzzleBehaviour = muzzleArray.SelectAndSetActive(muzzleIndex);
        }

        public void SetLaser(int index)
        {
            laserIndex = index;
            laserBehaviour = laserArray.SelectAndSetActive(laserIndex);
        }

        public void SetGrip(int index)
        {
            gripIndex = index;
            gripBehaviour = gripArray.SelectAndSetActive(gripIndex);
        }

        protected override void Awake()
        {
            if (scopeIndexRandom)
                scopeIndex = Random.Range(scopeIndexFirst, scopeArray.Length);
            scopeBehaviour = scopeArray.SelectAndSetActive(scopeIndex);
            if (scopeBehaviour == null && scopeIndex == -1)
            {
                scopeBehaviour = scopeDefaultBehaviour;
                scopeBehaviour.gameObject.SetActive(scopeDefaultShow);
            }
            if (muzzleIndexRandom)
                muzzleIndex = Random.Range(0, muzzleArray.Length);
            muzzleBehaviour = muzzleArray.SelectAndSetActive(muzzleIndex);
            if (laserIndexRandom)
                laserIndex = Random.Range(0, laserArray.Length);
            laserBehaviour = laserArray.SelectAndSetActive(laserIndex);
            if (gripIndexRandom)
                gripIndex = Random.Range(0, gripArray.Length);
            gripBehaviour = gripArray.SelectAndSetActive(gripIndex);
            if (magazineIndexRandom)
                magazineIndex = Random.Range(0, magazineArray.Length);
            magazineBehaviour = magazineArray.SelectAndSetActive(magazineIndex);
        }
        #endregion

        #region GETTERS
        public override ScopeBehaviour GetEquippedScope() => scopeBehaviour;
        public override ScopeBehaviour GetEquippedScopeDefault() => scopeDefaultBehaviour;
        public override MagazineBehaviour GetEquippedMagazine() => magazineBehaviour;
        public override MuzzleBehaviour GetEquippedMuzzle() => muzzleBehaviour;
        public override LaserBehaviour GetEquippedLaser() => laserBehaviour;
        public override GripBehaviour GetEquippedGrip() => gripBehaviour;
        #endregion
    }
}