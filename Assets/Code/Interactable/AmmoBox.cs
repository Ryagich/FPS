using InfimaGames.LowPolyShooterPack;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    public void Use(GameObject hero, GameObject _)
    {
        var inventory = hero.GetComponent<InventoryIniter>()._inventory;
        inventory.FillAmmo();
        var character = hero.GetComponent<Character>();
        character.FillGrenades();
    }
}