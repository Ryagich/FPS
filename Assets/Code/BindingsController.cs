using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BindingsController : MonoBehaviour
{
    [SerializeField] private ButtonSoundPlayer _soundPlayer;
    [SerializeField] private Transform _settingsCanvas;

    [Header("Colors For Bindings Button")]
    [SerializeField] private Color _disableColorActiveButton;
    [SerializeField] private Color _defDisableColor;

    [Header("Colors For Settings Button")]
    [SerializeField] private Color _highlightColor;
    [SerializeField] private Color _disableHighlightColor;
    
    [Header("Debug Serialize")]
    [SerializeField] private List<Button> _settingsButtons;
    [SerializeField] private List<Button> _buttons;
    private void Awake()
    {
        _settingsButtons = _settingsCanvas.GetComponentsInChildren<Button>().ToList();
        _buttons = GetComponentsInChildren<Button>().ToList();
        foreach (var button in _buttons)
        {
            button.onClick.AddListener(() => DisableButtons(button));
            button.onClick.AddListener(() => _soundPlayer.PlayClick());

            var rebinder = button.GetComponent<InputRebinder>();
            rebinder.RebindCompleted += () => ActivateButtons(button);
            rebinder.RebindCCanceled += () => ActivateButtons(button);
            rebinder.Entered += PlayHover;
        }
    }

    private void PlayHover()
    {
        _soundPlayer.PlayHover();
    }
    
    private void DisableButtons(Button button)
    {
        foreach (var b in _buttons)
        {
            if (b == button)
            {
                var colors = button.colors;
                colors.disabledColor = _disableColorActiveButton;
                button.colors = colors;
            }
            
            b.GetComponent<InputRebinder>().Entered -= PlayHover;
            b.interactable = false;
        }

        foreach (var b in _settingsButtons)
        {
            b.GetComponent<EventTrigger>().enabled = false;
            
            var colors = b.colors;
            colors.highlightedColor = _disableHighlightColor;
            b.colors = colors;
        }
    }

    private void ActivateButtons(Button button)
    {
        foreach (var b in _buttons)
        {
            if (b == button)
            {
                var colors = button.colors;
                colors.disabledColor = _defDisableColor;
                button.colors = colors;
            }
            
            b.GetComponent<InputRebinder>().Entered += PlayHover;
            b.interactable = true;
            b.GetComponent<InputRebinder>().UpdateText();
        }
        
        foreach (var b in _settingsButtons)
        {
            b.GetComponent<EventTrigger>().enabled = true;
            
            var colors = b.colors;
            colors.highlightedColor = _highlightColor;
            b.colors = colors;
        }
        
        EventSystem.current.SetSelectedGameObject(null);
    }
}