using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using YG;

public class CharactersShower : MonoBehaviour
{
    public event Action<int> CharacterChanged;

    [SerializeField] private List<Transform> _idlePlaces = new();
    [SerializeField] private List<GameObject> _readyCharacters = new();
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;

    [SerializeField] private Mover _mover;
    [SerializeField] private Rotater _rotater;

    private int index;
    private bool inited;

    private void Awake()
    {
        if (YandexGame.SDKEnabled)
        {
            index = YandexGame.savesData.CharacterIndex;
            inited = true;
        }

        _leftButton.onClick.AddListener(onLeftButton);
        _rightButton.onClick.AddListener(onRightButton);
    }

    public void Show()
    {
        CheckIndex();
        _mover.Move(_idlePlaces[index]);
        _rotater.Rotate(_idlePlaces[index]);
        CharacterChanged?.Invoke(index);
    }

    public void ShowReadyCharacter()
    {
        CheckIndex();
        for (int i = 0; i < _readyCharacters.Count; i++)
        {
            _readyCharacters[i].SetActive(i == index);
        }
    }

    private void CheckIndex()
    {
        if (!inited)
        {
            index = YandexGame.SDKEnabled ? YandexGame.savesData.CharacterIndex : 0;
            inited = true;
        }
    }
    
    private void onLeftButton()
    {
        if (index - 1 < 0)
        {
            index = _idlePlaces.Count - 1;
        }
        else
        {
            index--;
        }

        Show();
    }

    private void onRightButton()
    {
        if (_idlePlaces.Count <= index + 1)
        {
            index = 0;
        }
        else
        {
            index++;
        }

        Show();
    }
}