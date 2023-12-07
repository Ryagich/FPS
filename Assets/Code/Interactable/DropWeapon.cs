using InfimaGames.LowPolyShooterPack;
using UnityEngine;

public class DropWeapon : MonoBehaviour
{
    public int Ammo;

    [SerializeField] private WeaponBehaviour _weapon;
    [field: SerializeField] public Ammo AmmoType { get; private set; }

    public void Use(GameObject hero, GameObject _)
    {
        hero.GetComponent<Character>().OnTryPickupWeapon(_weapon, gameObject);
        Hide();
    }

    public void Show(GameObject hero, GameObject weapon)
    {
        WeaponInteractableInterface.instance.Show(hero, this);
    }

    public Sprite GetSpriteBody()
    {
        return _weapon.GetComponent<WeaponBehaviour>().GetSpriteBody();
    }

    public void Hide()
    {
        WeaponInteractableInterface.instance.Hide();
    }
}