using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using YG;

public class InputRebinder : MonoBehaviour, IPointerEnterHandler
{
    public event Action RebindCompleted;
    public event Action RebindCCanceled;
    public event Action Entered;

    [SerializeField] private InputActionAsset _inputActionAsset;
    [SerializeField] private string _mapName;
    [SerializeField] private string _actionName;
    [SerializeField] private int _bindingIndex;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Type _type;

    private InputAction inputAction;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;
    private InputActionMap map;
    private string oldBind;

    private enum Type
    {
        Button,
        Composite,
    }

    private void Awake()
    {
        map = _inputActionAsset.FindActionMap(_mapName);
        inputAction = map.FindAction(_actionName);

        if (_inputActionAsset == null | inputAction == null)
            throw new Exception("InputAction PB");
    }

    private void Start()
    {
        UpdateText();
    }

    public void StartRebinding()
    {
        //Debug.Log("StartRebinding");
        //PrintBinds();
        oldBind = GetCurrentBinding().effectivePath;
        SetActionMap("UI");

        if (_type == Type.Button)
        {
            rebindingOperation = inputAction.PerformInteractiveRebinding()
                // .WithControlsExcluding("Mouse")
                .WithCancelingThrough("<Keyboard>/escape")
                .OnMatchWaitForAnother(.1f)
                .OnComplete(operation => RebindComplete())
                .OnCancel(operation =>
                {
                    SetActionMap("Player");
                    RebindCCanceled?.Invoke();
                })
                .Start();
        }
        else if (_type == Type.Composite)
        {
            rebindingOperation = inputAction.PerformInteractiveRebinding()
                .WithTargetBinding(_bindingIndex)
                .WithCancelingThrough("<Keyboard>/escape")
                .OnMatchWaitForAnother(.1f)
                .OnComplete(operation => RebindComplete())
                .OnCancel(operation =>
                {
                    SetActionMap("Player");
                    RebindCCanceled?.Invoke();
                })
                .Start();
        }
    }

    private static void SetActionMap(string mapName)
    {
        var playerInput = FindObjectsByType<PlayerInput>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        if (playerInput.Length == 1)
        {
            playerInput[0].SwitchCurrentActionMap(mapName);
        }
    }

    private void RebindComplete()
    {
        UpdateText();
        // Debug.Log("RebindComplete");
        // PrintBinds();
        var currentBind = GetCurrentBinding();

        for (var i = 0; i < map.bindings.Count; i++)
        {
            if (map.bindings[i].effectivePath == currentBind.effectivePath
                && !BindingEqual(map.bindings[i], currentBind))
            {
                var newBind = map.bindings[i];
                newBind.overridePath = oldBind;
                map.ApplyBindingOverride(i, newBind);
            }
        }

        //Debug.Log("Unbind");
        //PrintBinds();
        rebindingOperation.Dispose();
        SetActionMap("Player");
        RebindCompleted?.Invoke();
    }

    private bool BindingEqual(InputBinding a, InputBinding b) => a.name == b.name && a.action == b.action;

    private void PrintBinds()
    {
        Debug.Log(string.Join('\n', map.bindings.Select(bind => $"{bind.name}-{bind.action}-{bind.effectivePath}")));
    }

    private InputBinding GetCurrentBinding()
    {
        var bindingIndex = _type switch
        {
            Type.Button => inputAction.GetBindingIndexForControl(inputAction.controls[0]),
            Type.Composite => _bindingIndex,
            _ => 0
        };
        return inputAction.bindings[bindingIndex];
    }

    public void UpdateText()
    {
        var name = InputControlPath.ToHumanReadableString(
            GetCurrentBinding().effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
        switch (YandexGame.EnvironmentData.language)
        {
            case "en":
                switch (name)
                {
                    case "Left Button":
                        name = "LMB";
                        break; 
                    case "Right Button":
                        name = "RMB";
                        break; 
                    case "Middle Button":
                        name = "MMB";
                        break; 
                    case "Left Shift":
                        name = "L.Shift";
                        break; 
                    case "Right Shift":
                        name = "R.Shift";
                        break;
                    case "Left Control":
                        name = "L.Control";
                        break;
                    case "Right Control":
                        name = "R.Control";
                        break;
                    case "Left Alt":
                        name = "L.Alt";
                        break;
                    case "Right Alt":
                        name = "R.Alt";
                        break;
                }
                break;
            case "ru":
                switch (name)
                {
                    case "Left Button":
                        name = "ЛКМ";
                        break; 
                    case "Right Button":
                        name = "ПКМ";
                        break; 
                    case "Middle Button":
                        name = "Колесико";
                        break; 
                    case "Space":
                        name = "Пробел";
                        break;
                    case "Left Shift":
                        name = "Л.Шифт";
                        break; 
                    case "Right Shift":
                        name = "П.Шифт";
                        break;
                    case "Left Control":
                        name = "Л.Контрол";
                        break;
                    case "Right Control":
                        name = "П.Контрол";
                        break;
                    case "Caps Lock":
                        name = "Капс";
                        break;
                    case "Left Alt":
                        name = "Л.Алт";
                        break;
                    case "Right Alt":
                        name = "П.Алт";
                        break;
                }
                break;
            case "tr":
                switch (name)
                {
                    case "Left Button":
                        name = "Sol Fare Tuşu";
                        break; 
                    case "Right Button":
                        name = "Sağ Fare Tuşu";
                        break; 
                    case "Middle Button":
                        name = "Orta Fare Tuşu";
                        break; 
                    case "Space":
                        name = "Uzay";
                        break;
                    case "Left Shift":
                        name = "Sol Shift";
                        break; 
                    case "Right Shift":
                        name = "Sağ Shift";
                        break;
                    case "Left Control":
                        name = "Sol Kontrol";
                        break;
                    case "Right Control":
                        name = "Sağ Kontrol";
                        break;
                    case "Caps Lock":
                        name = "Caps Lock";
                        break;
                    case "Left Alt":
                        name = "Sol Alt";
                        break;
                    case "Right Alt":
                        name = "Sağ Alt";
                        break;
                }
                break;
            case "es":
                switch (name)
                {
                    case "Left Button":
                        name = "BIM";
                        break; 
                    case "Right Button":
                        name = "BDM";
                        break; 
                    case "Middle Button":
                        name = "BCM";
                        break; 
                    case "Space":
                        name = "Espacio";
                        break;
                    case "Left Shift":
                        name = "L.Shift";
                        break; 
                    case "Right Shift":
                        name = "D.Shift";
                        break;
                    case "Left Control":
                        name = "L.Control";
                        break;
                    case "Right Control":
                        name = "D.Control";
                        break;
                    case "Caps Lock":
                        name = "Caps Lock";
                        break;
                    case "Left Alt":
                        name = "L.Alt";
                        break;
                    case "Right Alt":
                        name = "D.Alt";
                        break;
                }
                break;
            case "de":
                switch (name)
                {
                    case "Left Button":
                        name = "LM";
                        break; 
                    case "Right Button":
                        name = "RM";
                        break; 
                    case "Middle Button":
                        name = "MM";
                        break; 
                    case "Space":
                        name = "Raum";
                        break;
                    case "Left Shift":
                        name = "L.Shift";
                        break; 
                    case "Right Shift":
                        name = "R.Shift";
                        break;
                    case "Left Control":
                        name = "L.Control";
                        break;
                    case "Right Control":
                        name = "R.Control";
                        break;
                    case "Caps Lock":
                        name = "Caps Lock";
                        break;
                    case "Left Alt":
                        name = "L.Alt";
                        break;
                    case "Right Alt":
                        name = "R.Alt";
                        break;
                }
                break;
        }

        _text.text = name;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        Entered?.Invoke();
    }
}