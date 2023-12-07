using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using YG;

public class WeaponsSection : MonoBehaviour
{
    [SerializeField, Min(0)] private int _section = 0;

    [SerializeField] private List<int> Costs = new();

    [SerializeField] private Transform _parent;
    [SerializeField] private Transform _sectionsMenu;
    [SerializeField] private Transform _arrows;
    [SerializeField] private AttachmentsMenu _attachmentsMenu;
    [SerializeField] private Button _buyButton;
    [SerializeField] private Button _choseButton;
    [SerializeField] private Button _attachmentsButton;
    [SerializeField] private AudioSource _source;
    
    private Button button;
    private WeaponsShower shower;
    private int index;

    private void Awake()
    {
        shower = GetComponent<WeaponsShower>();
        shower.WeaponChanged += SetActionButton;
    }

    private void SetActionButton(int i)
    {
        index = i;
        HideButton();
        if (YandexGame.savesData.ChosenWeapons[_section][i])
        {
            button = Instantiate(_attachmentsButton, _parent);
            button.onClick.AddListener(ShowAttachments);
        }
        else if (YandexGame.savesData.OpenedWeapons[_section][i])
        {
            button = Instantiate(_choseButton, _parent);
            button.onClick.AddListener(Chose);
        }
        else
        {
            button = Instantiate(_buyButton, _parent);

            button.GetComponentInChildren<TMP_Text>().text = Costs[i].ToString() + '$';
            button.onClick.AddListener(TryBuy);
        }
        button.GetComponentInChildren<ButtonSoundPlayer>().SetSource(_source);
    }

    private void ShowAttachments()
    {
        _sectionsMenu.gameObject.SetActive(false);
        _arrows.gameObject.SetActive(false);
        HideButton();

        var attachmentsMenu = Instantiate(_attachmentsMenu, _parent);
        attachmentsMenu.Init(GetComponent<WeaponsShower>().GetWeapons()[index].GetComponent<InventoryWeapon>());
        attachmentsMenu.GetComponentInChildren<ButtonSoundPlayer>().SetSource(_source);

        attachmentsMenu.Back.onClick.AddListener(GetComponent<WeaponsShower>()
            .GetWeapons()[index].GetComponent<InventoryWeapon>().ChosenCurrentAttachments);
        attachmentsMenu.Back.onClick.AddListener(() => SetActionButton(index));
        attachmentsMenu.Back.onClick.AddListener(() => _sectionsMenu.gameObject.SetActive(true));
        attachmentsMenu.Back.onClick.AddListener(() => _arrows.gameObject.SetActive(true));
    }

    private void Chose()
    {
        if (_section >= 3)
        {
            for (var i = 3; i < YandexGame.savesData.ChosenWeapons.Length; i++)
            for (var j = 0; j < YandexGame.savesData.ChosenWeapons[i].Length; j++)
            {
                YandexGame.savesData.ChosenWeapons[i][j] = false;
            }
        }

        if (_section < 3)
        {
            for (var i = 0; i < 3; i++)
            for (var j = 0; j < YandexGame.savesData.ChosenWeapons[i].Length; j++)
            {
                YandexGame.savesData.ChosenWeapons[i][j] = false;
            }
        }

        YandexGame.savesData.ChosenWeapons[_section][index] = true;
        YandexGame.SaveProgress();
        SetActionButton(index);
    }

    private void TryBuy()
    {
        if (!MoneyController.Instanse.Check(Costs[index]))
            return;
        MoneyController.Instanse.ChangeAmount(-Costs[index]);
        YandexGame.savesData.OpenedWeapons[_section][index] = true;
        YandexGame.SaveProgress();
        SetActionButton(index);
    }

    private void OnDisable() => HideButton();

    private void HideButton()
    {
        if (button)
            Destroy(button.gameObject);
        button = null;
    }
}