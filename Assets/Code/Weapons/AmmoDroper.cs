using InfimaGames.LowPolyShooterPack;
using UnityEngine;

public class AmmoDroper : MonoBehaviour
{
    [SerializeField] private AudioClip _clip;

    private void OnTriggerEnter(Collider other)
    {
        var hero = other.GetComponent<Character>();
        if (!hero)
            return;

        var dropWeapon = GetComponentInParent<DropWeapon>();
        if (dropWeapon.Ammo == 0)
            return;

        var inventory = hero.GetInventory() as Inventory;
        var lose = inventory.TryTakeAmmo(dropWeapon.AmmoType, dropWeapon.Ammo);
        dropWeapon.Ammo -= lose; 

        if (lose > 0)
        {
            var source = GetComponentInParent<AudioSource>();
            source.PlayOneShot(_clip);
        }
    }
}