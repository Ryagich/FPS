using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;
using TMPro;

public class AttachmentsMenu : MonoBehaviour
{
    //0 - Scopes; 1 - Muzzles; 2 - Lasers; 3 - Grips;
    public List<Button> AttachmentsButtons = new();
    public Button Back;
    public Button LeftArrow;
    public Button RightArrow;

    [SerializeField] private Button _buyButton;
    [SerializeField] private Button _choseButton;
    [SerializeField] private GameObject _shosen;
    [SerializeField] private AudioSource _source;

    private InventoryWeapon weapon;
    private int section;
    private GameObject showButton;

    public void Init(InventoryWeapon weapon)
    {
        this.weapon = weapon;

        for (var i = 0; i < AttachmentsButtons.Count; i++)
        {
            InitButton(AttachmentsButtons[i], i);
        }
    }

    private void RemoveArrowsListeners()
    {
        LeftArrow.onClick.RemoveAllListeners();
        RightArrow.onClick.RemoveAllListeners();
    }

    private void InitButton(Button button, int s)
    {
        button.onClick.AddListener(() => LeftArrow.gameObject.SetActive(true));
        button.onClick.AddListener(() => RightArrow.gameObject.SetActive(true));
        button.onClick.AddListener(RemoveArrowsListeners);

        button.onClick.AddListener(() =>
            LeftArrow.onClick.AddListener(OnLeftArrow));
        button.onClick.AddListener(() =>
            RightArrow.onClick.AddListener(OnRightArrow));

        button.onClick.AddListener(() => section = s);
        button.onClick.AddListener(() => weapon.ChosenCurrentAttachments());
        button.onClick.AddListener(() => ShowButton(GetCurrI()));
    }

    private void OnLeftArrow()
    {
        var i = GetCurrI();
        var nextIndex = i - 1 < (section == 1 ? 0 : -1)
            ? weapon.Attachments[section].Length - 1
            : i - 1;
        SetCurrAttachment(nextIndex);
        ShowButton(nextIndex);
    }

    private void OnRightArrow()
    {
        var i = GetCurrI();
        var nextIndex = weapon.Attachments[section].Length <= i + 1
            ? (section == 1 ? 0 : -1)
            : i + 1;
        SetCurrAttachment(nextIndex);
        ShowButton(nextIndex);
    }

    public void HideButton()
    {
        if (showButton)
            Destroy(showButton.gameObject);
        showButton = null;
    }

    private void ShowButton(int i)
    {
        HideButton();
        if (i<0||YandexGame.savesData.ChosenAttachments[weapon.WeaponIndex][section] == i)
        {
            // if i<0 => index == -1. its nothing/default. its ok
            showButton = Instantiate(_shosen, transform);
        }
        else if (YandexGame.savesData.OpenedAttachments[weapon.WeaponIndex][section][i])
        {
            showButton = Instantiate(_choseButton.gameObject, transform);
            showButton.GetComponent<Button>().onClick.AddListener(() =>Chose(i));
        }
        else
        {
            showButton = Instantiate(_buyButton.gameObject, transform);
            showButton.GetComponent<Button>().onClick.AddListener(() => TryBuy(i));
            showButton.GetComponentInChildren<TMP_Text>().text = GetCurrCost(i).ToString() +'$';
        }
        showButton.GetComponentInChildren<ButtonSoundPlayer>()?.SetSource(_source);
    }

    private void Chose(int i)
    {
        YandexGame.savesData.ChosenAttachments[weapon.WeaponIndex][section] = i;
        YandexGame.SaveProgress();
        ShowButton(i);
    }

    private void TryBuy(int i)
    {
        if (!MoneyController.Instanse.Check(GetCurrCost(i)))
            return;
        MoneyController.Instanse.ChangeAmount(-GetCurrCost(i));
        YandexGame.savesData.OpenedAttachments[weapon.WeaponIndex][section][i] = true;
        YandexGame.savesData.ChosenAttachments[weapon.WeaponIndex][section] = i;
        YandexGame.SaveProgress();
        ShowButton(i);
    }

    private int GetCurrCost(int i)
    {
        if (section == 0)
            return AttachmentsCostsHolder.Instance.CostsScopes[i];
        if (section == 1)
            return AttachmentsCostsHolder.Instance.CostsMuzzle[i];
        if (section == 2)
            return AttachmentsCostsHolder.Instance.CostsLaser[i];
        return AttachmentsCostsHolder.Instance.CostsGrip[i];
    }

    private void SetCurrAttachment(int i)
    {
        if (section == 0)
            weapon.Manager.SetScope(i);
        if (section == 1)
            weapon.Manager.SetMuzzle(i);
        if (section == 2)
            weapon.Manager.SetLaser(i);
        if (section == 3)
            weapon.Manager.SetGrip(i);
    }

    private int GetCurrI()
    {
        if (section == 0)
            return weapon.Manager.GetScopeIndex;
        if (section == 1)
            return weapon.Manager.GetMuzzleIndex;
        if (section == 2)
            return weapon.Manager.GetLaserIndex;
        return weapon.Manager.GetGripIndex;
    }

    public void DestroyMenu()
    {
        Destroy(gameObject);
    }
}