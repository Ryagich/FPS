using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

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
                .WithControlsExcluding("Mouse")
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
        _text.text = InputControlPath.ToHumanReadableString(
            GetCurrentBinding().effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        Entered?.Invoke();
    }
}