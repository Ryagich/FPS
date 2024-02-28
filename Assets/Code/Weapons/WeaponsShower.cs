using System;
using System.Collections.Generic;
using InfimaGames.LowPolyShooterPack;
using UnityEngine;
using UnityEngine.UI;

public class WeaponsShower : MonoBehaviour
{
    public event Action<int> WeaponChanged;
    [SerializeField] private List<WeaponAttachmentManager> _weapons = new();
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;

    private int index = 0;
    
    public List<WeaponAttachmentManager> GetWeapons() => _weapons;

    private void OnEnable()
    {
        _leftButton.onClick.AddListener(onLeftButton);
        _rightButton.onClick.AddListener(onRightButton);
    }

    private void OnDisable()
    {
        _leftButton.onClick.RemoveListener(onLeftButton);
        _rightButton.onClick.RemoveListener(onRightButton);
    }

    public void Show()
    {
        _weapons[index].gameObject.SetActive(true);
        WeaponChanged?.Invoke(index);
    }

    public void Hide() => _weapons[index].gameObject.SetActive(false);

    public void onRightButton()
    {
        _weapons[index].gameObject.SetActive(false);
        if (_weapons.Count <= index + 1)
        {
            index = 0;
        }
        else
        {
            index++;
        }
        _weapons[index].gameObject.SetActive(true);
        WeaponChanged?.Invoke(index);
    }

    public void onLeftButton()
    {
        _weapons[index].gameObject.SetActive(false);
        if (index - 1 < 0)
        {
            index = _weapons.Count - 1;
        }
        else
        {
            index--;
        }

        _weapons[index].gameObject.SetActive(true);
        WeaponChanged?.Invoke(index);
    }
}