using System.Linq;
using InfimaGames.LowPolyShooterPack.Legacy;
using UnityEngine;
using YG;

namespace InfimaGames.LowPolyShooterPack
{
    public class Weapon : WeaponBehaviour
    {
        [HideInInspector] public int ammunitionCurrent;
        
        #region FIELDS SERIALIZED   
        
        [Title(label: "Settings")] [SerializeField]
        private string weaponName;

        [SerializeField] private float multiplierMovementSpeed = 1.0f;

        [Title(label: "Firing")]
        [Tooltip("Is this weapon automatic? If yes, then holding down the firing button will continuously fire.")]
        [SerializeField]
        private bool automatic;

        [Tooltip("Is this weapon bolt-action? If yes, then a bolt-action animation will play after every shot.")]
        [SerializeField]
        private bool boltAction;

        [Tooltip(
            "Amount of shots fired at once. Helpful for things like shotguns, where there are multiple projectiles fired at once.")]
        [SerializeField]
        private int shotCount = 1;

        [Tooltip("How far the weapon can fire from the center of the screen.")] [SerializeField]
        private float spread = 0.25f;

        public float Spread => spread;
        [Tooltip("How fast the projectiles are.")] [SerializeField]
        private float projectileImpulse = 400.0f;

        [Tooltip("Amount of shots this weapon can shoot in a minute. It determines how fast the weapon shoots.")]
        [SerializeField]
        private int roundsPerMinutes = 200;

        [SerializeField] private float _damage = 40f;
        public float Damage => _damage;

        [Title(label: "Reloading")]
        [Tooltip("Determines if this weapon reloads in cycles, meaning that it inserts one bullet at a time, or not.")]
        [SerializeField]
        private bool cycledReload;

        [Tooltip("Determines if the player can reload this weapon when it is full of ammunition.")] [SerializeField]
        private bool canReloadWhenFull = true;

        [Tooltip("Should this weapon be reloaded automatically after firing its last shot?")] [SerializeField]
        private bool automaticReloadOnEmpty;

        [Tooltip("Time after the last shot at which a reload will automatically start.")] [SerializeField]
        private float automaticReloadOnEmptyDelay = 0.25f;

        [Title(label: "Animation")]
        [Tooltip(
            "Transform that represents the weapon's ejection port, meaning the part of the weapon that casings shoot from.")]
        [SerializeField]
        private Transform socketEjection;

        [Tooltip("Settings this to false will stop the weapon from being reloaded while the character is aiming it.")]
        [SerializeField]
        private bool canReloadAimed = true;

        [Title(label: "Resources")] [Tooltip("Casing Prefab.")] [SerializeField]
        private GameObject prefabCasing;

        [Tooltip("Projectile Prefab. This is the prefab spawned when the weapon shoots.")] [SerializeField]
        private GameObject prefabProjectile;

        [Tooltip("The AnimatorController a player character needs to use while wielding this weapon.")] [SerializeField]
        public RuntimeAnimatorController controller;

        [Tooltip("Weapon Body Texture.")] [SerializeField]
        private Sprite spriteBody;

        [Title(label: "Audio Clips Holster")] [Tooltip("Holster Audio Clip.")] [SerializeField]
        private AudioClip audioClipHolster;

        [Tooltip("Unholster Audio Clip.")] [SerializeField]
        private AudioClip audioClipUnholster;

        [Title(label: "Audio Clips Reloads")] [Tooltip("Reload Audio Clip.")] [SerializeField]
        private AudioClip audioClipReload;

        [Tooltip("Reload Empty Audio Clip.")] [SerializeField]
        private AudioClip audioClipReloadEmpty;

        [Title(label: "Audio Clips Reloads Cycled")] [Tooltip("Reload Open Audio Clip.")] [SerializeField]
        private AudioClip audioClipReloadOpen;

        [Tooltip("Reload Insert Audio Clip.")] [SerializeField]
        private AudioClip audioClipReloadInsert;

        [Tooltip("Reload Close Audio Clip.")] [SerializeField]
        private AudioClip audioClipReloadClose;

        [Title(label: "Audio Clips Other")]
        [Tooltip("AudioClip played when this weapon is fired without any ammunition.")]
        [SerializeField]
        private AudioClip audioClipFireEmpty;

        [SerializeField] private AudioClip audioClipBoltAction;
        [field: SerializeField] public DropWeapon DropWeapon { get; private set; }
        [field: SerializeField] public WeaponType Type { get; private set; }
        [field: SerializeField] public Ammo AmmoType { get; private set; }

        #endregion

        private Animator animator;
        private WeaponAttachmentManagerBehaviour attachmentManager;
        private bool inited = false;
        private ScopeBehaviour scopeBehaviour;
        public MagazineBehaviour magazineBehaviour { get; private set; }
        private MuzzleBehaviour muzzleBehaviour;
        private LaserBehaviour laserBehaviour;
        private GripBehaviour gripBehaviour;

        private IGameModeService gameModeService;
        private CharacterBehaviour characterBehaviour;
        private Transform playerCamera;
        private int shootCounter;

        #region UNITY

        protected override void Awake()
        {
            animator = GetComponent<Animator>();
            attachmentManager = GetComponent<WeaponAttachmentManagerBehaviour>();

            gameModeService = ServiceLocator.Current.Get<IGameModeService>();
            characterBehaviour = gameModeService.GetPlayerCharacter();
            playerCamera = characterBehaviour.GetCameraWorld().transform;
        }


        public void Init()
        {
            if (inited)
                return;

            #region Cache Attachment References

            scopeBehaviour = attachmentManager.GetEquippedScope();
            magazineBehaviour = attachmentManager.GetEquippedMagazine();
            muzzleBehaviour = attachmentManager.GetEquippedMuzzle();
            laserBehaviour = attachmentManager.GetEquippedLaser();
            gripBehaviour = attachmentManager.GetEquippedGrip();

            #endregion

            if (ammunitionCurrent == 0)
                ammunitionCurrent = magazineBehaviour.GetAmmunitionTotal();
            inited = true;
            SetDamage();
        }

        public void SetDamage()
        {
            switch (YandexGame.savesData.CharacterIndex)
            {
                case 0:
                    if (Type is WeaponType.ShotGun or WeaponType.SMG)
                        _damage += _damage * .2f;
                    break;
                //+20% к урону и скорости использования и скорости стрельбы ПП и дробовиков.
                case 1:
                    if (Type is WeaponType.Pistol)
                        _damage += _damage * .5f;
                    break;
                //Урон пистолетов +50%.
                case 2:
                    if (Type is WeaponType.SniperRifle)
                        _damage += _damage * .2f;
                    break;
                //+20% урона снайперских винтовок.
            }

            _damage += _damage * .05f * (1 + YandexGame.savesData.Upgrades[1][0][0]);
            switch (Type)
            {
                case WeaponType.Pistol:
                    _damage += _damage * .1f * (1 + YandexGame.savesData.Upgrades[1][1][1]);
                    break;
                case WeaponType.SniperRifle or WeaponType.AutoRifle or WeaponType.SMG:
                    _damage += _damage * .1f * (1 + YandexGame.savesData.Upgrades[1][2][1]);
                    break;
                case WeaponType.ShotGun:
                    _damage += _damage * .1f * (1 + YandexGame.savesData.Upgrades[1][3][0]);
                    break;
            }

            _damage += _damage * YandexGame.savesData.DamageFactor;
        }

        private float GetDamage()
        {
            var talents = YandexGame.savesData.Talents;

            var damage = GetDefaultDamage(talents);
            if (Random.Range(.0f, 1f) <= GetCriticalDamageChange(talents))
            {
                return damage + GetAddCriticalDamage(talents, damage);
            }

            return damage + GetAddSimpleDamage(talents, damage);
        }

        private float GetCriticalDamageChange(bool[] talents)
        {
            var change = .0f;
            if (YandexGame.savesData.Upgrades[1][3][1] is not -1)
            {
                change += .1f + .05f * (1 + YandexGame.savesData.Upgrades[1][4][1]);
            }

            if (talents[17])
            {
                change += .01f * (int)(StatsController.Instance.NeedHealth / StatsController.Instance.Hp.Max / 5);
            }

            if (talents[29])
            {
                change += .25f;
            }

            if (talents[55] && StatsController.Instance.TookDamageInLast5Seconds)
            {
                change += .2f;
            }

            if (talents[63])
            {
                change += .5f;
                change = Mathf.Clamp(change, 0, .75f);
            }

            return change;
        }

        private float GetDefaultDamage(bool[] talents)
        {
            shootCounter++;
            var damage = _damage;

            if (YandexGame.savesData.CharacterIndex is 1 && YandexGame.savesData.BootAddSpellBullets > 0)
            {
                damage += .2f * _damage;
                YandexGame.savesData.BootAddSpellBullets--;
            }
            
            if (shootCounter is 4)
            {
                shootCounter = 0;
                if (talents[1])
                {
                    damage += .2f * _damage;
                }
            }

            if (talents[3] && StatsController.Instance.IsHpMax)
            {
                damage += .25f * _damage;
            }

            if (talents[4] && ammunitionCurrent == magazineBehaviour.GetAmmunitionTotal() - 1)
            {
                damage += _damage;
            }

            if (talents[18])
            {
                damage += StatsController.Instance.IsArmorMax ? .75f : -.25f * _damage;
            }

            if (talents[27])
            {
                damage -= .5f * _damage;
            }

            if (talents[32])
            {
                damage += AddSpellController.Instance.SpellCount > 0 ? .75f : -.25f * _damage;
            }

            if (talents[33] && StatsController.Instance.TookDamageInLast5Seconds)
            {
                damage += .15f * _damage;
            }

            if (talents[52])
            {
                var max = magazineBehaviour.GetAmmunitionTotal();
                damage += Mathf.Clamp((max - ammunitionCurrent) / max, .0f, .75f) * _damage;
            }
            
            if (talents[58])
            {
                damage += _damage;
            }
            
            if (talents[60])
            {
                damage += YandexGame.savesData.Money / 100 * .05f * _damage;
            }

            return damage;
        }

        private float GetAddSimpleDamage(bool[] talents, float damage)
        {
            var addDamage = .0f;
            if (talents[0])
            {
                addDamage += damage * .2f;
            }

            if (talents[15])
            {
                addDamage += -.2f * damage;
            }

            return addDamage;
        }

        private float GetAddCriticalDamage(bool[] talents, float damage)
        {
            var addDamage = .0f;

            addDamage += damage * .5f * (1 + YandexGame.savesData.Upgrades[1][3][1]);
            addDamage += damage * .15f * (1 + YandexGame.savesData.Upgrades[1][4][0]);

            if (talents[15])
            {
                addDamage += .5f * damage;
            }

            return addDamage;
        }

        protected override void Start()
        {
            Init();
        }

        #endregion

        #region GETTERS

        public override float GetFieldOfViewMultiplierAim()
        {
            if (!inited)
                Init();
            //Make sure we don't have any issues even with a broken setup!
            if (scopeBehaviour != null)
                return scopeBehaviour.GetFieldOfViewMultiplierAim();
            Debug.LogError("Weapon has no scope equipped!");
            return 1.0f;
        }

        public override float GetFieldOfViewMultiplierAimWeapon()
        {
            if (!inited)
                Init();
            //Make sure we don't have any issues even with a broken setup!
            if (scopeBehaviour != null)
                return scopeBehaviour.GetFieldOfViewMultiplierAimWeapon();
            Debug.LogError("Weapon has no scope equipped!");

            return 1.0f;
        }

        #endregion

        public override void Reload()
        {
            var inventory = characterBehaviour.GetInventory() as Inventory;
            if (!inventory.CheckAmmo(AmmoType))
                return;
            //Set Reloading Bool. This helps cycled reloads know when they need to stop cycling.
            const string boolName = "Reloading";
            animator.SetBool(boolName, true);

            //Try Play Reload Sound.
            AudioManager.Instance.PlaySound
                (HasAmmunition() ? audioClipReload : audioClipReloadEmpty, AudioSourceType.Player);

            //Play Reload Animation.
            animator.Play(cycledReload
                ? "Reload Open"
                : (HasAmmunition()
                    ? "Reload"
                    : "Reload Empty"), 0, 0.0f);
        }

        public override void Fire(float spreadMultiplier = 1.0f)
        {
            if (muzzleBehaviour is null)
                return;

            if (playerCamera is null)
                return;

            const string stateName = "Fire";
            animator.Play(stateName, 0, 0.0f);
            ammunitionCurrent = Mathf.Clamp(ammunitionCurrent - 1, 0, magazineBehaviour.GetAmmunitionTotal());

            if (ammunitionCurrent == 0)
                SetSlideBack(1);

            muzzleBehaviour.Effect();

            for (var i = 0; i < shotCount; i++)
            {
                var spreadValue = Random.insideUnitSphere * (spread * spreadMultiplier);
                spreadValue.z = 0;
                spreadValue = playerCamera.TransformDirection(spreadValue);

                var a = Quaternion.Euler(playerCamera.eulerAngles + spreadValue);
                var projectile = Instantiate(prefabProjectile, muzzleBehaviour.transform.position, a);

                projectile.GetComponent<Projectile>()
                    .SetDamage(GetDamage());
                projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * projectileImpulse;
            }
        }

        public override void FillAmmunition(int amount)
        {
            var inventory = characterBehaviour.GetInventory() as Inventory;

            // if amount is 0 -> full reload
            var neededAmmo = amount == 0
                ? magazineBehaviour.GetAmmunitionTotal()
                : amount;

            if (YandexGame.savesData.Talents[29])
            {
                ammunitionCurrent += inventory.GetAmmoWithTalent29(AmmoType, neededAmmo * 2);
            }
            else
            {
                var delta = neededAmmo > inventory.GetAmmo(AmmoType)
                    ? inventory.GetAmmo(AmmoType)
                    : neededAmmo;

                if (delta + ammunitionCurrent > GetAmmunitionTotal())
                    delta = GetAmmunitionTotal() - ammunitionCurrent;
                inventory.TakeAmmo(AmmoType, -delta);
                ammunitionCurrent += delta;
            }


            // Debug.Log(inventory.CheckAmmo(AmmoType));
            // if (!inventory.CheckAmmo(AmmoType))
            //    inventory.GetComponent<Character>().AnimationEndedReload();
        }

        public override void SetSlideBack(int back)
        {
            //Set the slide back bool.
            const string boolName = "Slide Back";
            animator.SetBool(boolName, back != 0);
        }

        public override void EjectCasing()
        {
            //Spawn casing prefab at spawn point.
            if (prefabCasing != null && socketEjection != null)
                Instantiate(prefabCasing, socketEjection.position, socketEjection.rotation);
        }

        public override Animator GetAnimator() => animator;
        public override bool CanReloadAimed() => canReloadAimed;
        public override Sprite GetSpriteBody() => spriteBody;
        public override float GetMultiplierMovementSpeed() => multiplierMovementSpeed;
        public override AudioClip GetAudioClipHolster() => audioClipHolster;
        public override AudioClip GetAudioClipUnholster() => audioClipUnholster;
        public override AudioClip GetAudioClipReload() => audioClipReload;
        public override AudioClip GetAudioClipReloadEmpty() => audioClipReloadEmpty;
        public override AudioClip GetAudioClipReloadOpen() => audioClipReloadOpen;
        public override AudioClip GetAudioClipReloadInsert() => audioClipReloadInsert;
        public override AudioClip GetAudioClipReloadClose() => audioClipReloadClose;
        public override AudioClip GetAudioClipFireEmpty() => audioClipFireEmpty;
        public override AudioClip GetAudioClipBoltAction() => audioClipBoltAction;
        public override AudioClip GetAudioClipFire() => muzzleBehaviour.GetAudioClipFire();
        public override int GetAmmunitionCurrent() => ammunitionCurrent;
        public override int GetAmmunitionTotal() => magazineBehaviour.GetAmmunitionTotal();
        public override bool HasCycledReload() => cycledReload;
        public override bool IsAutomatic() => automatic;
        public override bool IsBoltAction() => boltAction;
        public override bool GetAutomaticallyReloadOnEmpty() => automaticReloadOnEmpty;
        public override float GetAutomaticallyReloadOnEmptyDelay() => automaticReloadOnEmptyDelay;
        public override bool CanReloadWhenFull() => canReloadWhenFull;
        public override float GetRateOfFire() => roundsPerMinutes;
        public override bool IsFull() => ammunitionCurrent == magazineBehaviour.GetAmmunitionTotal();

        public override bool HasAmmunition()
        {
            return ammunitionCurrent > 0;
        }

        public override RuntimeAnimatorController GetAnimatorController() => controller;
        public override WeaponAttachmentManagerBehaviour GetAttachmentManager() => attachmentManager;
    }

    public enum WeaponType
    {
        Pistol,
        SniperRifle,
        AutoRifle,
        ShotGun,
        SMG,
        Grenade,
    }
}