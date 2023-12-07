using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    public class Inventory : InventoryBehaviour
    {
        #region FIELDS

        public WeaponBehaviour[] weapons;
        [SerializeField] private Character _character;

        private WeaponBehaviour equipped;
        private int equippedIndex = -1;

        #endregion

        [field: SerializeField] public int[] AmmunitionMax { get; private set; } = { 120, 180, 150, 90, 21, 10, 3 };
        [field: SerializeField] public int[] Ammunition { get; private set; } = { 120, 180, 150, 90, 21, 10, 3 };

        #region METHODS

        public int GetCurrentAmmo() => Ammunition[(int)GetCurrentWeapon.AmmoType];
        public int GetCurrentMaxAmmo() => AmmunitionMax[(int)GetCurrentWeapon.AmmoType];
        public Weapon GetCurrentWeapon => equipped as Weapon;

        public void FillAmmo()
        {
            for (int i = 0; i < Ammunition.Length; i++)
                Ammunition[i] = AmmunitionMax[i];
        }

        public int TryTakeAmmo(Ammo type, int value)
        {
            var i = (int)type;
            var need = AmmunitionMax[i] - Ammunition[i];
            
            if (need >= value)
            {
                Ammunition[i] += value;
                return value;
            }
            Ammunition[i] = AmmunitionMax[i];
            return need;
        }

        public void TakeAmmo(Ammo type, int value)
        {
            var i = (int)type;
            Ammunition[i] = Mathf.Clamp(Ammunition[i] + value, 0, AmmunitionMax[i]);
        }

        public int GetAmmo(Ammo type, int need)
        {
            var i = (int)type;
            if (Ammunition[i] >= need)
            {
                Ammunition[i] -= need;
                return need;
            }

            var toReturn = Ammunition[i];
            Ammunition[i] = 0;
            return toReturn;
        }

        public int GetAmmo(Ammo type)
        {
            return Ammunition[(int)type];
        }

        public bool CheckAmmo(Ammo type)
        {
            return Ammunition[(int)type] > 0;
        }

        private void DropWeapon(GameObject go)
        {
            var weapon = weapons[equippedIndex].GetComponent<Weapon>();
            var dropWeapon = Instantiate(weapon.DropWeapon, weapon.transform.position, weapon.transform.rotation);

            //Ammo
            dropWeapon.Ammo = weapon.ammunitionCurrent;
            //Attachments
            var wam = weapon.GetComponent<WeaponAttachmentManager>();
            var dwam = dropWeapon.GetComponent<WeaponAttachmentManager>();

            dwam.SetScope(wam.GetScopeIndex);
            dwam.SetGrip(wam.GetGripIndex);
            dwam.SetLaser(wam.GetLaserIndex);
            dwam.SetMuzzle(wam.GetMuzzleIndex);

            Destroy(weapons[equippedIndex].gameObject);
            Destroy(go);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void ChangeWeapon(WeaponBehaviour wb, GameObject go)
        {
            var ammo = go.GetComponent<DropWeapon>().Ammo;
            DropWeapon(go);
            weapons[equippedIndex] = Instantiate(wb, transform);

            ((Weapon)weapons[equippedIndex]).ammunitionCurrent = ammo;

            var am = go.GetComponent<WeaponAttachmentManager>();
            var nam = weapons[equippedIndex].GetComponent<WeaponAttachmentManager>();
            nam.SetScope(am.GetScopeIndex);
            nam.SetGrip(am.GetGripIndex);
            nam.SetLaser(am.GetLaserIndex);
            nam.SetMuzzle(am.GetMuzzleIndex);

            ChangeEquip();
            _character.RefreshWeaponSetup();
        }

        public WeaponBehaviour[] InitWeapons()
        {
            weapons = GetComponentsInChildren<WeaponBehaviour>(true);
            return weapons;
        }

        public override void Init(int equippedAtStart = 0)
        {
            foreach (WeaponBehaviour weapon in weapons)
                weapon.gameObject.SetActive(false);
            Equip(equippedAtStart);
        }

        private WeaponBehaviour ChangeEquip()
        {
            equipped = weapons[equippedIndex];
            equipped.gameObject.SetActive(true);
            return equipped;
        }

        public override WeaponBehaviour Equip(int index)
        {
            if (weapons == null)
                return equipped;
            if (index > weapons.Length - 1)
                return equipped;
            if (equippedIndex == index)
                return equipped;
            if (equipped != null)
                equipped.gameObject.SetActive(false);
            equippedIndex = index;
            equipped = weapons[equippedIndex];
            equipped.gameObject.SetActive(true);
            return equipped;
        }

        #endregion

        #region Getters

        public override int GetLastIndex()
        {
            int newIndex = equippedIndex - 1;
            if (newIndex < 0)
                newIndex = weapons.Length - 1;
            return newIndex;
        }

        public override int GetNextIndex()
        {
            int newIndex = equippedIndex + 1;
            if (newIndex > weapons.Length - 1)
                newIndex = 0;
            return newIndex;
        }

        public override WeaponBehaviour GetEquipped() => equipped;
        public override int GetEquippedIndex() => equippedIndex;

        #endregion
    }

    public enum Ammo
    {
        Seven = 0,
        Five = 1,
        Nine = 2,
        Eleven,
        Twelve,
        Grenade,
        Raket
    }
}