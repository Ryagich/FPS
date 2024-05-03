using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
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
    [SerializeField] private SpecificationHolder _holder;
    [SerializeField] private List<Specification> _specifications = new();
    [Space] 
    [SerializeField] private string _firstSkillCooldownTextBefore;
    [SerializeField] private string _firstSkillCooldownTextAfter;
    [SerializeField] private string _secondSkillCooldownText;

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
        ShowSpecification();

        CharacterChanged?.Invoke(index);
    }

    private void ShowSpecification()
    {
        _holder.Name.text = _specifications[index].name;
        _holder.History.text = _specifications[index].history;
        _holder.Health.text = $"{_specifications[index].health}";
        _holder.Armour.text  =  $"{_specifications[index].armour}";
        _holder.Speed.text =  $"x{_specifications[index].speed.ToString("F1",CultureInfo.InvariantCulture)}";
        _holder.Ammo.text = $"x{_specifications[index].ammo.ToString("F1",CultureInfo.InvariantCulture)}";

        _holder.FirstSkillName.text = _specifications[index].skills[0].name;
        _holder.FirstSkillCooldown.text = $"{_firstSkillCooldownTextBefore} {_specifications[index].skills[0].value} {_firstSkillCooldownTextAfter}";
        _holder.FirstSkillDescription.text = _specifications[index].skills[0].description;

        _holder.SecondSkillName.text = _specifications[index].skills[1].name;
        _holder.SecondSkillCount.text =  $"{_secondSkillCooldownText} {_specifications[index].skills[1].value}";
        _holder.SecondSkillDescription.text = _specifications[index].skills[1].description;

    }

    public void ShowReadyCharacter()
    {
        CheckIndex();
        for (int i = 0; i < _readyCharacters.Count; i++)
        {
            _readyCharacters[i].SetActive(i == YandexGame.savesData.CharacterIndex);
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