using InfimaGames.LowPolyShooterPack;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableInterface : MonoBehaviour
{
    public static InteractableInterface instance;

    [SerializeField] private TMP_Text _text;
    [SerializeField] private InputActionAsset _inputActionAsset;

    private void Awake()
    {
        instance = this;
        Hide();
    }
    
    public void Show()
    {
        SetKey();
        gameObject.SetActive(true);
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    private void SetKey()
    {
        var inputAction = _inputActionAsset.FindActionMap("Player").FindAction("Interact");

        _text.text = InputControlPath.ToHumanReadableString(
            inputAction.bindings[inputAction.GetBindingIndexForControl(
                inputAction.controls[0])].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
    }
}
