using InfimaGames.LowPolyShooterPack;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponInteractableInterface : MonoBehaviour
{
    public static WeaponInteractableInterface instance;
    [SerializeField] private Weapon2DView _current;
    [SerializeField] private Weapon2DView _new;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private InputActionAsset _inputActionAsset;

    private void Awake()
    {
        instance = this;
        Hide();
    }

    public void Show(GameObject hero, DropWeapon dropWeapon)
    {
        gameObject.SetActive(true);
        var currentWeapon = hero.GetComponent<Character>().GetInventory().GetEquipped();
        _current.Show(currentWeapon.gameObject, currentWeapon.GetSpriteBody());
        _new.Show(dropWeapon.gameObject, dropWeapon.GetSpriteBody());
        SetKey();
    }

    private void SetKey()
    {
        var inputAction = _inputActionAsset.FindActionMap("Player").FindAction("Interact");

        _text.text = InputControlPath.ToHumanReadableString(
            inputAction.bindings[inputAction.GetBindingIndexForControl(
                inputAction.controls[0])].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
    }

    public void Hide()
    {
        _new.Hide();
        _current.Hide();
        gameObject.SetActive(false);
    }
}